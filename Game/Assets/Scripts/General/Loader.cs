using UnityEngine;
using System.Collections;

public class Loader
{
	static Loader me;//singleton
	public static Loader instance
	{
		get
		{
			if (me == null)	me = new Loader ();
			return me;
		}
	}
	//stale nazwy rekordów
	//player
	private const string anySaves = "AnySaves";
	private const string healthStr = "Health";
	private const string syfStr = "Syf";
	private const string ammoStr = "Ammo";
	//poziom trudnosci
	private const string roundStr = "Round";	//ile cykli juz minelo
	private const string waveSizeStr = "WaveSize";	
	private const string spawnRateStr = "SpawnRate";
	//autosave
	public const string autoSaveStr = "autoSave";
	private const string levelStr = "Level";
	public string saveToLoad;
	//ustawiane zanim zmienimy scene zeby bylo wiadomo ktory save wczytac
	
	public void save(string saveName=autoSaveStr)
	{
		Debug.Log("Saving:"+saveName);
		PlayerPrefs.SetInt (anySaves, 1);
		PlayerPrefs.SetFloat(healthStr+saveName, PlayerStats.instance.health);
		PlayerPrefs.SetInt(syfStr+saveName, PlayerStats.instance.syf);
		PlayerPrefs.SetInt (ammoStr + saveName, LaserRifle.instance.ammo);
		PlayerPrefs.SetInt (roundStr+saveName, GameMaster.instance.currentRound);
		PlayerPrefs.SetInt (waveSizeStr+saveName, GameMaster.instance.waveSize);
		PlayerPrefs.SetFloat(spawnRateStr+saveName, GameMaster.instance.spawnRate);
		PlayerPrefs.SetInt (levelStr + saveName, Application.loadedLevel);
		PlayerPrefs.Save ();
		HUD.instance.setHintvisible ("Game Saved", 2);
	}

	public void load(string saveName=autoSaveStr)	//domyslnie autoLoad
	{
		saveToLoad = saveName;
		if(saveName == "NewGame")
			Application.LoadLevel (1);	//TO DO CHECK NA KONCU CZY 1 TO TEN LEVEL O KTORY NAM CHODZI
		else Application.LoadLevel (PlayerPrefs.GetInt (levelStr + saveName));
	}
	public void continueLoading()
	{
		if (saveToLoad == "NewGame") 
		{
			HUD.instance.setHintvisible ("Welcome!", 2);
			return;
		}
		string saveName = saveToLoad;
		Debug.Log ("Loaded save :" + saveName);
		Debug.Log ("Health = " + PlayerPrefs.GetFloat (healthStr + saveName));
		Debug.Log ("Syf = " + PlayerPrefs.GetInt (syfStr + saveName));
		Debug.Log ("Round = " +PlayerPrefs.GetInt (roundStr + saveName));
		Debug.Log ("Ammo = " + PlayerPrefs.GetInt (ammoStr + saveName));
		PlayerStats.instance.health = PlayerPrefs.GetFloat (healthStr + saveName);
		PlayerStats.instance.syf = PlayerPrefs.GetInt (syfStr + saveName);
		LaserRifle.instance.ammo = PlayerPrefs.GetInt (ammoStr + saveName);
		GameMaster.instance.currentRound = PlayerPrefs.GetInt (roundStr + saveName);
		GameMaster.instance.waveSize = PlayerPrefs.GetInt (waveSizeStr + saveName);
		GameMaster.instance.spawnRate = PlayerPrefs.GetFloat (spawnRateStr + saveName);
		saveToLoad = null;
		HUD.instance.setHintvisible ("Game Loaded", 2);
	}
	public bool areThereAnySaves()
	{
		return PlayerPrefs.HasKey (anySaves);
	}
}

