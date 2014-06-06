using UnityEngine;
using System.Collections;

public class MedicalVendingMachine : Interactive {
	public int syringePrice;
	public override string message {
		get {
			string ret;
			CanBuy(out ret);
			return ret;
		}
	}

	public override void MomentaryAction() {
		string str;
		if (CanBuy(out str)) {
			PlayerStats.instance.syringes++;
			PlayerStats.instance.syf -= syringePrice;
			action = true;
		}
	}

	private GameObject door;
	private float phase;
	private bool action;

	void Start() {
		door = transform.FindChild("FirstAidKitDoor").gameObject;
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

	private bool CanBuy(out string msg) {
		PlayerStats p = PlayerStats.instance;
		if (p.syringes >= p.maxSyringes) {
			msg = "Syringes full";
			return false;
		}
		else if (p.syf < syringePrice) {
			msg = "Insufficient syf";
			return false;
		}
		else {
			msg = "Buy syringe for " + syringePrice + " syf";
			return true;
		}
	}
}
