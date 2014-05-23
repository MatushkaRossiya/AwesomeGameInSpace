using UnityEngine;
using System.Collections;

public abstract class BaseFSM : MonoBehaviour {


    protected NavMeshAgent agent;
    protected GameObject player;
    protected AlienController controller;
    protected State currentState;
    protected float stateTransitionDistance = 5.0f;
    protected float distanceToPlayer;

    protected bool mainObjectiveDelayed;
    protected SubObjective subObjective;
    protected float closeEnoughToSubobjective = 1.7f;
    
    protected Vector3 subobjectivePosition = Vector3.zero;
    protected GameObject blockadeToDestroy = null;

    protected abstract void UpdateAttack();
    protected abstract void UpdateChase();
    protected abstract void UpdateSubObjective();
    protected void Initialize()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerController>().gameObject;
        controller = GetComponent<AlienController>();
        currentState = State.Chase;
    }

    protected void SubObjectiveClear()
    {
        mainObjectiveDelayed = false;
        subobjectivePosition = Vector3.zero;
        subObjective = SubObjective.none;
        blockadeToDestroy = null;
    }

    protected abstract void Look();


    protected enum State
    {
        Chase,
        Attack
    }

    protected enum SubObjective
    {
        none = 0,
        anotherWay,
        destroyBlockade
    }

}
