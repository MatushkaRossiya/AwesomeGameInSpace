using UnityEngine;
using System.Collections;

public class BlockadeElement : Damageable {
	public Blockade parent;

	public override void DealDamage(Vector3 damage) {
		parent.DealDamage(gameObject, damage);
	}
}
