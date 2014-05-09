using UnityEngine;
using System.Collections;

public class AmmoVendingMachine : Interactive {
	public int ammoCost;
	public int ammoAmount;
	public override string message {
		get {
			if (AK47Shooter.instance.ammo < AK47Shooter.instance.maxAmmo) {
				return string.Format("Buy {0} ammo for {1} SYF", ammoAmount, ammoCost);
			}
			else {
				return "Ammo full";
			}
		}
	}

	public override void Action() {
		if (AK47Shooter.instance.ammo < AK47Shooter.instance.maxAmmo) {
			AK47Shooter.instance.ammo += ammoAmount;
			// TODO take away scrap as payment
		}
	}
}
