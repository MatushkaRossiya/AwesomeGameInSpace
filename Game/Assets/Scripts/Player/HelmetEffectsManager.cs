using UnityEngine;
using System.Collections.Generic;

public class HelmetEffectsManager : MonoSingleton<HelmetEffectsManager> {
	public Shader gooShader;
	public Texture[] effectTextures;
	public bool[] allowRotation;
	public GameObject quad;

	public float lifeTime;

	float lastZ = 0.00001f;

	private Queue<Effect> queue;

	public void AddDamageEffect() {
		float posX = Random.Range(-1.0f, 1.0f) * camera.aspect * 0.7f;
		float posY = Random.Range(-1.0f, 1.0f) * 0.7f;
		lastZ += 0.00001f;
		float posZ = Mathf.Lerp(camera.nearClipPlane, camera.farClipPlane, lastZ);

		float size = Random.Range(0.2f, 0.5f);

		int index = Random.Range(0, effectTextures.Length);
		Quaternion rotation;

		if(allowRotation[index]){
			rotation = Quaternion.Euler(new Vector3(0, 180, Random.value * 360.0f));
		}else{
			rotation = Quaternion.Euler(new Vector3(0, 180, 180));
		}
		Material mat = new Material(gooShader);
		mat.mainTexture = effectTextures[index];

		GameObject q = (GameObject)Instantiate(quad);
		q.transform.parent = transform;
		q.renderer.material = mat;
		q.transform.localScale = new Vector3(size, size * effectTextures[index].height / effectTextures[index].width, 1);
		q.transform.localPosition = new Vector3(posX, posY, posZ);
		q.transform.localRotation = rotation;
		q.layer = 31;

		queue.Enqueue(new Effect(q));
	}

	// Use this for initialization
	void Start () {
		queue = new Queue<Effect>();
	}
	
	// Update is called once per frame
	void Update () {
		foreach (var eff in queue) {
			eff.visibility -= Time.deltaTime / lifeTime;
			eff.gameObject.renderer.material.SetFloat("_Visibility", eff.visibility);
		}
		if (queue.Count > 0) {
			Effect first = queue.Peek();
			if (first.visibility <= 0) {
				Destroy(first.gameObject);
				queue.Dequeue();
			}
		}
	}

	private class Effect {
		public GameObject gameObject;
		public float visibility;

		public Effect(GameObject eff) {
			this.gameObject = eff;
			visibility = 1.0f;
		}
	}				 
}
