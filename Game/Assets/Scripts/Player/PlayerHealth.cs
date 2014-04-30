using UnityEngine;
using System.Collections;

public class PlayerHealth : Damageable {
	public float health;
	private float currentHealth{
		get{
			return health;
		}
		set{
			health = value;
			if(health < 0){
				Application.LoadLevel(1);
			}
		}
	}

	public override void DealDamage(Vector3 damage) {
		currentHealth -= damage.magnitude;
		rigidbody.AddForce(damage * 100.0f);
	}
}
