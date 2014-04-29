using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WalkingGuyTest : CharacterAI {
    NavMeshAgent navMeshAgent;
    RagdollAnimation ragdollAnimation;
    GameObject player;
    public float maxHitPoints;
    private float currentHitPoints;

    public float attackCooldown;
    public AudioClip wilhelmScream;
    private float nextAttack;

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

            if (navMeshAgent.remainingDistance < 1.5f) {
                Attack();
            } else if(navMeshAgent.velocity.magnitude < 1.0f){
                Attack();
            }
        }
    }

    public void Kill() {
        if (!isDead) {
            isDead = true;
            navMeshAgent.enabled = false;
            ragdollAnimation.isRagdoll = true;
            StartCoroutine(DelayDie(5.0f));
            audio.PlayOneShot(wilhelmScream, 10.0f);
        }
    }

    void Attack() {
        if (Time.fixedTime > nextAttack) {
            nextAttack = Time.fixedTime + attackCooldown;
            RaycastHit[] hits = Physics.SphereCastAll(transform.position + new Vector3(0, 1.5f, 0), 0.5f, transform.forward, 0.5f, Layers.enemyAttack);
            if (hits.Length > 0) {
                foreach (var hit in hits) {
                    Damageable obj = hit.collider.GetComponent<Damageable>();
                    if (obj != null) {
                        obj.DealDamage(transform.forward * 10.0f);
                    }
                }
            }
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
