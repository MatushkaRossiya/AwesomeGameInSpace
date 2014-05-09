using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoSingleton<PlayerHealth> {
	public float maxHealth;
	private float _health;
	public float health{
		get{
			return _health;
		}
		set{
			_health = Mathf.Clamp(value, 0, maxHealth);
			//GameObject.FindObjectOfType<HUD>().GetComponent<HUD>().updateHealth(_health/maxHealth);	//powiadamia hud o zmianie zycia
			if(_health == 0){
				Application.LoadLevel(1);
			}
		}
	}

	public int maxSyringes;
	private int _syringes;
	public int syringes{
		get{
			return _syringes;
		}
		set{
			_syringes = value;
			Debug.Log("You now have " + _syringes + " syringes");
		}
	}


	public float syringeHealAmount;


	void Start(){
		_health = maxHealth;
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.H)) {
			UseSyringe();
		}
	}

	private bool UseSyringe() {
		if (syringes > 0) {
			syringes--;
			health += syringeHealAmount;
			return true;
		}
		return false;
	}

	public float healthPercentage {
		get {
			return health / maxHealth;
		}
	}

	public bool canBuySyringe {
		get { return syringes < maxSyringes; }
	}
}
