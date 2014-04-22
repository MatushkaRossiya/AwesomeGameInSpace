using UnityEngine;
using System.Collections;

public class AK47Shooter : MonoBehaviour {
    public AudioClip pewSound;
    public AudioClip outOfAmmoSound;

    [Range(0.1f, 0.5f)]
    public float shotPeriod;
    public int clipSize;


    enum ShootingPhase {
        NotShooting,
        ShootingStart,
        Shooting
    }

    ShootingPhase shootingPhase;

    private float nextShot;
    private int currentClip;

    void Start() {
        shootingPhase = ShootingPhase.NotShooting;
        nextShot = Time.fixedTime;
        currentClip = clipSize;
    }

    void Update() {
        if (Input.GetMouseButton(0)) {
            if (shootingPhase == ShootingPhase.NotShooting)
                shootingPhase = ShootingPhase.ShootingStart;
        } else {
            if (shootingPhase == ShootingPhase.Shooting)
                shootingPhase = ShootingPhase.NotShooting;
        }
        
        if (Input.GetKeyDown(KeyCode.R)) {
            currentClip = clipSize;
        }
    }

    void FixedUpdate() {
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(transform.position, transform.forward, out hitInfo, 10000.0f, ~(1 << 8));
        if (Time.fixedTime >= nextShot && shootingPhase != ShootingPhase.NotShooting) {
            shootingPhase = ShootingPhase.Shooting;
            nextShot = Time.fixedTime + shotPeriod;
            if (currentClip <= 0) {
                audio.PlayOneShot(outOfAmmoSound);
            } else {
                --currentClip;
                audio.PlayOneShot(pewSound);
                if (hit) {
                    Damageable damagable = hitInfo.collider.GetComponent<Damageable>();
                    if (damagable != null) {
                        damagable.DealDamage(transform.forward * 11.0f);
                    }
                }
            }
        }
    }

    void OnGUI() {
        int sw = Screen.width;
        int sh = Screen.height;
        GUI.Label(new Rect(sw * 0.8f, sh * 0.8f, sw * 0.2f, sh * 0.2f), "AMMO: " + currentClip);
    }
}
