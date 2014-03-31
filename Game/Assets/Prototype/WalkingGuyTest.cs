using UnityEngine;
using System.Collections;

public class WalkingGuyTest : MonoBehaviour {
    NavMeshAgent agent;
    GameObject player;
    Animator animator;
    Rigidbody[] skeleton;
    bool _isRagdoll;

    public bool isRagdoll{
        get { return _isRagdoll; }
        set {
            if (value && !_isRagdoll) {
                _isRagdoll = true;
                animator.enabled = false;
                foreach (var bone in skeleton) {
                    bone.velocity = Vector3.zero;
                }
            } else if (!value && _isRagdoll) {
                _isRagdoll = false;
                animator.enabled = true;
                foreach (var bone in skeleton) {
                    bone.isKinematic = true;
                }
            }
        }
    }

    // Use this for initialization
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerController>().gameObject;
        animator = GetComponent<Animator>();
        skeleton = GetComponentsInChildren<Rigidbody>();
    }

    void FixedUpdate() {
        if (!_isRagdoll) {
            agent.SetDestination(player.transform.position);
            float speed = agent.velocity.magnitude / agent.speed;
            if (speed < 0.1)
                animator.speed = 1.0f;
            else
                animator.speed = speed;
            animator.SetFloat("Speed", speed);
        }
    }
}
