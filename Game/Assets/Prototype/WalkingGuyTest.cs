using UnityEngine;
using System.Collections.Generic;

public class WalkingGuyTest : MonoBehaviour {
    NavMeshAgent agent;
    GameObject player;
    Animator animator;
    Rigidbody[] skeleton;
    bool _isRagdoll;
    float deathTime;
    float deathDelay = 5.0f;

    public bool isRagdoll{
        get { return _isRagdoll; }
        set {
            if (value && !_isRagdoll) {
                _isRagdoll = true;
                animator.enabled = false;
                foreach (var bone in skeleton) {
                    bone.isKinematic = false;
                }
                agent.enabled = false;
                deathTime = Time.fixedTime;
                
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
        foreach (var bone in skeleton) {
            bone.isKinematic = true;
        }
    }

    void FixedUpdate() {
        if (!_isRagdoll) {
            agent.SetDestination(player.transform.position);
            if (agent.remainingDistance > 5) {
                agent.speed = 3;
            } else {
                agent.speed = 1.5f;
            }
            float speed = agent.velocity.magnitude / 1.513f;
            if (speed < 0.1 || speed > 1.0f)
                animator.speed = 1.0f;
            else
                animator.speed = speed;
            animator.SetFloat("Speed", speed);
        } else {
            if (Time.fixedTime > deathTime + deathDelay) {
                Destroy(this.gameObject);
            } 
        }
    }
}
