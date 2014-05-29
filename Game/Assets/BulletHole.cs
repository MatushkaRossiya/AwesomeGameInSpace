using UnityEngine;
using System.Collections;

public class BulletHole : MonoBehaviour {
	public float brightness;
	Material mat;
	int ID;
	void Start () {
		mat = renderer.material;
		ID = Shader.PropertyToID("_EmissionLM");
	}
	
	void Update () {
		brightness -= Time.deltaTime * 0.5f;
		if (brightness <= 0) Destroy(this);
		mat.SetFloat(ID, brightness);
	}
}
