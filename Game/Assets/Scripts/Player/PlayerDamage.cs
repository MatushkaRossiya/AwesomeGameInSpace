using UnityEngine;
using System.Collections;

public class PlayerDamage : Damageable {
	PlayerHealth playerHealth;
	// Use this for initialization
	void Start () {
		playerHealth = GetComponent<PlayerHealth>();
	}

	public override void DealDamage(Vector3 damage) {
		playerHealth.health -= damage.magnitude;
		playerHealth.rigidbody.AddForce(damage * 100.0f);
	}
}
