using UnityEngine;
using System.Collections;

public class SyfCollectible : Interactive {
	public int value;

	void Start() {
		StartCoroutine(DelayDie());
	}
	
	public override string message {
		get { return "Pick up Syf"; }
	}

	public override void MomentaryAction() {
		PlayerStats.instance.syf += value;
		Destroy(gameObject);
	}

	IEnumerator DelayDie() {
		yield return new WaitForSeconds(Random.Range(60.0f, 90.0f));
		Destroy(gameObject);
	}
}
