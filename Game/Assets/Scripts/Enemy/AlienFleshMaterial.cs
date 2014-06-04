using UnityEngine;
using System.Collections;

public class AlienFleshMaterial : MaterialProperties {
	public GameObject bloodParticlesPrefab;
	
	private static ParticleSystem bloodParticles;
	
	
	public override void Hit(RaycastHit hit, Vector3 force) {
		try {
			Emit(hit, force);
		}
		catch {
			bloodParticles = ((GameObject)Instantiate(bloodParticlesPrefab)).GetComponent<ParticleSystem>();
			Emit(hit, force);
		}
	}

	void Emit(RaycastHit hit, Vector3 force) {
		bloodParticles.transform.position = hit.point;
		bloodParticles.transform.forward = -force.normalized;
		bloodParticles.Emit(20);
	}
}
