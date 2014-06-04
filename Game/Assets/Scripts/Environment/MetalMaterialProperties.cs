using UnityEngine;
using System.Collections;

public class MetalMaterialProperties : MaterialProperties {
	public GameObject bulletHolePrefab;

	private float nextBulletHoleDepth = 0.002f;

	public override void Hit(RaycastHit hit, Vector3 force) {
		GameObject hole = Instantiate(bulletHolePrefab, hit.point + nextBulletHoleDepth * hit.normal, Quaternion.LookRotation(hit.normal)) as GameObject;
			hole.transform.parent = hit.transform;
			BulletHoleManager.instance.AddBulletHole(hole);
			nextBulletHoleDepth += 0.001f;
			if (nextBulletHoleDepth > 0.01f)
				nextBulletHoleDepth = 0.002f;
	}
}
