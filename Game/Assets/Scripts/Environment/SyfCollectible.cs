using UnityEngine;
using System.Collections;

public class SyfCollectible : Interactive {
	public int value;
	public AudioClip pickUpSound;

	void Start() {
		StartCoroutine(DelayDie());
	}
	
	public override string message {
		get { return "Pick up Syf"; }
	}

	public override void MomentaryAction() {
		PlayerStats.instance.syf += value;
		audio.PlayOneShot(pickUpSound);
		//Destroy(gameObject);
		GetComponent<MeshRenderer>().enabled = false;
		GetComponent<BoxCollider>().enabled = false;
		StartCoroutine(DelayDie2());
	}

	IEnumerator DelayDie() {
		yield return new WaitForSeconds(Random.Range(90.0f, 120.0f));
		Destroy(gameObject);
	}

	IEnumerator DelayDie2() {
		yield return new WaitForSeconds(pickUpSound.length);
		Destroy(gameObject);
	}
}
