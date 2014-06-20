using UnityEngine;
using System.Collections;

public class AmmoCounter : MonoBehaviour {
	public int maxAmmo;
	private int _ammo;
	public TextMesh units;
	public TextMesh tens;
	public TextMesh hundreds;
	public GameObject indicator;
	public GameObject[] grenadeIndicators;
	public float blinkPeriod;

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

	public int _grenades;
	public int grenades {
		get { return _grenades; }
		set {
			_grenades = value;
			for (int i = 0; i < grenadeIndicators.Length; ++i) {
				grenadeIndicators[i].SetActive(i < value);
			}
		}
	}

	float ThresholdFunc(float input){
		float output = Mathf.Lerp(0.942f, 0.01f, input / maxAmmo);
		return output;
	}

	void Update() {
		if ((float)ammo / (float)maxAmmo < 0.1f) {
			indicator.SetActive(Mathf.Repeat(Time.time / blinkPeriod, 1) < 0.5f);
		}
		else {
			indicator.SetActive(true);
		}
	}
}
