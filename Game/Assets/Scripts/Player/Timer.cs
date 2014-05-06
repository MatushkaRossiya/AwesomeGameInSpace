using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	public float beginTime; //time in seconds
	float time;
	// Use this for initialization
	void Start () {
		time = beginTime;
	}
	public float getTime(){
		return time;
	}
	public override string ToString(){
		int minuty = Mathf.FloorToInt(time / 60.0f);
		int sekundy = (int)((time/60.0f - minuty)*60);
		if(sekundy < 10) return minuty.ToString() + ":0" + sekundy.ToString();
		return minuty.ToString() + ":" + sekundy.ToString();
	}
	public void setTime(float inpTime){
		time = inpTime;
	}

	// Update is called once per frame
	void Update () {
		if(time > Time.deltaTime)
		time -= Time.deltaTime;
	}
}
