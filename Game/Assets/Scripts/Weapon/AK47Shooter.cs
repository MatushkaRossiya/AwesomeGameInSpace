using UnityEngine;
using System.Collections;

public class AK47Shooter : MonoBehaviour {
    public AudioClip pewSound;
    public AudioClip outOfAmmoSound;

    [Range(0.1f, 0.5f)]
    public float shotPeriod;
    public int clipSize;
    

    private float nextShot;
    private int currentClip;

    void Start() {
        nextShot = Time.fixedTime;
        currentClip = clipSize;
    }

    void FixedUpdate() {
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(transform.position, transform.forward, out hitInfo, 10000.0f, ~(1 << 8));
        if (Time.fixedTime >= nextShot && Input.GetMouseButton(0)) {
            nextShot = Time.fixedTime + shotPeriod;
            if (currentClip <= 0) {
                audio.PlayOneShot(outOfAmmoSound);
            } else {
                --currentClip;
                audio.PlayOneShot(pewSound);
                if (hit) {
                    Damageable damagable = hitInfo.collider.GetComponent<Damageable>();
                    if (damagable != null) {
                        damagable.DealDamage(transform.up * 11.0f);
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            currentClip = clipSize;
        }
    }

    void OnGUI() {
        int sw = Screen.width;
        int sh = Screen.height;
        GUI.Label(new Rect(sw * 0.8f, sh * 0.8f, sw * 0.2f, sh * 0.2f), "AMMO: " + currentClip);
    }
}
