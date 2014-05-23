using UnityEngine;
using System.Collections;

public class SyfCollectible : Interactive {
	public int value;

	void Start() {
		StartCoroutine(DelayDie());
	}
	
	public override string message {
		get { return "Pick up syf"; }
	}

	public override void Action() {
		PlayerStats.instance.syf += value;
		Destroy(gameObject);
	}

	IEnumerator DelayDie() {
		yield return new WaitForSeconds(Random.Range(30.0f, 60.0f));
		Destroy(gameObject);
	}
}
