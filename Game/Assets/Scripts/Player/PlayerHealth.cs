using UnityEngine;
using System.Collections;

public class PlayerHealth : Damageable {
	public float health;
	private float maxHealth;
	private float currentHealth{
		get{
			return health;
		}
		set{
			health = value;
			GameObject.FindObjectOfType<HUD>().GetComponent<HUD>().updateHealth(health/maxHealth);	//powiadamia hud o zmianie zycia
			if(health < 0){
				Application.LoadLevel(1);
			}
		}
	}
	void Start(){
		maxHealth = health;
	}
	public override void DealDamage(Vector3 damage) {
		currentHealth -= damage.magnitude;
		rigidbody.AddForce(damage * 100.0f);
	}
}
