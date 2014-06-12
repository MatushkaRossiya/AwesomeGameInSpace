using UnityEngine;
using System.Collections;

public class AmmoClipUpgrade : Interactive {
	public GameObject clip;
	
	public bool _pickedUp;
	public bool pickedUp {
		get {
			return _pickedUp;
		}
		set {
			_pickedUp = value;
			clip.renderer.enabled = !value;
		}
	}

	public override string message {
		get {
			return pickedUp ?
				 null :
				 "Pick up ammo upgrade";
		}
	}

	public override void MomentaryAction() {
		if (!pickedUp) {
			pickedUp = true;
			LaserRifle rifle = LaserRifle.instance;
			rifle.hasAmmoClipUpgrade = true;
			rifle.ammo = rifle.maxAmmo;
		}
	}
}
