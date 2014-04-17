using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WalkingGuyTest : CharacterAI {
    NavMeshAgent navMeshAgent;
    RagdollAnimation ragdollAnimation;
    GameObject player;
    public float maxHitPoints;
    private float currentHitPoints;

    //public GameObject stuffPrefab;

    public override bool isDead { get; set; }
    public override Vector3 velocity { get { return navMeshAgent.velocity; } }

    void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerController>().gameObject;
        ragdollAnimation = GetComponent<RagdollAnimation>();
        currentHitPoints = maxHitPoints;
    }

    void FixedUpdate() {
        if (!isDead) {
            navMeshAgent.SetDestination(player.transform.position);
            if (navMeshAgent.remainingDistance > 5) {
                navMeshAgent.speed = 3;
            } else {
                navMeshAgent.speed = 1.5f;
            }
        }
    }

    public void Kill() {
        if (!isDead) {
            isDead = true;
            navMeshAgent.enabled = false;
            ragdollAnimation.isRagdoll = true;
            StartCoroutine(DelayDie(5.0f));
        }
    }


    IEnumerator DelayDie(float delay) {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }

    public override void DealDamage(float damage) {
        currentHitPoints -= damage;
        if (currentHitPoints <= 0.0f) Kill();
    }
}
