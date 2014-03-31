using UnityEngine;
using System.Collections;

public class AK47Shooter : MonoBehaviour {
    public AudioClip pewSound;
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            audio.PlayOneShot(pewSound);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.up, out hit)) {
                Debug.Log("HIT");
                WalkingGuyTest guy = hit.collider.transform.root.GetComponent<WalkingGuyTest>();
                if (guy != null) {
                    guy.isRagdoll = true;
                }
            }
        }
    }
}
