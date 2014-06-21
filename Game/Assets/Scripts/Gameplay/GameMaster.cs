using UnityEngine;
using System.Collections.Generic;

public class GameMaster : MonoSingleton<GameMaster>
{
    public int spawnPool { get; set; }
    public int activeEnemies { get; set; }

    public float dayLenght;
    public float changeDuration;
    public int waveSize;
    public float spawnRate;

    private delegate void Phase();

    private Phase phase;
    private float nightBeginTime;
    private float dayPhase = 0;
    private List<EnemySpawner> spawners = new List<EnemySpawner>();
	public int currentRound;

    GameMaster()
    {
        phase = Day;
    }

    void FixedUpdate()
    {
        phase();
    }

    void Start()
    {
        nightBeginTime = Time.time + dayLenght;
        MusicMaster.instance.startDayMusic();
		LightManager.instance.TurnLightsNight(false);
		LightManager.instance.TurnLightsDay(true);
    }

    public string TimeToDayEnd
    {
        get
        {
            float timeLeft = Mathf.Max(0, nightBeginTime - Time.time);
            int minuty = Mathf.FloorToInt(timeLeft / 60.0f);
            int sekundy = (int)((timeLeft / 60.0f - minuty) * 60);
            if (sekundy < 10)
                return minuty.ToString() + ":0" + sekundy.ToString();
            return minuty.ToString() + ":" + sekundy.ToString(); 
        }
    }

    void Day()
    {
        if (dayPhase < dayLenght)
        {
            dayPhase += Time.fixedDeltaTime;
        }
        else
        {
            phase = Evening;
            dayPhase = 0;
        }
    }

    void Evening()
    {
        if (dayPhase < changeDuration)
        {
            dayPhase += Time.fixedDeltaTime;
            LightManager.instance.dayPhase = dayPhase / changeDuration;
            GravityManager.instance.gravity = dayPhase / changeDuration;
        }
        else
        {
            phase = Night;
            spawnPool = waveSize;
            float meanTimeBetweenSpawns = spawners.Count / spawnRate;
            foreach (EnemySpawner spawner in spawners)
            {
                spawner.meanTimeBetweenSpawns = meanTimeBetweenSpawns;
                spawner.Activate();
            }
            currentRound++;
            HUD.instance.showRoundNumber(currentRound);
            MusicMaster.instance.startExplorationMusic();
			LightManager.instance.TurnLightsDay(false);
			LightManager.instance.TurnLightsNight(true);
        }
    }

    void Night()
    {
        if (spawnPool <= 0 && activeEnemies <= 0)
        {
            phase = Dawn;
            dayPhase = 0;
            waveSize += 5;
            if (waveSize > 100) waveSize = 100;
            spawnRate += 0.01f;
            if (spawnRate > 2.0f) spawnRate = 2.0f;
        }
    }

    void Dawn()
    {
        if (dayPhase < changeDuration)
        {
            dayPhase += Time.fixedDeltaTime;
			LightManager.instance.dayPhase = (1 - dayPhase / changeDuration);
			GravityManager.instance.gravity = (1 - dayPhase / changeDuration);
        }
        else
        {
            nightBeginTime = Time.time + dayLenght;
            //totalTime = dayLenght + changeDuration;
            phase = Day;
            dayPhase = 0;
			Loader.instance.save();
            LightManager.instance.dayPhase = 0;
            MusicMaster.instance.startDayMusic();
			LightManager.instance.TurnLightsNight(false);
			LightManager.instance.TurnLightsDay(true);
        }
    }

    public void RegisterSpawner(EnemySpawner spawner)
    {
        spawners.Add(spawner);
    }
}
