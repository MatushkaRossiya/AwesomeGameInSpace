using UnityEngine;
using System.Collections.Generic;

public class Blockade : MonoBehaviour {
	public List<GameObject> blockadeComponents;
	public float hitPoints;
	public float maxHitpoints;
	public float forceMultiplier;
	public float destroyedThreshold = 0.5f;
	private int currentComponentCount;
	private int lastComponentCount;
	private Vector3 forceDirection;
	private NavMeshObstacle[] obstacles;
	bool _destroyed;
	bool destroyed{
		get{
			return _destroyed;
		}
		set {
			if(value && !_destroyed){
				foreach (var obstacle in obstacles) {
					obstacle.enabled = false;
				}
			}else if(_destroyed && !value){
				foreach (var obstacle in obstacles) {
					obstacle.enabled = true;
				}
			}
			_destroyed = value;
		}
	}

	public float hitPointsPercentage {
		get {
			return hitPoints / maxHitpoints;
		}
		set {
			hitPoints = Mathf.Clamp01(value) * maxHitpoints;
		}
	}

	void Start() {
		obstacles = GetComponentsInChildren<NavMeshObstacle>();
		destroyed = hitPoints <= 0;
		lastComponentCount = blockadeComponents.Count;
		currentComponentCount = hitPointsPercentage <= 0 ? 0 : (int)((1 / destroyedThreshold - 1 + hitPointsPercentage) * destroyedThreshold * blockadeComponents.Count);
		for (int i = currentComponentCount; i < lastComponentCount; ++i) {
			blockadeComponents[i].SetActive(false);
		}
		lastComponentCount = currentComponentCount;
	}

	private void SetActiveComponents() {
		currentComponentCount = hitPointsPercentage <= 0 ? 0 : (int)((1 / destroyedThreshold - 1 + hitPointsPercentage) * destroyedThreshold * blockadeComponents.Count);
		for (int i = lastComponentCount; i < currentComponentCount; ++i) {
			ActivateComponent(blockadeComponents[i]);
		}
		for (int i = currentComponentCount; i < lastComponentCount; ++i) {
			DeactivateComponent(blockadeComponents[i]);
		}
		lastComponentCount = currentComponentCount;
		destroyed = hitPoints < 0;
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
		hitPoints = Mathf.Max(0, hitPoints - damage.magnitude);
		SetActiveComponents();
	}

	public void Repair(float repairAmount) {
		hitPointsPercentage += repairAmount;
		SetActiveComponents();
	}
}
