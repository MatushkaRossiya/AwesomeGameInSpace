using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class Menu : MonoBehaviour {
	enum MenuSelector {
		PLAY,MAIN,OPTIONS,EXIT,CREDITS,STARTNEWGAME
	}
	MenuSelector currMenu;
	float ScrHeight = Screen.height;
	float ScrWidth = Screen.width;
	float width = 0.3f*Screen.width;
	float height = 0.2f*Screen.height;
	public Texture2D backgroundIMG;
	public Texture2D creditsIMG;
	
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
						goToMenu (MenuSelector.PLAY);
				} else if (GUI.Button (optionsButtonRect.rect, "OPTIONS")) {
						goToMenu (MenuSelector.OPTIONS);
				} else if (GUI.Button (exitButtonRect.rect, "EXIT")) {
						goToMenu (MenuSelector.EXIT);
				} else if (GUI.Button (yesButtonRect.rect, "YES")) {
						if (currMenu == MenuSelector.EXIT) 
								Application.Quit ();
						 else if (currMenu == MenuSelector.STARTNEWGAME)
								Application.LoadLevel (1);
				} else if (GUI.Button (noButtonRect.rect, "NO")) {
						if (currMenu == MenuSelector.EXIT)
								goToMenu (MenuSelector.MAIN);	
						else if (currMenu == MenuSelector.STARTNEWGAME)
								goToMenu (MenuSelector.PLAY);
				} else if (GUI.Button (previousButtonRect.rect, "BACK")) {
						goToMenu (MenuSelector.MAIN);
				} else if (GUI.Button (previousButtonRect.rect, "BACK")) {
						goToMenu (MenuSelector.MAIN);
				}
				else if (GUI.Button (continueButtonRect.rect, "CONTINUE")) {
					Application.LoadLevel (1);
				}	
				else if (GUI.Button (newGameButtonRect.rect, "NEW GAME")) {
					goToMenu (MenuSelector.STARTNEWGAME);
				}
				else if(GUI.Button (loadButtonRect.rect, "LOAD GAME")){	
				}
				//LABELS
				if (currMenu != MenuSelector.STARTNEWGAME)
						GUI.Box (doYouwantToExitLabelRect.rect, "DO YOU WANT TO EXIT?");
				if( currMenu != MenuSelector.EXIT)
						GUI.Box(doYouWantToStartNewGame.rect,"DO YOU WANT TO START NEW GAME?");
		}
}
