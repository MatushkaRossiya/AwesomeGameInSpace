using UnityEngine;
using System.Collections;

public class NotZombieSpawner : MonoBehaviour {
    public GameObject notAZombie;
    [Range(0.1f, 5.0f)]
    public float spawnInterval = 1.0f;
    public 

    int notZombieCount = 0;
    float nextSpawnTime;
	// Use this for initialization
	void Start () {
        nextSpawnTime = Time.fixedTime + spawnInterval;
	}

    void FixedUpdate()
    {
        if (Time.fixedTime > nextSpawnTime)
        {
            nextSpawnTime += spawnInterval;
            Instantiate(notAZombie, transform.position, Quaternion.identity);
            ++notZombieCount;
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 200, 50), "Not zombie count: " + notZombieCount);
    }
}
