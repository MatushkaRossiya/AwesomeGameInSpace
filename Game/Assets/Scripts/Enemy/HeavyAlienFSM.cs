using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class HeavyAlienFSM : BaseFSM
{

    internal NeuralNetwork brain = new NeuralNetwork();

    private float oldRotY = 0.0f;
    private float oldDistance = 0.0f;
    private bool inAction = false;

    internal float timeAttacking = 0.0f;
    

    void Awake()
    {
        Initialize();
        attackMultiplier = 1.5f;   
    }
    void Start()
    {
        brain.IntializeNetwork();
        HeavyAlienBrainCreator.instance.GiveWeights(ref brain);
    }

    void FixedUpdate()
    {
        if (!GetComponent<Alien>().isDead)
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            switch (currentState)
            {
                case State.Patrol:
                    UpdatePatrol();
                    break;
                case State.Chase:
                    UpdateChase();     
                    break;              
                case State.Attack:
                    UpdateAttack();     
                    break;
                default:
                    break;
            }
        }
    }

    protected override void UpdateAttack()
    {
        timeAttacking += Time.fixedDeltaTime;
        transform.LookAt(player.transform);

            if (distanceToPlayer > distanceChaseToAttack) currentState = State.Chase;
            else
            {
                List<float> inputs = new List<float>();
                inputs.Add((transform.rotation.y - player.transform.rotation.y) * 10.0f);
                inputs.Add(oldRotY);
                inputs.Add(Vector3.Distance(transform.position, player.transform.position));
                inputs.Add(oldDistance);

               
                oldRotY = (transform.rotation.y - player.transform.rotation.y) * 10.0f;
                oldDistance = Vector3.Distance(transform.position, player.transform.position);

                if (!wait)
                {
                    List<float> response = brain.NetworkResponse(inputs);
                    int decision = response.IndexOf(response.Max());

                    switch(decision)
                    {
                        case 0:
                          transform.LookAt(player.transform);
                            controller.AttackFast(attackMultiplier);
                            StartCoroutine(moment(0.4f));
                            break;
                        case 1:
                            transform.LookAt(player.transform);
                            controller.AttackStrong(attackMultiplier);
                            StartCoroutine(moment(0.9f));
                            break;
                        case 2:
                            controller.Dodge();
                            StartCoroutine(moment(0.3f));
                            break;
                        default:
                            break;
                    }
                }

       
            }
        
    }

   
 
    protected override void Look()
    {
        RaycastHit sight;
        bool hit = Physics.Raycast(this.transform.position + new Vector3(0.0f, 0.3f, 0.0f), transform.TransformDirection(Vector3.forward) + new Vector3(0.0f, 0.3f, 0.0f), out sight, 2.0f);

        if (hit && sight.collider.tag == "Blockade")
        {
            subObjective = SubObjective.destroyBlockade;
            mainObjectiveDelayed = true;
            subobjectivePosition = sight.collider.transform.position;
            blockadeToDestroy = sight.collider.gameObject;
        }
    }

    protected override void UpdateSubObjective()
    {
        if (!blockadeToDestroy)
        {
            SubObjectiveClear();
        }
        else if (Vector3.Distance(transform.position, subobjectivePosition) < closeEnoughToSubobjective)
        {

            controller.AttackStrong(1.5f);

        }
        else agent.SetDestination(subobjectivePosition);
    }


    public void PlayerDied()
    {
        currentState = BaseFSM.State.Patrol;
        brain.Fitness(timeAttacking, damageDealt, !(GetComponent<Alien>().isDead));
    }
}
