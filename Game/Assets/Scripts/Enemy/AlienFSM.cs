using UnityEngine;
using System.Collections;

public class AlienFSM : BaseFSM {


    private bool wait = false;
    
	void Start () {
        Initialize();
        viewAngle = 120.0f;
        viewRadius = 20.0f;
	}
	

	void FixedUpdate () {
   
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

            if (!wait)
            {

                if (Mathf.Abs(Quaternion.Angle(transform.rotation, player.transform.rotation) - 180.0f) < 1.0f && Random.Range(0.0f, 1.0f) > 0.95f) controller.Dodge();
                if (distanceToPlayer > 2.0f)
                {
                    agent.SetDestination(player.transform.position);
                    agent.speed = 3.5f;
                    agent.stoppingDistance = 0.75f;
                    StartCoroutine(moment());
                }
                else
                {

                    int atk = Random.Range(1, 9);


                    switch (atk)
                    {
                        case 6:
                        case 1:
                        case 9:
                            controller.AttackFast(1.0f);
                            break;
                        case 7:
                        case 8:                        
                        case 2:
                            controller.AttackStrong(1.0f);
                            break;                     
                       
                        case 4:                            
                            StartCoroutine(atk4());
                            break;
                        case 5:                           
                            StartCoroutine(atk5());
                            break;
                        default:
                            break;
                    }
                }
            }
            }
        

    }

    protected override void UpdateChase()
    {

        agent.speed = 3.0f;
        if (distanceToPlayer < distanceChaseToAttack)
        {
            currentState = State.Attack;
            SubObjectiveClear();
        }
        else
        {

            if (agent.velocity.magnitude < 1.0f)
            {
              
                controller.AttackStrong(1.0f);
            }
            if (mainObjectiveDelayed) UpdateSubObjective();
            else
            {
                Look();
                agent.SetDestination(player.transform.position);
            }


        }
        
    }



    protected override void UpdateSubObjective()
    {

        switch (subObjective)
        {
            case SubObjective.destroyBlockade:
                {
                    agent.stoppingDistance = 0.001f;
                    if (!blockadeToDestroy)
                    {
                        SubObjectiveClear();
                    }
                    else if(Vector3.Distance(transform.position, subobjectivePosition) < closeEnoughToSubobjective)
                    {
                        controller.AttackStrong(1.0f);
                    }
                    else
                    {
                        agent.SetDestination(subobjectivePosition);
                    }
                    break;
                }

            case SubObjective.anotherWay:
                {
                    agent.stoppingDistance = 1.5f;
                 if(Vector3.Distance(transform.position, subobjectivePosition) < closeEnoughToSubobjective) SubObjectiveClear();
                 else agent.SetDestination(subobjectivePosition);
                    break;
                }

            default:
                break;
        }
       
    }

  
    protected override void Look()
    {
        RaycastHit sight;
        bool hit = Physics.Raycast(this.transform.position + new Vector3(0.0f, 0.3f, 0.0f), transform.TransformDirection(Vector3.forward) + new Vector3(0.0f, 0.3f, 0.0f), out sight, 2.0f);

        if (hit && sight.collider.tag == "Blockade")
        {
            subObjective = (SubObjective)(Random.Range(1, 3));
            mainObjectiveDelayed = true;

            switch (subObjective)
            {
                case SubObjective.anotherWay:
                    subobjectivePosition = waypoints[Random.Range(0, waypoints.Length)].transform.position;
                    break;
                case SubObjective.destroyBlockade:
                    subobjectivePosition = sight.collider.transform.position;
                    blockadeToDestroy = sight.collider.gameObject;
                    break;
            }
        }     
    }


   IEnumerator moment()
    {
        wait = true;
       yield return new WaitForSeconds(0.25f);
       wait = false;
    }


    IEnumerator atk4()
   {
       wait = true;
       if (!GetComponent<Alien>().isDead)
       {
           for (int i = 0; i < 25; i++)
           {
               if (!GetComponent<Alien>().isDead) agent.SetDestination(player.transform.position + Vector3.right / 4.0f);
               agent.speed = 5.0f;
               controller.AttackFast(1.0f);
               yield return new WaitForSeconds(0.01f);
           }
       }
       wait = false;
   }

    IEnumerator atk5()
    {
        wait = true;
        if (!GetComponent<Alien>().isDead)
        {
            for (int i = 0; i < 25; i++)
            {
                if (!GetComponent<Alien>().isDead) agent.SetDestination(player.transform.position + Vector3.left / 4.0f);
                agent.speed = 5.0f;
                controller.AttackStrong(1.0f);
                yield return new WaitForSeconds(0.01f);
            }
        }
        wait = false;
    }
}
