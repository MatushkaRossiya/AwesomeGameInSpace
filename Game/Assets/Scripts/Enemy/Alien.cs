using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Alien : CharacterAI
{
    NavMeshAgent navMeshAgent;
    RagdollAnimation ragdollAnimation;
    GameObject player;
    public float maxHitPoints;
    public float attackCooldown;
    public AudioClip[] scream;
    private float currentHitPoints;
    private float nextAttack;
    private bool mainObjectiveDelayed = false;
    private SubObjective subobjective = SubObjective.none;
    private Vector3 subobjectivePosition = Vector3.zero;
    private GameObject blockadeToDestroy = null;

    //public GameObject stuffPrefab;
    private enum SubObjective
    {
        none = 0,
        anotherWay,
        destroyBlockade
    }

    private GameObject[] waypoints;

    public override bool isDead { get; set; }

    public override Vector3 velocity { get { return navMeshAgent.velocity; } }

    private bool disposeBody;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerController>().gameObject;
        ragdollAnimation = GetComponent<RagdollAnimation>();
        currentHitPoints = maxHitPoints;
        waypoints = GameObject.FindGameObjectsWithTag("waypoint");
        disposeBody = false;
    }
    
    void FixedUpdate()
    {
        if (!isDead)
        {
            if (!mainObjectiveDelayed)
            {
                RaycastHit sight;
                bool hit = Physics.Raycast(this.transform.position + new Vector3(0.0f, 0.3f, 0.0f), transform.TransformDirection(Vector3.forward) + new Vector3(0.0f, 0.3f, 0.0f), out sight, 5.0f);
                                
                if (hit && sight.collider.tag == "Blockade")
                {
                    subobjective = (SubObjective)(Random.Range(1, 3));
                    mainObjectiveDelayed = true;

                    switch (subobjective)
                    {
                        case SubObjective.anotherWay:
                            subobjectivePosition = waypoints [Random.Range(0, waypoints.Length)].transform.position;
                            break;
                        case SubObjective.destroyBlockade:
                            subobjectivePosition = sight.collider.transform.position;
                            blockadeToDestroy = sight.collider.gameObject;
                            break;
                    }
                }         
                
                navMeshAgent.SetDestination(player.transform.position);

                if (navMeshAgent.remainingDistance > 5)
                {
                    navMeshAgent.speed = 3;
                }
                else
                {
                    navMeshAgent.speed = 1.5f;
                }

                if (navMeshAgent.remainingDistance < 1.5f)
                {
                    Attack();
                }
                else if (navMeshAgent.velocity.magnitude < 1.0f)
                {
                    Attack();
                }

            }
            else
            {
                if (Vector3.Distance(transform.position, player.transform.position) < 2.0f)
                {
                    mainObjectiveDelayed = false;
                    subobjective = SubObjective.none;
                    subobjectivePosition = Vector3.zero;
                }
                else
                {
                    switch (subobjective)
                    {
                        case SubObjective.anotherWay:
                            navMeshAgent.SetDestination(subobjectivePosition);
                 
                            if (Vector3.Distance(transform.position, subobjectivePosition) < 1.0f)
                            {
                                mainObjectiveDelayed = false;
                                subobjectivePosition = Vector3.zero;
                                subobjective = SubObjective.none;
                            }
                            break;

                        case SubObjective.destroyBlockade:
                            if (!blockadeToDestroy)
                            {
                                mainObjectiveDelayed = false;
                                subobjectivePosition = Vector3.zero;
                                subobjective = SubObjective.none;
                                blockadeToDestroy = null;
                            }
                            else
                            {
                                if (Vector3.Distance(new Vector3(transform.position.x, transform.position.y), new Vector3(subobjectivePosition.x, subobjectivePosition.y)) < 2.0f)
                                {
                                    blockadeToDestroy.GetComponent<Damageable>().DealDamage(transform.forward * 0.5f);
                                }
                                else
                                {
                                    navMeshAgent.SetDestination(subobjectivePosition); 
                                }
                            }
                            break;
                    }
                }
            }
        }
    }

    public void Kill()
    {
        if (!isDead)
        {
            isDead = true;
            navMeshAgent.enabled = false;
			ragdollAnimation.Kill();
            StartCoroutine(DelayDie(10.0f));
            audio.PlayOneShot(scream [Random.Range(0, scream.Length)], 10.0f);
            GameMaster.instance.activeEnemies--;
        }
    }

    void Attack()
    {
        if (Time.fixedTime > nextAttack)
        {
            nextAttack = Time.fixedTime + attackCooldown;
            RaycastHit[] hits = Physics.SphereCastAll(transform.position + new Vector3(0, 1.5f, 0), 1.0f, transform.forward, 1.0f, Layers.enemyAttack);
                        
            if (hits.Length > 0)
            {
                foreach (var hit in hits)
                {
                    Damageable obj = hit.collider.GetComponent<Damageable>();
                    //Debug.Log(obj.tag);
                    if (obj != null)
                    {
                        obj.DealDamage(transform.forward * 10.0f);
                    }
                }
            }
        }
    }

    IEnumerator DelayDie(float delay)
    {
        yield return new WaitForSeconds(delay);
        disposeBody = true;
    }

    public override void DealDamage(float damage)
    {
        currentHitPoints -= damage;
        if (currentHitPoints <= 0.0f)
            Kill();
    }

    void OnBecameInvisible()
    {
        if (disposeBody)
        {
            Destroy(this.gameObject);
        }
    }

}
