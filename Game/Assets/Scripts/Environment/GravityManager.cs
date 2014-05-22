using UnityEngine;
using System.Collections;

public class GravityManager : MonoSingleton<GravityManager> {
	public AnimationCurve curve;
	public float gravity {
		get {
			return Physics.gravity.magnitude / 9.81f;
		}
		set {
			Physics.gravity = new Vector3(0, -9.81f * curve.Evaluate(value), 0);
		}
	}
}
