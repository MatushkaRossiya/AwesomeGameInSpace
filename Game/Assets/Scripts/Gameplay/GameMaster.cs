using UnityEngine;
using System.Collections.Generic;

public class GameMaster : MonoSingleton<GameMaster>
{
    public int spawnPool { get; set; }
    public int activeEnemies { get; set; }

    public float dayLenght;
    public float changeDuration;
    public int waveSize;
    public int spawnRate;

    private delegate void Phase();

    private Phase phase;
    private float nightBeginTime;
    private float dayPhase = 0;
    private List<EnemySpawner> spawners = new List<EnemySpawner>();

    GameMaster()
    {
        phase = Day;
    }

    void Update()
    {
        phase();
    }

    void Start()
    {
        nightBeginTime = Time.time + dayLenght;
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
            dayPhase += Time.deltaTime;
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
            dayPhase += Time.deltaTime;
            LightManager.instance.dayPhase = dayPhase / changeDuration;
        } 
        else
        {
            phase = Night;
            spawnPool = waveSize;
            float meanTimeBetweenSpawns = (spawnRate / spawners.Count) * spawnRate;
            foreach (EnemySpawner spawner in spawners)
            {
                spawner.meanTimeBetweenSpawns = meanTimeBetweenSpawns;
                spawner.Activate();
            }
        }
    }

    void Night()
    {
        if (spawnPool <= 0 && activeEnemies <= 0)
        {
            phase = Dawn;
            dayPhase = 0;
        }
    }

    void Dawn()
    {
        if (dayPhase < changeDuration)
        {
            dayPhase += Time.deltaTime;
            LightManager.instance.dayPhase = (1 - dayPhase / changeDuration);
        } 
        else
        {
            //totalTime = dayLenght + changeDuration;
            phase = Day;
            dayPhase = 0;
            LightManager.instance.dayPhase = 0;
        }
    }

    public void RegisterSpawner(EnemySpawner spawner)
    {
        spawners.Add(spawner);
    }
}
