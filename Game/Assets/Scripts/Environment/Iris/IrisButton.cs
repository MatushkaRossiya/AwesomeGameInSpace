using UnityEngine;
using System.Collections;

public class IrisButton : Interactive {
	public int cost;
	public Iris iris;
	public int nextLevelID;

	private bool locked = true;
	private AsyncOperation level = null;

	public override string message {
		get {
			if (locked) {
				if (PlayerStats.instance.syf >= cost) {
					return "Hold to open for " + cost + " syf";
				}
				else {
					return "Need " + cost + " syf";
				}
			}
			else {
				if (level == null || (level != null && level.progress >= 0.9f)) {
					return "Hold to open";
				}
				else {
					return "Pressurising.. Please wait";
				}
			}
		}
	}

	public override void HoldAction() {
		if (!locked || (locked && PlayerStats.instance.syf >= cost)) {
			if (locked) PlayerStats.instance.syf -= cost;
			locked = false;
			if (level != null) {
				if (level.progress >= 0.9f) {
					iris.Action();
					level.allowSceneActivation = true;
					Destroy(this);
				}
			}
			else {
				iris.Action();
				Destroy(this);
			}
		}
	}

	void Start() {
		StreamLevel();
	}


	void StreamLevel() {
		if (nextLevelID >= 0) {
			level = Application.LoadLevelAdditiveAsync(nextLevelID);
			level.allowSceneActivation = false;
		}
	}
}
