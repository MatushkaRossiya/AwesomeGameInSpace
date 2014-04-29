using UnityEngine;

public class LightManager : MonoSingleton<LightManager> {
	Light[] lights;
	public AnimationCurve red;
	public AnimationCurve green;
	public AnimationCurve blue;

	public float dayPhase {
		set {
			Color color = new Color(
				red.Evaluate(value),
				green.Evaluate(value),
				blue.Evaluate(value));
			foreach (Light l in lights) {
				l.color = color;
			}
			RenderSettings.ambientLight = color * 0.1f + new Color(0.01f, 0.01f, 0.01f);
		}
	}


	public void FindLights() {
		var temp = GameObject.FindGameObjectsWithTag("Light");
		lights = new Light[temp.Length];
		for (int i = 0; i < temp.Length; ++i) {
			lights[i] = temp[i].GetComponent<Light>();
		}
	}

	public override void Init() {
		FindLights();
		dayPhase = 0;
	}
}
