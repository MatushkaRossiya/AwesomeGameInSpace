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
	public AudioClip meleeAttackSound;
	public AudioClip[] meleeScreamSound;
	public Animation handsAttack;
	public GameObject glowingCoils;
	public Gradient glowColor;
	public AnimationCurve glow;

    [Range(0.01f, 0.5f)]
    public float shotPeriod;
	public int maxAmmo { 
		get{
			return hasAmmoClipUpgrade ? 500 : 250;	
		}
	}
    public float recoil;
	public float handling;
	public GameObject GLModel;
	public bool hasAmmoClipUpgrade;			// TODO Save it
	public bool hasGrenadeLaucherUpgrade;	// TODO Save it
	private int _grenades;
	public int grenades{ 					// TODO Save it
		get {
			return _grenades;
		}
		set {
			_grenades = value;
			ammoCounter.grenades = value;
		}
	}
	public int maxGrenades = 3;


    private ShootingPhase shootingPhase;
    private float nextShot;
    private int _ammo;
    private float muzzleFlashBrightness;
	private Light muzzleFlashLight;
	private Material glowingCoilsMat;

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

    public bool isZoomed{ get; set; }

    private float spread;
    private float knockBack;
    private float grenadeCooldown = 1.0f;
    private float nextGrenade = 0.0f;
    private float zoomPhase;
    private float zoomTime = 0.5f;
    private Vector3 riflePosition;
    private Vector3 rifleZoomPosition;

	private float HTHduration;

    void Start()
    {
        shootingPhase = ShootingPhase.NotShooting;
        nextShot = Time.fixedTime;
        ammoCounter.maxAmmo = maxAmmo;
        ammo = maxAmmo;
		grenades = 0;
        muzzleFlashLight = transform.FindChild("MuzzleFlashLight").gameObject.GetComponent<Light>();
        muzzleFlashLight.intensity = 0.0f;
		glowingCoilsMat = glowingCoils.renderer.material;
        riflePosition = transform.localPosition;
        rifleZoomPosition = new Vector3(0.0195f, -0.075f, 0.0f);
    }

    void Update()
    {
        if (nextGrenade > 0.0f)
            nextGrenade -= Time.deltaTime;

		if ((Input.GetMouseButton(0) || Gamepad.instance.rightTrigger() > 0.75f)) {
			if (shootingPhase == ShootingPhase.NotShooting) {
				shootingPhase = ShootingPhase.ShootingStart;
			}
		}
		else if (shootingPhase == ShootingPhase.Shooting) {
			shootingPhase = ShootingPhase.NotShooting;
		}

		if((Input.GetKeyDown(KeyCode.X) || Gamepad.instance.justPressedLeftShoulder()) && 
			shootingPhase != ShootingPhase.HandToHand &&
			shootingPhase != ShootingPhase.HandToHandEnd &&
			!isZoomed) 
		{
			shootingPhase = ShootingPhase.HandToHand;
			handsAttack.Play();
			HTHduration = 0.0f;
			audio.PlayOneShot(meleeScreamSound[Random.Range(0, meleeScreamSound.Length)]);
		}

        if (hasGrenadeLaucherUpgrade &&
			grenades > 0 &&
			nextGrenade <= 0.0f && 
			(Input.GetMouseButtonDown(2) || Gamepad.instance.justPressedRightShoulder()))
        {
			--grenades;
            audio.PlayOneShot(grenadeLauncherSound);
            GameObject granade = Instantiate(grenadePrefab, grenadeLauncher.transform.position, grenadeLauncher.transform.rotation * Quaternion.AngleAxis(-90.0f, Vector3.right)) as GameObject;
            granade.rigidbody.velocity = transform.root.rigidbody.velocity + grenadeLauncher.transform.forward * grenadeSpeed;
            nextGrenade = grenadeCooldown;
        }

        muzzleFlash.material.SetColor("_TintColor", new Color(muzzleFlashBrightness, muzzleFlashBrightness, muzzleFlashBrightness, muzzleFlashBrightness));
        muzzleFlashLight.intensity = muzzleFlashBrightness * 5.0f;
		float ammoPercentage = (float)ammo / (float)maxAmmo;
		glowingCoilsMat.SetColor("_EmissionColor",
			(muzzleFlashBrightness + glow.Evaluate(ammoPercentage)) *
			glowColor.Evaluate(ammoPercentage));

        muzzleFlashBrightness = Mathf.Max(0.0f, muzzleFlashBrightness * 0.7f - Time.deltaTime);


		SetPos();
    }

    void FixedUpdate()
    {
        float effectiveHandling = handling;
        float effectiveRecoil = recoil;

        if (PlayerController.instance.isCrouching)
        {
            effectiveHandling *= 4.0f;
        }

        if (isZoomed)
        {
            effectiveRecoil *= 0.75f;
            effectiveHandling *= 1.25f;
        }

        spread += PlayerController.instance.speed * 0.006f;
        knockBack *= Mathf.Exp(-Time.fixedDeltaTime * effectiveHandling);
        spread *= Mathf.Exp(-Time.fixedDeltaTime * effectiveHandling);
        
        if (Time.fixedTime >= nextShot
			&& (shootingPhase == ShootingPhase.ShootingStart
			|| shootingPhase == ShootingPhase.Shooting))
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
                    Damageable damagable = hitInfo.collider.GetComponent<Damageable>();

                    if (damagable != null)
                    {
                        damagable.DealDamage(dir * 11.0f);
                    }

                    MaterialHolder mat = hitInfo.collider.GetComponent<MaterialHolder>();

                    if (mat != null)
                    {
						if (mat.materialProperties != null)
						{
                        	mat.materialProperties.Hit(hitInfo, dir);
						}
                    }

                    hitParticleEffect.transform.position = hitInfo.point;
                    hitParticleEffect.transform.forward = (Vector3.Reflect(dir, hitInfo.normal) + hitInfo.normal) / 2;
                    hitParticleEffect.Emit(10);
                    endPosition = hitInfo.point;
                }
                else
                {
                    endPosition = start + dir * 1000.0f;
                }
                bulletTrail.end = endPosition;

                spread += effectiveRecoil;
                knockBack += effectiveRecoil;
                FirstPersonCameraController.instance.verticalAngle += knockBack * 10.0f;
                muzzleFlashBrightness += 0.6f;
            }
		}
		else if (shootingPhase == ShootingPhase.HandToHand) {
			HTHduration += Time.fixedDeltaTime;
			if (HTHduration > 1.0f) {
				RaycastHit hit;
				if(Physics.SphereCast(
					transform.position,
					0.3f,
					transform.forward,
					out hit,
					1.0f,
					Layers.playerAttack))
				{
					audio.PlayOneShot(meleeAttackSound);
					Damageable dam = hit.collider.GetComponent<Damageable>();
					if (dam != null) {
						dam.DealDamage(transform.forward * 10.0f);
					}
				}
				shootingPhase = ShootingPhase.HandToHandEnd;
			}
		}
		else if (shootingPhase == ShootingPhase.HandToHandEnd) {
			HTHduration += Time.fixedDeltaTime;
			if (HTHduration > 1.75f) {
				shootingPhase = ShootingPhase.NotShooting;
			}
		}

        transform.localRotation = Quaternion.Euler(new Vector3(-100.0f * knockBack, 0, 0));
    }

	private float swingPhase = 0;
	private float swingSpeedMul = 0.15f;
	private float swingAmplitude = 0.005f;

	void SetPos() {
		Vector3 pos;
		zoomPhase = Mathf.Clamp01(zoomPhase + (isZoomed ? 1 : -1) * Time.deltaTime / zoomTime);
		float t = MathfX.sinerp(0, 1, zoomPhase);
		pos = Vector3.Lerp(riflePosition, rifleZoomPosition, t);

		float walkSpeed = PlayerController.instance.speed;
		walkSpeed += 0.08f;
		swingPhase += walkSpeed * swingSpeedMul;

		Vector3 swing = new Vector3(
			Mathf.Sin(swingPhase) * 2.0f,
			Mathf.Sin(swingPhase * 2),
			0);
		swing *= swingAmplitude;
		swing *= walkSpeed;

		pos += swing;

		transform.localPosition = pos;
	}


    enum ShootingPhase
    {
        NotShooting,
        ShootingStart,
        Shooting,
		HandToHand,
		HandToHandEnd
    }
}
