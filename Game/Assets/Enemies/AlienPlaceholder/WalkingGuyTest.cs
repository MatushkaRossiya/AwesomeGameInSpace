using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WalkingGuyTest : AICharacter {
    NavMeshAgent navMeshAgent;
    Animator animator;

    public GameObject stuffPrefab;
    GameObject player;
    
    public override bool isDead { get; set; }

    private bool _isAnimatorActive = true;
    public override bool isAnimatorActive {
        get { return _isAnimatorActive; }
        set {
            if (value && !_isAnimatorActive) {
                _isAnimatorActive = true;
                animator.enabled = false;
                navMeshAgent.enabled = false;

            } else if (!value && _isAnimatorActive) {
                _isAnimatorActive = false;
                animator.enabled = true;
            }
        }
    }


    void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerController>().gameObject;
        animator = GetComponent<Animator>();
    }

    void FixedUpdate() {
        if (!isDead) {
            navMeshAgent.SetDestination(player.transform.position);
            if (navMeshAgent.remainingDistance > 5) {
                navMeshAgent.speed = 3;
            } else {
                navMeshAgent.speed = 1.5f;
            }
            float speed = navMeshAgent.velocity.magnitude / 1.513f;
            if (speed < 0.1 || speed > 1.0f)
                animator.speed = 1.0f;
            else
                animator.speed = speed;
            animator.SetFloat("Speed", speed);
        }
    }

    public override void DealDamage(BodyPart bodyPart, float rawDamage) {

        //foreach(var rigidBody)
    }


    public override void Kill() {
        animator.enabled = false;
        navMeshAgent.enabled = false;
    }
}
