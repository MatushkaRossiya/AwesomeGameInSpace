using UnityEngine;
using System.Collections;

public class AmmoCounter : MonoBehaviour {
	public int maxAmmo;
	private int _ammo;
	public TextMesh units;
	public TextMesh tens;
	public TextMesh hundreds;
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
			units.text = (value % 10).ToString();
			tens.text = (value / 10 % 10).ToString();
			hundreds.text = (value / 100 % 10).ToString();
			_ammo = value;
		}
	}

	float ThresholdFunc(float input){
		float output = Mathf.Lerp(0.942f, 0.01f, input / maxAmmo);
		return output;
	}
}
