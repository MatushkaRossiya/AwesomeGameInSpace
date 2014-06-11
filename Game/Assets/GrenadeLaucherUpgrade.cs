using UnityEngine;
using System.Collections;

public class GrenadeLaucherUpgrade : Interactive {
	public GameObject[] objs;

	public bool _pickedUp;
	public bool pickedUp {
		get {
			return _pickedUp;
		}
		set {
			_pickedUp = value;
			foreach(var obj in objs){
				Destroy(obj);
			}
		}
	}

	public override string message {
		get {
			return pickedUp ?
				 null :
				 "Pick up grenade laucher upgrade";
		}
	}

	public override void MomentaryAction() {
		if (!pickedUp) {
			pickedUp = true;
			LaserRifle rifle = LaserRifle.instance;
			rifle.hasGrenadeLaucherUpgrade = true;
			rifle.GLModel.SetActive(true);
			rifle.grenades = rifle.maxGrenades;
		}
	}
}
