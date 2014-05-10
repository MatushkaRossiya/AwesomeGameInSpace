using UnityEngine;
using System.Collections;

public class MedicalVendingMachine : Interactive {
	public int syringePrice;
	public override string message {
		get {
			if (PlayerHealth.instance.canBuySyringe) {
				return "Buy syringe for " + syringePrice + " SYF";
			}
			else {
				return "Syringes full";
			}
		}
	}

	public override void Action() {
		if (PlayerHealth.instance.canBuySyringe) {
			PlayerHealth.instance.syringes++;
			// TODO take away scrap as payment
		}
	}
}
