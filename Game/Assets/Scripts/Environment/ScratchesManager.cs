using UnityEngine;
using System.Collections;

public class ScratchesManager : MonoSingleton<ScratchesManager>
{
	public float dayPhase
	{
		set
		{
			_dayPhase = value;
			for (int i = 0; i < scratches.Length; ++i)
			{
				scratches [i].volume = (_dayPhase / GameMaster.instance.dayLenght) + 0.1f;
			}
		}
	}
	private float _dayPhase;

	private AudioSource[] scratches;
	
	void Start()
	{
	
	}

	public void TurnScratches(bool on)
	{
		for (int i = 0; i < scratches.Length; ++i)
		{
			scratches [i].enabled = on;
		}
	}

	public void FindScratches()
	{
		GameObject[] temp = GameObject.FindGameObjectsWithTag("Scratch");
		if (temp != null)
		{
			scratches = new AudioSource[temp.Length];
			for (int i = 0; i < temp.Length; ++i)
			{
				AudioSource scratch = temp [i].GetComponent<AudioSource>();
				if (scratch != null)
				{
					scratches [i] = scratch;
				}
			}
		}
	}

	public override void Init()
	{
		FindScratches();
		dayPhase = 0.0f;
	}
}
