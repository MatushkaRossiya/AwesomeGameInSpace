using UnityEngine;
using System.Collections.Generic;

public class Blockade : MonoBehaviour {
	public List<GameObject> blockadeComponents;
	public float hitPoints;
	public float maxHitpoints;
	public float forceMultiplier;
	private int currentComponentCount;
	private int lastComponentCount;
	private Vector3 forceDirection;

	void Start() {
		lastComponentCount = blockadeComponents.Count;
		SetActiveComponents();
	}

	private void SetActiveComponents() {
		float percentage = 0.5f;
		currentComponentCount = (hitPoints / maxHitpoints) <= 0 ? 0 : (int)((1 / percentage - 1 + hitPoints / maxHitpoints) * percentage * blockadeComponents.Count);
		Debug.Log(currentComponentCount);
		for (int i = lastComponentCount; i < currentComponentCount; ++i) {
			ActivateComponent(blockadeComponents[i]);
		}
		for (int i = currentComponentCount; i < lastComponentCount; ++i) {
			DeactivateComponent(blockadeComponents[i]);
		}
		lastComponentCount = currentComponentCount;
	}

	private void ActivateComponent(GameObject component) {
		component.SetActive(true);
	}

	private void DeactivateComponent(GameObject component){
		GameObject copy = Instantiate(component, component.transform.position, component.transform.localRotation) as GameObject;
		Destroy(copy.GetComponent<Damageable>());
		copy.AddComponent<SyfCollectible>().value = 1;
		Rigidbody body = copy.AddComponent<Rigidbody>();
		body.mass = 5;
		body.AddForce(forceDirection * forceMultiplier, ForceMode.Impulse);
		
		component.SetActive(false);
	}

	public void DealDamage(GameObject target, Vector3 damage) {
		forceDirection = damage;
		hitPoints -= damage.magnitude;
		SetActiveComponents();
	}
}
