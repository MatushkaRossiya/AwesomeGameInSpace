using UnityEngine;
using System.Collections;

public class AmmoVendingMachine : Interactive {
	
    public int ammoPrice;
	public int ammoAmount;
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
			LaserRifle.instance.ammo += ammoAmount;
			PlayerStats.instance.syf -= ammoPrice;
		}
	}

	private bool CanBuy(out string msg) {
        if (LaserRifle.instance.ammo >= LaserRifle.instance.maxAmmo) {
			msg = "Ammo full";
			return false;
		}
		else if (PlayerStats.instance.syf < ammoPrice) {
			msg = "Insufficient syf";
			return false;
		}
		else {
			msg = string.Format("Buy {0} ammo for {1} syf", ammoAmount, ammoPrice);
			return true;
		}
	}
}
