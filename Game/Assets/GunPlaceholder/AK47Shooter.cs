using UnityEngine;
using System.Collections;

public class AK47Shooter : MonoBehaviour {
    public AudioClip pewSound;
    void Update() {

        RaycastHit hitInfo;
        bool hit = Physics.Raycast(transform.position, transform.up, out hitInfo);
        if (hit) {
            Debug.DrawRay(transform.position, transform.up * hitInfo.distance, Color.red);
        } else {
            Debug.DrawRay(transform.position, transform.up * 10000.0f, Color.red);
        }
        if (Input.GetMouseButtonDown(0)) {
            audio.PlayOneShot(pewSound);
            if (hit) {
                Damageable damagable = hitInfo.collider.GetComponent<Damageable>();
                if (damagable != null) {
                    damagable.DealDamage(transform.up * 11.0f);
                }
            } 
        }
    }
}
