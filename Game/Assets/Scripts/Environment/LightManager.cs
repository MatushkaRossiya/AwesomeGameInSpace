using UnityEngine;

public class LightManager : MonoSingleton<LightManager>
{
	public AnimationCurve red;
	public AnimationCurve green;
	public AnimationCurve blue;

	private Light[] lightsDay;
	private Light[] lightsNight;
	private float[] lightsDayIntensity;
	private float[] lightsNightIntensity;

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
	private float _flickerTime = 0.0f;
	private float _nextFlicker = 0.1f;

	void FixedUpdate()
	{
		if (_dayPhase != 0.0f)
		{
			_flickerTime += Time.fixedDeltaTime;

			if (_flickerTime > _nextFlicker)
			{
				for (int i = 0; i < lightsDay.Length; ++i)
				{
					lightsDay [i].intensity = lightsDayIntensity [i] * Random.Range(0.0f, 1.0f);
				}

				/*for (int i = 0; i < lightsNight.Length; ++i)
				{
					lightsNight [i].intensity = lightsNightIntensity [i] * Random.Range(0.0f, 1.0f);
				}*/

				_flickerTime = 0.0f;
				_nextFlicker = Random.Range(0.25f, 0.5f);
			}

			/*foreach (Light light in lightsNight)
			{
				if (light.intensity != 1.0f && Random.Range(0, 60) == 0)
				{
					light.intensity = 1.0f;
				}
				else if (Random.Range(0, 60) == 0)
				{
					light.intensity = Random.Range(0.0f, 1.0f);
				}
			}*/
		}
	}

	public void FindLights()
	{
		GameObject[] tempD = GameObject.FindGameObjectsWithTag("LightDay");
		if (tempD != null)
		{
			lightsDay = new Light[tempD.Length];
			lightsDayIntensity = new float[tempD.Length];
			for (int i = 0; i < tempD.Length; ++i)
			{
				Light light = tempD [i].GetComponent<Light>();
				if (light != null)
				{
					lightsDay [i] = light;
					lightsDayIntensity [i] = light.intensity;
				}
			}
		}

		GameObject[] tempN = GameObject.FindGameObjectsWithTag("LightNight");
		if (tempN != null)
		{
			lightsNight = new Light[tempN.Length];
			lightsNightIntensity = new float[tempN.Length];
			for (int i = 0; i < tempN.Length; ++i)
			{
				Light light = tempN [i].GetComponent<Light>();
				if (light != null)
				{
					lightsNight [i] = light;
					lightsNightIntensity [i] = light.intensity;
				}
			}
		}
	}

	public void TurnLightsDay(bool on)
	{
		for (int i = 0; i < lightsDay.Length; ++i)
		{
			lightsDay [i].enabled = on;
			lightsDay [i].intensity = lightsDayIntensity [i];
		}
	}

	public void TurnLightsNight(bool on)
	{
		for (int i = 0; i < lightsNight.Length; ++i)
		{
			lightsNight [i].enabled = on;
			lightsNight [i].intensity = lightsNightIntensity [i];
		}
	}

	public override void Init()
	{
		FindLights();
		dayPhase = 0.0f;
	}
}
