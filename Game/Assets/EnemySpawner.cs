using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	public GameObject enemyPrefab;

	void Start() {
		GameMaster.instance.RegisterSpawner(this);
	}

	public void Activate() {
		StartCoroutine(Spawn());
	}

	private IEnumerator Spawn() {
		yield return new WaitForSeconds(Random.Range(5.0f, 15.0f));
		if (GameMaster.instance.spawnPool > 0) {
			GameMaster.instance.spawnPool--;
			GameMaster.instance.activeEnemies++;
			Instantiate(enemyPrefab, transform.position, transform.localRotation);
			StartCoroutine(Spawn());
		}
	}
}
