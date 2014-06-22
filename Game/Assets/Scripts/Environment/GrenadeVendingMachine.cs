using UnityEngine;
using System.Collections;

public class GrenadeVendingMachine : Interactive
{
	public int grenadePrice;
	public int grenadeAmount;
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
			LaserRifle.instance.grenades += grenadeAmount;
			PlayerStats.instance.syf -= grenadePrice;
			action = true;
			audio.PlayOneShot(getSound);
		}
	}

	void Start() {
		door = transform.FindChild("GrenadeDoor").gameObject;
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
		if (!LaserRifle.instance.hasGrenadeLaucherUpgrade)
		{
			msg = "You don't have grenade launcher";
			return false;
		}
		if (LaserRifle.instance.grenades >= LaserRifle.instance.maxGrenades)
		{
			msg = "Grenades full";
			return false;
		}
		else if (PlayerStats.instance.syf < grenadePrice)
		{
			msg = "Insufficient Syf";
			return false;
		}
		else
		{
			msg = string.Format("Buy {0} grenade for {1} Syf", grenadeAmount, grenadePrice);
			return true;
		}
	}
}
