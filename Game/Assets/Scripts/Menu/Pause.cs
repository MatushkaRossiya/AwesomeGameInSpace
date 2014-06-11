using UnityEngine;
using System.Collections;
using System;

public class Pause : MonoBehaviour
{
	bool paused;
	LTRect mainMenuBtnRect;
	LTRect continueBtnRect;
	LTRect saveBtnRect;
	LTRect PausedLabelRect;
	float ScrHeight = Screen.height;
	float ScrWidth = Screen.width;
	float gamespeed;
	float timeToWait = 0.0f;

	public Texture2D buttonOnTexture;
	public Texture2D buttonOffTexture;
	public Texture2D buttonHoverTexture;
	public GUIStyle fontStyle;
	public AudioClip click;

	// Use this for initialization
	void Start()
	{
		paused = false;
		setupRects();
		gamespeed = 1.0f;
	}

	void setupRects()
	{
		float btnWidth = 0.3f * ScrWidth;
		float btnHeight = 0.1f * ScrHeight;
		//float horCenter = 0.5f * ScrWidth - 0.5f*btnWidth;
		float verCcenter = 0.5f * ScrHeight - 0.5f * btnHeight;
		continueBtnRect = new LTRect(ScrWidth, verCcenter - 1.5f * btnHeight, btnWidth, btnHeight);
		mainMenuBtnRect = new LTRect(ScrWidth, verCcenter + 1.5f * btnHeight, btnWidth, btnHeight);
		saveBtnRect = new LTRect(ScrWidth, verCcenter, btnWidth, btnHeight);
	}

	void pauseGame()
	{
		gamespeed = 0.0f;
		showButtons(-0.65f, 0, 0, continueBtnRect, saveBtnRect, mainMenuBtnRect);
		//Debug.Log("Game Paused");
		paused = true;
		timeToWait = Time.realtimeSinceStartup;
	}

	void unPauseGame()
	{
		gamespeed = 1.0f;
		Time.timeScale = gamespeed;
		hideButtons(0.65f, 0, 0, continueBtnRect, saveBtnRect, mainMenuBtnRect);
		//Debug.Log("Game unPaused");
		paused = false;
		timeToWait = Time.realtimeSinceStartup;
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Delete) && timeToWait <= 0.0f)
		{
			if (paused)
			{
				unPauseGame();
			}
			else
			{
				pauseGame();
			}
		}

		if (timeToWait > 0.0f)
		{
			if (Time.realtimeSinceStartup - timeToWait > 0.5f)
			{
				timeToWait = 0.0f;
			}
		}
	}

	void OnGUI()
	{
		if (GUI.Button(continueBtnRect.rect, "CONITNUE", fontStyle))
		{
			unPauseGame();
			playSound();
		}
		if (GUI.Button(mainMenuBtnRect.rect, "MAIN MENU", fontStyle))
		{
			unPauseGame();
			playSound();
			Application.LoadLevel(0);
		}
		if (GUI.Button(saveBtnRect.rect, "SAVE", fontStyle))
		{
			unPauseGame();
			playSound();
			Loader.instance.save(System.DateTime.Now.ToString());
		}
	}

	IEnumerator moveRects(float interval, float timeToMove, float delay, float endValX/*o ile przesunac w prawo relatywne do ekranu*/,
	                      float endValY/*o ile przesunac w gore relatywne do ekranu*/, params LTRect[] rectts)
	{
		yield return new WaitForSeconds(delay);
		foreach (var rectt in rectts)
		{
			LeanTween.move(rectt, new Vector2(rectt.rect.x + ScrWidth * endValX, rectt.rect.y + ScrHeight * endValY), timeToMove).setEase(LeanTweenType.easeOutQuad);
			yield return new WaitForSeconds(interval);
		}
		Screen.lockCursor = !paused;
		Time.timeScale = gamespeed;
	}

	void showButtons(float directionX, float directionY, float additonaldelay, params LTRect[] rectts)
	{
		StartCoroutine(moveRects(0.08f, 0.08f, additonaldelay, directionX, directionY, rectts));
	}

	void hideButtons(float directionX, float directionY, float additonaldelay, params LTRect[] rectts)
	{
		StartCoroutine(moveRects(0.2f, 0.18f, additonaldelay, directionX, directionY, rectts));
	}

	void playSound()
	{
		audio.PlayOneShot(click);
	}
}
