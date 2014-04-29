using UnityEngine;
using System.Collections.Generic;

public class GameMaster : MonoSingleton<GameMaster> {
	public int spawnPool { get; set; }
	public int activeEnemies { get; set; }
	public float dayLenght;
	public float changeDuration;
	public int waveSize;

	private delegate void Phase();
	private Phase phase;

	private float dayPhase = 0;
	private List<EnemySpawner> spawners = new List<EnemySpawner>();

	GameMaster(){
		phase = Day;
	}

	void Update () {
		phase();
	}

	void Day() {
		if(dayPhase < dayLenght){
			dayPhase += Time.deltaTime;
		} else {
			phase = Evening;
			dayPhase = 0;
		}
	}

	void Evening() {
		if (dayPhase < changeDuration) {
			dayPhase += Time.deltaTime;
			LightManager.instance.dayPhase = dayPhase / changeDuration;
		} else {
			phase = Night;
			spawnPool = waveSize;
			foreach (EnemySpawner spawner in spawners) {
				spawner.Activate();
			}
		}
	}

	void Night() {
		if (spawnPool <= 0 && activeEnemies <= 0) {
			phase = Dawn;
			dayPhase = 0;
		}
	}

	void Dawn(){
		if (dayPhase < changeDuration) {
			dayPhase += Time.deltaTime;
			LightManager.instance.dayPhase = (1 - dayPhase / changeDuration);
		} else {
			phase = Day;
		}
	}

	public void RegisterSpawner(EnemySpawner spawner) {
		spawners.Add(spawner);
	}
}
