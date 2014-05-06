using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	public float meanTimeBetweenSpawns;
	public GameObject enemyPrefab;

	void Start() {
		GameMaster.instance.RegisterSpawner(this);
	}

	public void Activate() {
		StartCoroutine(Spawn());
	}

	private IEnumerator Spawn() {
		while(true){
			yield return new WaitForSeconds(Random.Range(0.5f * meanTimeBetweenSpawns, 1.5f * meanTimeBetweenSpawns));
			if (GameMaster.instance.spawnPool > 0) {
				GameMaster.instance.spawnPool--;
				GameMaster.instance.activeEnemies++;
				Instantiate(enemyPrefab, transform.position, transform.localRotation);
			}
			else {
				break;
			}
		}
	}
}
