using UnityEngine;
using System.Collections;

public class AmmoCounter : MonoSingleton<AmmoCounter> {
	public int maxAmmo;
	private int _ammo;
	public TextMesh number;
	public GameObject indicator;
	private Material indicatorMat;
	private int ID;

	void Start() {
		ID = Shader.PropertyToID("_Cutoff");
		indicatorMat = indicator.renderer.material;
	}

	public int ammo {
		get { return _ammo; }
		set {
			indicatorMat.SetFloat(ID, ThresholdFunc(value));
			number.text = string.Format("{0:000}", value);
			_ammo = value;
		}
	}

	float ThresholdFunc(float input){
		float output = Mathf.Lerp(0.942f, 0.01f, input / maxAmmo);
		return output;
	}
}
