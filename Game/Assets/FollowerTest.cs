using UnityEngine;
using System.Collections;

public class FollowerTest : MonoBehaviour {
    NavMeshAgent agent;
    GameObject player;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerController>().gameObject;
	}

    void Update()
    {
        agent.SetDestination(player.transform.position);
    }
}
