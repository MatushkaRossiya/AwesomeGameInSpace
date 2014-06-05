using UnityEngine;
using System.Collections;

public abstract class BaseFSM : MonoBehaviour {


    protected NavMeshAgent agent;
    protected GameObject player;
    protected AlienController controller;
    protected internal State currentState;
    protected float distanceChaseToAttack = 3.5f;
    protected float distancePatroltoChase = 10.0f;
    protected float distanceAlarm = 8.0f;
    protected float distanceToPlayer;
    protected GameObject[] waypoints;
    protected bool mainObjectiveDelayed;
    protected SubObjective subObjective;
    protected float closeEnoughToSubobjective = 1.7f;
    
    protected Vector3 subobjectivePosition = Vector3.zero;
    protected GameObject blockadeToDestroy = null;

    protected PatrolObjective patrolObjective = PatrolObjective.nothing;
    protected Vector3 patrolObjectivePosition = Vector3.zero;
    protected float patrolSpeed = 1.0f;

    protected float patrolTaskTime = 0.0f;

    protected float viewAngle = 130.0f;
    protected float viewRadius = 25.0f;

    protected float chasingTime = 10.0f;
    protected bool chaseTimeout = false;
    protected float stopChase = 0.0f;

    protected float alienMultiplier = 1.0f;

    protected bool wait = false;


    internal float damageDealt = 0.0f;


    protected abstract void UpdateAttack();
    protected virtual void  UpdateChase()
    {
        agent.speed = 3.0f * alienMultiplier;
          agent.stoppingDistance = 3.0f;
        if (distanceToPlayer < distanceChaseToAttack)
        {
           
            currentState = State.Attack;
            SubObjectiveClear();
        }
        else
        {
            if(distanceToPlayer > distancePatroltoChase && !IsPlayerInMyFOV() && !chaseTimeout)
            {
                chaseTimeout = true;
                stopChase = Time.time + chasingTime;
            }
            else if(chaseTimeout && IsPlayerInMyFOV())
            {
                chaseTimeout = false;
            }
            else if(chaseTimeout && !IsPlayerInMyFOV() && distanceToPlayer > distancePatroltoChase)
            {
                if (Time.time > stopChase) currentState = State.Patrol;
            }

            if (agent.velocity.magnitude < 1.0f)
            {
              
                controller.AttackStrong(alienMultiplier);
            }
            if (mainObjectiveDelayed) UpdateSubObjective();
            else
            {
                Look();
                agent.SetDestination(player.transform.position);
            }


        }
    }
    protected abstract void UpdateSubObjective();
    protected void UpdatePatrol()
    {
        if (IsPlayerTooClose())
        {
            currentState = State.Chase;
            Alarm();
        }

        else if (IsPlayerInMyFOV())
        {
            currentState = State.Chase;
            Alarm();
        }
        else
        {
            if (mainObjectiveDelayed) UpdateSubObjective();
            else
            {
                Look();

                switch (patrolObjective)
                {
                    case PatrolObjective.nothing:
                        int next = Random.Range(0, 3);
                        if (next % 2 == 1)
                        {
                            patrolTaskTime = Time.time + Random.Range(1.0f, 10.0f);
                            patrolObjective = PatrolObjective.hangOut;
                        }
                        else if (next % 2 == 0)
                        {
                            patrolObjectivePosition = waypoints[Random.Range(0, waypoints.Length)].transform.position;

                            if (next == 0)
                            {
                                Vector3 pos = new Vector3(transform.position.x + Random.Range(-10.0f, 10.0f), 2.0f, transform.position.z + Random.Range(-10.0f, 10.0f));
                                RaycastHit hit;
                                if (Physics.Raycast(pos, Vector3.down, out hit))
                                {
                                    if (hit.collider.gameObject.name == "Ground") patrolObjectivePosition = hit.transform.position;
                                }
                            }
                            patrolObjective = PatrolObjective.takeAWalk;
                            agent.speed = Random.Range(0.9f, 1.2f);
                            agent.stoppingDistance = 0.5f;
                            patrolTaskTime = Time.time + Random.Range(8.0f, 40.0f);
                        }
                        break;
                    case PatrolObjective.hangOut:
                        if (Time.time > patrolTaskTime) patrolObjective = PatrolObjective.nothing;
                        break;
                    case PatrolObjective.takeAWalk:

                        if ((Vector3.Distance(transform.position, patrolObjectivePosition) < closeEnoughToSubobjective) || (Time.time > patrolTaskTime)) patrolObjective = PatrolObjective.nothing;
                        else
                        {
                            agent.SetDestination(patrolObjectivePosition);
                        }
                        break;

                }
            }
        }
    }

    protected void Initialize()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerController>().gameObject;
        controller = GetComponent<AlienController>();
        currentState = State.Patrol;
        waypoints = GameObject.FindGameObjectsWithTag("waypoint");
        
        
    }

    protected void SubObjectiveClear()
    {
        mainObjectiveDelayed = false;
        subobjectivePosition = Vector3.zero;
        subObjective = SubObjective.none;
        blockadeToDestroy = null;
    }

    protected abstract void Look();
    protected bool IsPlayerInMyFOV()
    {
        Vector3 playerPos = new Vector3(player.transform.position.x, 1.0f, player.transform.position.z);
        Vector3 alienPos = new Vector3(transform.position.x, 1.0f, transform.position.z);

        RaycastHit rayHit;
        bool hit = Physics.Raycast(alienPos, (playerPos - alienPos), out rayHit, viewRadius);
        if (hit && rayHit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (Vector3.Angle((playerPos - transform.position), transform.forward) <= (viewAngle / 2.0f))
            {
              //  Debug.Log("I can spy with my little eye...");
                return true;
            }
        }
        return false;
    }

    protected bool IsPlayerTooClose()
    {
        Vector3 playerPos = new Vector3(player.transform.position.x, 0.0f, player.transform.position.z);
        Vector3 alienPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
        RaycastHit rayHit;
        bool hit = Physics.Raycast(alienPos, (playerPos - alienPos), out rayHit, distancePatroltoChase);
        if (hit && rayHit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
           // Debug.Log("sensing sth");
            return true;
        }
        else return false;
    }

    protected internal enum State
    {
        Patrol,
        Chase,
        Attack
    }

    protected enum SubObjective
    {
        none,
        anotherWay,
        destroyBlockade
    }

    protected enum PatrolObjective
    {
        nothing,
        hangOut,
        takeAWalk
    }

    protected IEnumerator moment(float t)
    {
        wait = true;
        yield return new WaitForSeconds(t);
        wait = false;
    }


    protected void Alarm()
    {
        BaseFSM[] aliensAround = FindObjectsOfType<BaseFSM>();
        foreach(BaseFSM alien in aliensAround)
        {
            if(Vector3.Distance(transform.position, alien.transform.position) < distanceAlarm)
            {
                if (alien.currentState == State.Patrol)
                {
                    alien.currentState = State.Chase;
                }
            }
        }
    }
}
