using UnityEngine;
using System.Collections;

public class AmmoVendingMachine : Interactive
{
	public int ammoPrice;
	public int ammoAmount;
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
			LaserRifle.instance.ammo += ammoAmount;
			PlayerStats.instance.syf -= ammoPrice;
			action = true;
			audio.PlayOneShot(getSound);
		}
	}

	void Start() {
		door = transform.FindChild("AmmoDoor").gameObject;
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
		if (LaserRifle.instance.ammo >= LaserRifle.instance.maxAmmo)
		{
			msg = "Ammo full";
			return false;
		}
		else if (PlayerStats.instance.syf < ammoPrice)
		{
			msg = "Insufficient Syf";
			return false;
		}
		else
		{
			msg = string.Format("Buy {0} ammo for {1} Syf", ammoAmount, ammoPrice);
			return true;
		}
	}
}
