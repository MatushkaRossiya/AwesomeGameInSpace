﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class Menu : MonoBehaviour {
	enum MenuSelector {
		PLAY,MAIN,OPTIONS,EXIT,CREDITS,STARTNEWGAME,LOADGAME
	}
	MenuSelector currMenu;
	float ScrHeight = Screen.height;
	float ScrWidth = Screen.width;
	float width = 0.3f*Screen.width;
	float height = 0.2f*Screen.height;
	public Texture2D backgroundIMG;
	public Texture2D creditsIMG;
	public AudioClip click, hover;
	
	void Start () {
		currMenu = MenuSelector.MAIN;
		setUpRects ();
	}

	void Update () {
		if (Screen.height != ScrHeight || Screen.width != ScrWidth)
			setUpRects ();	//W przypadku gdyby nastapila zmiana rozdzielczosci
	}
	void OnGUI ()
		{
				//TEXTURES
				GUI.DrawTexture (guiTextureRect, backgroundIMG, ScaleMode.ScaleAndCrop);
				//BUTONS
				if (GUI.Button (startButtonRect.rect, "PLAY")) {
						playSound ();
						goToMenu (MenuSelector.PLAY);
				} else if (GUI.Button (optionsButtonRect.rect, "OPTIONS")) {
						playSound ();
						goToMenu (MenuSelector.OPTIONS);
				} else if (GUI.Button (exitButtonRect.rect, "EXIT")) {
						playSound ();
						goToMenu (MenuSelector.EXIT);
				} else if (GUI.Button (yesButtonRect.rect, "YES")) {
						playSound ();
						if (currMenu == MenuSelector.EXIT) 
								Application.Quit ();
						else if (currMenu == MenuSelector.STARTNEWGAME)
								Application.LoadLevel (1);
				} else if (GUI.Button (noButtonRect.rect, "NO")) {
						playSound ();
						if (currMenu == MenuSelector.EXIT)
								goToMenu (MenuSelector.MAIN);
						else if (currMenu == MenuSelector.STARTNEWGAME)
								goToMenu (MenuSelector.PLAY);
				} else if (GUI.Button (previousButtonRect.rect, "MAIN")) {
						playSound ();
						goToMenu (MenuSelector.MAIN);
				}if (Loader.instance.areThereAnySaves () == false) {
						GUI.enabled=false;
						GUI.Button (continueButtonRect.rect, "CONTINUE");
						GUI.enabled=true;
				}
				else if(GUI.Button (continueButtonRect.rect, "CONTINUE")) {
						playSound ();
						Loader.instance.load ();//default parametr dla autosave'a
				} if(GUI.Button (newGameButtonRect.rect, "NEW GAME")) {
						Loader.instance.load("NewGame");
						playSound();
				}
				else if(GUI.Button (loadButtonRect.rect, "LOAD GAME")){	
					goToMenu(MenuSelector.LOADGAME);
					playSound();
				}
				if(savesBtnsRects!=null)
				foreach (var rect in savesBtnsRects) {
						if (GUI.Button (rect.Value.rect, rect.Key)) {
								playSound ();
								Loader.instance.load (rect.Key);
						}
				}
				
				if (GUI.Button (polishSmallBtnRect.rect, "POLISH")) {
                    Loc.instance.setLanguage(Loc.Language.POLISH);
				} else if (GUI.Button (englishSmallBtnRect.rect, "ENGLISH")) {
                    Loc.instance.setLanguage(Loc.Language.ENGLISH);
				} else if (GUI.Button (enableTutorialbtnRect.rect, "ENABLE")) {
                    PlayerPrefs.SetInt("tutorial", 1);
				} else if (GUI.Button (disableTutorialbtnRect.rect,"DISABLE")){
                    PlayerPrefs.SetInt("tutorial", 0);
				}
				
				//LABELS
				GUI.Box(languageButtonRect.rect,"CHOOSE LANGUAGE");
				GUI.Box(tutorialswitchButtonRect.rect,"TUTORIAL ON/OFF");
				
				if (currMenu != MenuSelector.STARTNEWGAME) 
						GUI.Box (doYouwantToExitLabelRect.rect, "DO YOU WANT TO EXIT?");
				
				if (currMenu != MenuSelector.EXIT) 
						GUI.Box (doYouWantToStartNewGame.rect, "DO YOU WANT TO START NEW GAME?");
				
		}
	void playSound()
	{
		AudioSource source = GetComponent<AudioSource> ();
		source.clip = click;
		source.Play ();
	}
}
