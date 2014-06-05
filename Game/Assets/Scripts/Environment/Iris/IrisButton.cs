using UnityEngine;
using System.Collections;

public class IrisButton : Interactive {
	public int cost;
	public Iris iris;


	public override string message {
		get {
			if (PlayerStats.instance.syf >= cost) {
				return "Hold to open for " + cost + " syf";
			}
			else {
				return "Need " + cost + " syf";
			}
			
		}
	}

	public override void HoldAction() {
		if (PlayerStats.instance.syf >= cost) {
			iris.Action();
			Destroy(this);
		}
	}
}
