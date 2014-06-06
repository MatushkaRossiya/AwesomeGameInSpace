using UnityEngine;
using System.Collections;

public class Iris : RemoteActor {
	public float openingTime;
	public NavMeshObstacle[] obstacles;
	public AnimationCurve curve;

	private Transform[] blades;
	private float _angle;
	private float angle{
		get{ return _angle; }
		set{
			_angle = value;
			for (int i = 0; i < 20; ++i) {
				blades[i].localRotation = Quaternion.Euler(angle / 50, 0, _angle - 18 * i + 2);
			}
		}
	}

	private bool _isOpen;
	public bool isOpen {
		get { return _isOpen; }
		private set {
			_isOpen = value;
			dir = 1 / openingTime * (value ? 1.0f : -1.0f);
			audio.Play();
		}
	}

	private float phase;
	private float dir;

	void Start () {
		blades = new Transform[20];
		for (int i = 0; i < 20; ++i) {
			blades[i] = transform.GetChild(i);
		}
	}

	// Update is called once per frame
	void Update() {
		if (dir != 0) {
			phase += dir * Time.deltaTime;
			if (phase > 1.0f) {
				dir = 0.0f;
				phase = 1.0f;
			}
			else if (phase < 0) {
				dir = 0.0f;
				phase = 0.0f;
			}
			foreach (var obst in obstacles) {
				obst.enabled = phase < 0.5f;
			}
			angle = curve.Evaluate(phase) * 51;
		}
	}

	public override void Action()
	{
		isOpen = !isOpen;
	}
}
