using UnityEngine;
using System.Collections;

public class BulletTrail : MonoBehaviour {
	public float lifeTime = 0.4f;
	private float age = 0;
	LineRenderer bulletTrail;
	void Start () {
		bulletTrail = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (age > lifeTime) {
			Destroy(gameObject);
			return;
		}
		Color c = new Color(1, 1, 1, 1 - age / lifeTime);
		bulletTrail.SetColors(c, c);
		age += Time.deltaTime;
		bulletTrail.material.mainTextureOffset = new Vector2(age * -4.0f, 0);
	}
}
