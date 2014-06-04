using UnityEngine;
using System.Collections;

public class MetalMaterialProperties : MaterialProperties {
	public GameObject bulletHolePrefab;
	public GameObject sparksParticlesPrefab;

	private float nextBulletHoleDepth = 0.002f;
	private static ParticleSystem sparksParticles;

	public override void Hit(RaycastHit hit, Vector3 force) {
        GameObject hole = Instantiate(bulletHolePrefab, hit.point + nextBulletHoleDepth * hit.normal, Quaternion.LookRotation(hit.normal)) as GameObject;
		hole.transform.parent = hit.transform;
		BulletHoleManager.instance.AddBulletHole(hole);
		nextBulletHoleDepth += 0.002f;
		if (nextBulletHoleDepth > 0.01f)
			nextBulletHoleDepth = 0.002f;

		try {
			Emit(hit, force);
		}
		catch {
			sparksParticles = ((GameObject)Instantiate(sparksParticlesPrefab)).GetComponent<ParticleSystem>();
			Emit(hit, force);
		}
	}

	void Emit(RaycastHit hit, Vector3 force) {
		sparksParticles.transform.position = hit.point;
		sparksParticles.transform.forward = hit.normal;
		sparksParticles.Emit(20);
	}
}
