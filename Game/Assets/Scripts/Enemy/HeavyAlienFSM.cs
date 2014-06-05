using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class HeavyAlienFSM : BaseFSM
{

    NeuralNetwork brain = new NeuralNetwork();

    private float oldRotY = 0.0f;
    private float oldDistance = 0.0f;
    private bool inAction = false;

    void Awake()
    {
        Initialize();
        
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
     
        if (distanceToPlayer > distanceChaseToAttack) currentState = State.Chase;
        else
        {
            List<float> inputs = new List<float>();
            inputs.Add((transform.rotation.y - player.transform.rotation.y)*10.0f);
            inputs.Add(oldRotY);


            inputs.Add(Vector3.Distance(transform.position, player.transform.position));
            inputs.Add(oldDistance);

            inputs.Add(inAction ? 1.0f : 0.0f);

            Debug.Log("inputs");
            foreach (var i in inputs) Debug.Log(i);
            oldRotY = (transform.rotation.y - player.transform.rotation.y)*10.0f;
            oldDistance = Vector3.Distance(transform.position, player.transform.position);
            Debug.Log(Mathf.RoundToInt(brain.NetworkResponse(inputs)[0]));
        }
    }

    float sigmoid(float x)
    {
        return 6.0f / (1 + Mathf.Exp(-x));
    }
    protected override void UpdateChase()
    {
        if (distanceToPlayer < distanceChaseToAttack)
        {
            currentState = State.Attack;
            SubObjectiveClear();
        }
        else
        {

            if (agent.velocity.magnitude < 1.0f)
            {

                controller.AttackStrong(1.5f);
            }
            if (mainObjectiveDelayed) UpdateSubObjective();
            else
            {
                Look();
                agent.SetDestination(player.transform.position);
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
}
