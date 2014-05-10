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

	public override void Action() {
		string str;
		if (CanBuy(out str)) {
			PlayerStats.instance.syringes++;
			PlayerStats.instance.syf -= syringePrice;
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
