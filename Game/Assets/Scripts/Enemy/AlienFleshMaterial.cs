using UnityEngine;
using System.Collections;

public class AlienFleshMaterial : MaterialProperties {
	public GameObject bloodParticlesPrefab;
	public GameObject bloodSplashPrefab;
	
	private static ParticleSystem bloodParticles;
	private static float offset = 0.0001f;
	
	public override void Hit(RaycastHit hit, Vector3 force) {
		try {
			Emit(hit, force);
		}
		catch {
			bloodParticles = ((GameObject) Instantiate(bloodParticlesPrefab)).GetComponent<ParticleSystem>();
			Emit(hit, force);
		}
		RaycastHit hitInfo;
		if(Physics.Raycast(hit.point, Random.onUnitSphere, out hitInfo, 4, Layers.environment)){
			GameObject splash = (GameObject)Instantiate(bloodSplashPrefab);
			splash.transform.position = hitInfo.point + hitInfo.normal * offset;
			splash.transform.rotation = Quaternion.LookRotation(hitInfo.normal, Random.onUnitSphere);
			splash.transform.parent = hitInfo.transform;
			BloodSplashManager.instance.AddSplash(splash);
			offset += 0.0001f;
			if (offset > 0.001f) offset = 0.0001f;
		}
	}

	void Emit(RaycastHit hit, Vector3 force) {
		bloodParticles.transform.position = hit.point;
		bloodParticles.transform.forward = -force.normalized;
		bloodParticles.Emit(20);
	}
}
