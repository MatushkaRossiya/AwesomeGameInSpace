﻿using UnityEngine;
using System.Collections;

public class AK47Shooter : MonoSingleton<AK47Shooter>
{
    public GameObject bulletSource;
    public GameObject granadeLauncher;
    public GameObject granadePrefab;
    public ParticleSystem hitParticleEffect;
    public GameObject bulletTrailPrefab;
    public float granadeSpeed;
    public AudioClip pewSound;
    public AudioClip outOfAmmoSound;
    public AudioClip granadeLauncherSound;
    [Range(0.1f, 0.5f)]
    public float
        shotPeriod;
    public int maxAmmo;
    public float recoil;
    public float handling;
    ShootingPhase shootingPhase;
    private float nextShot;
    private int _ammo;

    public int ammo
    {
        get
        {
            return _ammo;
        }
        set
        {
            _ammo = Mathf.Clamp(value, 0, maxAmmo);
        }
    }

    private float spread;
    private float knockBack;
    private float granadeCooldown = 1.0f;
    private float nextGranade = 0.0f;

    void Start()
    {
        shootingPhase = ShootingPhase.NotShooting;
        nextShot = Time.fixedTime;
        ammo = maxAmmo;
    }

    void Update()
    {
        if (nextGranade > 0.0f)
            nextGranade -= Time.deltaTime;

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
        
        if (Input.GetKeyDown(KeyCode.R) || Gamepad.instance.justPressedB())
        {
            ammo = maxAmmo;
        }

        if (nextGranade <= 0.0f && (Input.GetMouseButtonDown(2) || Gamepad.instance.justPressedRightShoulder()))
        {
            audio.PlayOneShot(granadeLauncherSound);
            GameObject granade = Instantiate(granadePrefab, granadeLauncher.transform.position, granadeLauncher.transform.localRotation) as GameObject;
            granade.rigidbody.velocity = transform.root.rigidbody.velocity + granadeLauncher.transform.forward * granadeSpeed;
            nextGranade = granadeCooldown;
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

                LineRenderer bulletTrail = (Instantiate(bulletTrailPrefab) as GameObject).GetComponent<LineRenderer>();
                float trailWidth = 0.02f;
                bulletTrail.SetWidth(trailWidth, trailWidth);
                bulletTrail.SetPosition(0, start);
                Vector3 endPosition;

                if (hit)
                {
                    Debug.DrawLine(start, hitInfo.point, Color.yellow, 0.2f, true);
                    Damageable damagable = hitInfo.collider.GetComponent<Damageable>();

                    if (damagable != null)
                    {
                        damagable.DealDamage(dir * 11.0f);
                    }

                    MaterialHolder mat = hitInfo.collider.GetComponent<MaterialHolder>();

                    if (mat != null)
                    {
                        GameObject hole = Instantiate(mat.materialProperties.bulletHolePrefab, hitInfo.point + 0.01f * hitInfo.normal, Quaternion.LookRotation(hitInfo.normal)) as GameObject;
                        hole.transform.parent = hitInfo.transform;
                        BulletHoleManager.instance.AddBulletHole(hole);
                    }

                    hitParticleEffect.transform.position = hitInfo.point;
                    hitParticleEffect.transform.forward = (Vector3.Reflect(dir, hitInfo.normal) + hitInfo.normal) / 2;
                    ;
                    hitParticleEffect.Emit(10);
                    endPosition = hitInfo.point;
                }
                else
                {
                    Debug.DrawRay(start, dir * 1000.0f, Color.yellow, 0.2f, true);
                    endPosition = start + dir * 1000.0f;
                }
                bulletTrail.SetPosition(1, endPosition);
                bulletTrail.material.mainTextureScale = new Vector2((start - endPosition).magnitude / trailWidth * 0.25f, 1);

                spread += recoil;
                knockBack += recoil;
                FirstPersonCameraController.instance.verticalAngle += knockBack * 10.0f;
            }
        }

        transform.localRotation = Quaternion.Euler(new Vector3(-100.0f * knockBack, 0, 0));
    }

    void OnGUI()
    {
        int sw = Screen.width;
        int sh = Screen.height;
        GUI.Label(new Rect(sw * 0.8f, sh * 0.8f, sw * 0.2f, sh * 0.2f), "AMMO: " + ammo);
    }

    enum ShootingPhase
    {
        NotShooting,
        ShootingStart,
        Shooting
    }
}
