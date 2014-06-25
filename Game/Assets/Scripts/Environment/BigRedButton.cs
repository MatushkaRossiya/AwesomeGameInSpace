using UnityEngine;
using System.Collections;

public class BigRedButton : Interactive {
	public AudioClip voice1;
	public AudioClip voice2;
	public Light escapePodLight;

	public AudioSource voiceSource;
	public AudioSource sirenSource;
	public AudioSource explosionSource;

	public ParticleSystem giantExplosionParticles;


	public override string message {
		get { return "PUSH ME!!!!"; }
	}

	public override void MomentaryAction() {
		StartCoroutine(selfDestruct());
	}

	IEnumerator selfDestruct() {
		yield return new WaitForSeconds(1.0f);
		voiceSource.clip = voice1;
		voiceSource.Play();
		escapePodLight.color = Color.red;
		yield return new WaitForSeconds(voice1.length);
		sirenSource.Play();
		yield return new WaitForSeconds(3.0f);
		voiceSource.clip = voice2;
		voiceSource.Play();
		yield return new WaitForSeconds(voice2.length);
		explosionSource.Play();
		giantExplosionParticles.enableEmission = true;
		// TODO delete saved games
	}
}
