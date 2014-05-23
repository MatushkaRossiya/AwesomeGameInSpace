using UnityEngine;
using System.Collections;

public class HeavyAlienFSM : BaseFSM
{


    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GetComponent<Alien>().isDead)
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            switch (currentState)
            {
                case State.Chase:
                    UpdateChase();      //Seek
                    break;
                //and
                case State.Attack:
                    UpdateAttack();     //Destroy
                    break;
                default:
                    break;
            }
        }
    }

    protected override void UpdateAttack()
    {
        throw new System.NotImplementedException();
    }

    protected override void UpdateChase()
    {
        if (distanceToPlayer < stateTransitionDistance)
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
