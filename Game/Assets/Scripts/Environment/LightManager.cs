using UnityEngine;

public class LightManager : MonoSingleton<LightManager>
{
	public AnimationCurve red;
	public AnimationCurve green;
	public AnimationCurve blue;

	private Light[] lightsDay;
	private Light[] lightsNight;

	public float dayPhase
	{
		set
		{
			Color color = new Color(
                red.Evaluate(value),
                green.Evaluate(value),
                blue.Evaluate(value));

			RenderSettings.ambientLight = color * 0.025f + new Color(0.01f, 0.01f, 0.01f);
			_dayPhase = value;
		}
	}

	private float _dayPhase;

	void FixedUpdate()
	{
		if (_dayPhase != 0.0f)
		{
			foreach (Light light in lightsDay)
			{
				if (light.intensity != 1.0f && Random.Range(0, 60) == 0)
				{
					light.intensity = 1.0f;
				}
				else if (Random.Range(0, 60) == 0)
				{
					light.intensity = Random.Range(0.0f, 1.0f);
				}
			}

			foreach (Light light in lightsNight)
			{
				if (light.intensity != 1.0f && Random.Range(0, 60) == 0)
				{
					light.intensity = 1.0f;
				}
				else if (Random.Range(0, 60) == 0)
				{
					light.intensity = Random.Range(0.0f, 1.0f);
				}
			}
		}
	}

	public void FindLights()
	{
		GameObject[] tempD = GameObject.FindGameObjectsWithTag("LightDay");
		if (tempD != null)
		{
			lightsDay = new Light[tempD.Length];
			for (int i = 0; i < tempD.Length; ++i)
			{
				Light light = tempD [i].GetComponent<Light>();
				if (light != null)
				{
					lightsDay [i] = light;
				}
			}
		}

		GameObject[] tempN = GameObject.FindGameObjectsWithTag("LightNight");
		if (tempN != null)
		{
			lightsNight = new Light[tempN.Length];
			for (int i = 0; i < tempN.Length; ++i)
			{
				Light light = tempN [i].GetComponent<Light>();
				if (light != null)
				{
					lightsNight [i] = light;
				}
			}
		}
	}

	public void TurnLightsDay(bool on)
	{
		foreach (Light light in lightsDay)
		{
			light.enabled = on;
		}
	}

	public void TurnLightsNight(bool on)
	{
		foreach (Light light in lightsNight)
		{
			light.enabled = on;
		}
	}

	public override void Init()
	{
		FindLights();
		dayPhase = 0.0f;
	}
}
