using UnityEngine;
using System.Collections;

public class Scratch : MonoBehaviour
{
	public AudioClip[] scratches;

	private float scratchLenght;
	private float pauseLenght;
	private float time = 0.0f;
	private bool playing = false;
	private int scratchIndex;
	private int scratchesLenght;

	void Start()
	{
		scratchesLenght = scratches.Length;
	}

	void FixedUpdate()
	{
		if (!playing)
		{
			time = 0.0f;
			playing = true;
			scratchIndex = Random.Range(0, scratchesLenght);
			audio.clip = scratches[scratchIndex];
			scratchLenght = scratches[scratchIndex].length;
			float pauseModifier = 1.0f - audio.volume;
			pauseLenght = Random.Range(2.0f * pauseModifier + 0.1f, 5.0f * pauseModifier + 0.1f);
			audio.Play();
		}
		else
		{
			time += Time.fixedDeltaTime;
			if (time > scratchLenght + pauseLenght)
			{
				playing = false;
			}
			if (time > scratchLenght)
			{
				audio.Stop();
			}
		}
	}
}
