using UnityEngine;
using System.Collections;

public class LaserRifle : MonoSingleton<LaserRifle>
{
    public GameObject bulletSource;
    public GameObject grenadeLauncher;
    public GameObject grenadePrefab;
	public MeshRenderer muzzleFlash;
	public AmmoCounter ammoCounter;
    public ParticleSystem hitParticleEffect;
    public GameObject bulletTrailPrefab;
    public float grenadeSpeed;
    public AudioClip pewSound;
    public AudioClip outOfAmmoSound;
    public AudioClip grenadeLauncherSound;
    [Range(0.01f, 0.5f)]
    public float
        shotPeriod;
    public int maxAmmo;
    public float recoil;
    public float handling;
    ShootingPhase shootingPhase;
    private float nextShot;
    private int _ammo;
    private float nextBulletHoleDepth = 0.002f;
	private float muzzleFlashBrightness;
    private Light muzzleFlashLight;

    public int ammo
    {
        get
        {
            return _ammo;
        }
        set
        {
            _ammo = Mathf.Clamp(value, 0, maxAmmo);
            ammoCounter.ammo = _ammo;
        }
    }

    private float spread;
    private float knockBack;
    private float grenadeCooldown = 1.0f;
    private float nextGrenade = 0.0f;

    void Start()
    {
        shootingPhase = ShootingPhase.NotShooting;
		nextShot = Time.fixedTime;
		ammoCounter.maxAmmo = maxAmmo;
        ammo = maxAmmo;
        muzzleFlashLight = transform.FindChild("MuzzleFlashLight").gameObject.GetComponent<Light>();
        muzzleFlashLight.intensity = 0.0f;
    }

    void Update()
    {
        if (nextGrenade > 0.0f)
            nextGrenade -= Time.deltaTime;

        if (Input.GetMouseButton(0) || Gamepad.instance.rightTrigger() > 0.75f)
        {
            if (shootingPhase == ShootingPhase.NotShooting)
                shootingPhase = ShootingPhase.ShootingStart;
        }
        else
        {
            if (shootingPhase == ShootingPhase.Shooting)
                shootingPhase = ShootingPhase.NotShooting;
        }

		muzzleFlash.material.SetColor("_TintColor", new Color(muzzleFlashBrightness, muzzleFlashBrightness, muzzleFlashBrightness, muzzleFlashBrightness));
		muzzleFlashBrightness = Mathf.Max(0.0f, muzzleFlashBrightness * 0.7f - Time.deltaTime);
        muzzleFlashLight.intensity = muzzleFlashBrightness * 5.0f;

        if (nextGrenade <= 0.0f && (Input.GetMouseButtonDown(2) || Gamepad.instance.justPressedRightShoulder()))
        {
            audio.PlayOneShot(grenadeLauncherSound);
            GameObject granade = Instantiate(grenadePrefab, grenadeLauncher.transform.position, grenadeLauncher.transform.rotation * Quaternion.AngleAxis(-90.0f, Vector3.right)) as GameObject;
            granade.rigidbody.velocity = transform.root.rigidbody.velocity + grenadeLauncher.transform.forward * grenadeSpeed;
            nextGrenade = grenadeCooldown;
        }
    }

    void FixedUpdate()
    {
        float effectiveHandling = handling;

        if (PlayerController.instance.isCrouching)
        {
            effectiveHandling *= 4.0f;
        }

        spread += PlayerController.instance.speed * 0.001f;
        knockBack *= Mathf.Exp(-Time.fixedDeltaTime * effectiveHandling);
        spread *= Mathf.Exp(-Time.fixedDeltaTime * effectiveHandling);
        
        if (Time.fixedTime >= nextShot && shootingPhase != ShootingPhase.NotShooting)
        {
            RaycastHit hitInfo;
            Vector3 start = bulletSource.transform.position;
            Vector3 dir = bulletSource.transform.forward;
            float angle = Random.Range(-Mathf.PI, Mathf.PI);
            float dist = Random.Range(0, spread);
            dir += transform.up * Mathf.Sin(angle) * dist;
            dir += transform.right * Mathf.Cos(angle) * dist;
            dir.Normalize();

            bool hit = Physics.Raycast(start, dir, out hitInfo, 10000.0f, Layers.playerAttack);
            shootingPhase = ShootingPhase.Shooting;
            nextShot = Time.fixedTime + shotPeriod;

            if (ammo <= 0)
            {
                audio.PlayOneShot(outOfAmmoSound);
            }
            else
            {
                --ammo;
                audio.PlayOneShot(pewSound);

				BulletTrail bulletTrail = (Instantiate(bulletTrailPrefab) as GameObject).GetComponent<BulletTrail>();
				bulletTrail.start = start;
				Vector3 endPosition;

                if (hit)
                {
                    //Debug.DrawLine(start, hitInfo.point, Color.yellow, 0.2f, true);
                    Damageable damagable = hitInfo.collider.GetComponent<Damageable>();

                    if (damagable != null)
                    {
                        damagable.DealDamage(dir * 11.0f);
                    }

                    MaterialHolder mat = hitInfo.collider.GetComponent<MaterialHolder>();

                    if (mat != null)
                    {
                        GameObject hole = Instantiate(mat.materialProperties.bulletHolePrefab, hitInfo.point + nextBulletHoleDepth * hitInfo.normal, Quaternion.LookRotation(hitInfo.normal)) as GameObject;
                        hole.transform.parent = hitInfo.transform;
                        BulletHoleManager.instance.AddBulletHole(hole);
                        nextBulletHoleDepth += 0.001f;
                        if (nextBulletHoleDepth > 0.01f)
                            nextBulletHoleDepth = 0.002f;
                    }

                    hitParticleEffect.transform.position = hitInfo.point;
                    hitParticleEffect.transform.forward = (Vector3.Reflect(dir, hitInfo.normal) + hitInfo.normal) / 2;
                    hitParticleEffect.Emit(10);
                    endPosition = hitInfo.point;
                }
                else
                {
                    //Debug.DrawRay(start, dir * 1000.0f, Color.yellow, 0.2f, true);
                    endPosition = start + dir * 1000.0f;
                }
				bulletTrail.end = endPosition;
				//bulletTrail.material.mainTextureScale = new Vector2((start - endPosition).magnitude / trailWidth * 0.25f, 1);

                spread += recoil;
                knockBack += recoil;
                FirstPersonCameraController.instance.verticalAngle += knockBack * 10.0f;
				muzzleFlashBrightness += 0.6f;
            }
        }

        transform.localRotation = Quaternion.Euler(new Vector3(-100.0f * knockBack, 0, 0));
    }

    enum ShootingPhase
    {
        NotShooting,
        ShootingStart,
        Shooting
    }
}
