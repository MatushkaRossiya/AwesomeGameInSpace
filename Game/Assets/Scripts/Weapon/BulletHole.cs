using UnityEngine;
using System.Collections;

public class BulletHole : MonoBehaviour {
	public float brightness;
	private Material mat;
	private int ID;

	void Start () {
		mat = renderer.material;
		ID = Shader.PropertyToID("_EmissionLM");
	}
	
	void FixedUpdate () {
		brightness -= Time.fixedDeltaTime * 0.5f;
		if (brightness <= 0) Destroy(this);
		mat.SetFloat(ID, brightness);
	}
}
