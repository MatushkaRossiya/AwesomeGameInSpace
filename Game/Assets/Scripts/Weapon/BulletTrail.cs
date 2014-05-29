using UnityEngine;
using System.Collections;

public class BulletTrail : MonoBehaviour {
	public Vector3 start;
	public Vector3 end;
	public float width;
	public float lifeTime = 0.4f;
	public float frequency;
	public float speed;
	public float density;
	private float age = 0;
	private LineRenderer bulletTrail;
	private int count;
	private Vector3[] positions;
	private Vector3[] velocities;
	

	void Start () {
		bulletTrail = GetComponent<LineRenderer>();
		Vector3 direction = end - start;
		Vector3 right = Vector3.Cross(direction, Vector3.up).normalized;
		Vector3 up = Vector3.Cross(direction, right).normalized;
		float length = direction.magnitude;
		count = Mathf.CeilToInt(length / width * density);
		velocities = new Vector3[count];
		positions = new Vector3[count];
		Vector3 increment = direction / count;

		bulletTrail.SetVertexCount(count);
		bulletTrail.SetWidth(width, width);
		bulletTrail.material.mainTextureScale = new Vector2(length / width * 0.25f, 1);

		float offset1 = Random.value * 100.0f;
		float offset2 = Random.value * 100.0f;
		for (int i = 0; i < count; ++i) {
			velocities[i] = speed * (right * (Mathf.PerlinNoise(0, i * frequency + offset1) - 0.5f) + up * (Mathf.PerlinNoise(0, i * frequency + offset2) - 0.5f));
			//velocities[i] = speed * (right * Mathf.Sin(frequency * i + offset1) + up * Mathf.Cos(frequency * i + offset1));
			positions[i] = start + increment * i;
		}
		SetPositions();
	}

	// Update is called once per frame
	void Update() {
		if (age > lifeTime) {
			Destroy(gameObject);
			return;
		}
		Color c = new Color(1, 1, 1, 1 - age / lifeTime);
		bulletTrail.SetColors(c, c);
		float w = width * (1 + (age / lifeTime) * 3.0f);
		bulletTrail.SetWidth(w, w);
		age += Time.deltaTime;
		bulletTrail.material.mainTextureOffset = new Vector2(age * -4.0f, 0);
		SetPositions();
	}

	private void SetPositions() {
		for (int i = 0; i < count; ++i) {
			positions[i] += velocities[i] * Time.deltaTime;
			bulletTrail.SetPosition(i, positions[i]);
		}
	}
}


