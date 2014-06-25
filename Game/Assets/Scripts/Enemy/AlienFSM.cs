using UnityEngine;
using System.Collections;

public class AlienFSM : BaseFSM
{
    void Start()
    {
        Initialize();
    }

    void FixedUpdate()
    {
        if (!alienComponent.isDead)
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer < 20.0f && IsInPlayerFOV())
            {
                MusicMaster.instance.startFightMusic();
                if (Tutorial.isEnabled())
                {
                    Tutorial.instance.showAlienTutorial();
                }
            }
          
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

    protected override void UpdateChase()
    {
        if (Mathf.Abs(Quaternion.Angle(transform.rotation, player.transform.rotation) - 180.0f) < 1.0f && Random.Range(0.0f, 1.0f) > 0.9995f)
        {
            agent.Stop();
            controller.Dodge();
            StartCoroutine(moment(0.3f));
        }
        if (!wait)
            base.UpdateChase();
    }

    protected override void UpdateAttack()
    {
        if (!wait)
        {
            if (distanceToPlayer > distanceChaseToAttack)
                currentState = State.Chase;
            else
            {

                if (Mathf.Abs(Quaternion.Angle(transform.rotation, player.transform.rotation) - 180.0f) < 1.0f && Random.Range(0.0f, 1.0f) > 0.95f)
                {
                    controller.Dodge();
                    StartCoroutine(moment(0.3f));
                }

                    if(LaserRifle.instance.handsAttack.isPlaying && Random.Range(0.0f, 1.0f) > 0.95f)
                    {
                        controller.DodgeBack();
                        StartCoroutine(moment(0.2f));
                    }
                //if (distanceToPlayer > 2.0f)
                //{
                //    agent.SetDestination(player.transform.position);
                //    agent.speed = 3.5f;
                //    agent.stoppingDistance = 1.75f;
                //    StartCoroutine(moment(0.3f));
                //}
                else
                {
                    int atk = Random.Range(1, 12);

                    switch (atk)
                    {
                        case 1:
                        case 2:
                        case 3:
                            transform.LookAt(player.transform);
                            controller.AttackFast(alienMultiplier);
                            StartCoroutine(moment(0.4f));
                            break;
                        case 4:
                        case 5:
                        case 6:
                            transform.LookAt(player.transform);
                            controller.AttackStrong(alienMultiplier);
                            StartCoroutine(moment(0.9f));
                            break;
                        case 7:
                            if (alienComponent.currentHitPoints < alienComponent.maxHitPoints * 0.1f)
                                StartCoroutine(retreat());
                            break;
                        case 8:
                        case 9:
                            StartCoroutine(atkAction1());
                            break;
                        case 10:
                        case 11:
                            StartCoroutine(atkAction2());
                            break;
                        default:
                            break;
                    }
                }
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
                    else if (Vector3.Distance(transform.position, subobjectivePosition) < closeEnoughToSubobjective)
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
                    if (Vector3.Distance(transform.position, subobjectivePosition) < closeEnoughToSubobjective)
                        SubObjectiveClear();
                    else
                        agent.SetDestination(subobjectivePosition);
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
                    subobjectivePosition = waypoints [Random.Range(0, waypoints.Length)].transform.position;
                    break;
                case SubObjective.destroyBlockade:
                    subobjectivePosition = sight.collider.transform.position;
                    blockadeToDestroy = sight.collider.gameObject;
                    break;
            }
        }     
    }

    IEnumerator retreat()
    {
        wait = true;
        Vector3 runaway = waypoints [Random.Range(0, waypoints.Length)].transform.position;
        while (Vector3.Distance(transform.position, runaway) > closeEnoughToSubobjective)
        {
            if (!alienComponent.isDead)
                agent.SetDestination(runaway);
            agent.speed = 4.0f;
            yield return new WaitForSeconds(1.0f);
        }
        if (Vector3.Distance(player.transform.position, transform.position) > distancePatroltoChase && !IsPlayerInMyFOV())
            currentState = State.Patrol;
        wait = false;
        yield return null;
    }

    IEnumerator atkAction1()
    {
        wait = true;
        controller.Dodge();
        yield return new WaitForSeconds(0.3f);
        transform.LookAt(player.transform);
        controller.AttackFast(alienMultiplier);
        yield return new WaitForSeconds(0.4f);
        wait = false;
        yield return null;

    }

    IEnumerator atkAction2()
    {
        wait = true;
        controller.Dodge();
        yield return new WaitForSeconds(0.3f);
        transform.LookAt(player.transform);
        controller.AttackStrong(alienMultiplier);
        yield return new WaitForSeconds(0.8f);
        wait = false;
        yield return null;
    }
}
