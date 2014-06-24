using UnityEngine;
using System.Collections;

public class MineVendingMachine : Interactive
{
	public int minePrice;
	public int mineAmount;
	public AudioClip getSound;
	
	private GameObject door;
	private float phase;
	private bool action;
	
	public override string message
	{
		get
		{
			string ret;
			CanBuy(out ret);
			return ret;
		}
	}
	
	public override void MomentaryAction()
	{
		string str;
		if (CanBuy(out str))
		{
			PlayerStats.instance.mines += mineAmount;
			PlayerStats.instance.syf -= minePrice;
			action = true;
			audio.PlayOneShot(getSound);
		}
	}
	
	void Start() {
		door = transform.FindChild("MineDoor").gameObject;
	}
	
	void Update() {
		if (action) {
			phase += Time.deltaTime;
			if (phase >= 1.0f) {
				action = false;
				phase = 0;
			}
			door.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90 * Mathf.Sin(phase * Mathf.PI)));
		}
	}
	
	private bool CanBuy(out string msg)
	{
		if (PlayerStats.instance.mines >= PlayerStats.instance.maxMines)
		{
			msg = "Mines full";
			return false;
		}
		else if (PlayerStats.instance.syf < minePrice)
		{
			msg = "Insufficient Syf";
			return false;
		}
		else
		{
			msg = string.Format("Buy {0} mine for {1} Syf", mineAmount, minePrice);
			return true;
		}
	}
}
