using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	enum MenuSelector {
		PLAY,MAIN,OPTIONS,EXIT
	}
	float ScrHeight = Screen.height;
	float ScrWidth = Screen.width;
	float width = 0.3f*Screen.width;
	float height = 0.2f*Screen.height;
	//main menu rects
	Rect startButtonRect;
	Rect optionsButtonRect;
	Rect exitButtonRect;
	Rect previousButtonRect;
	//exit rects
	Rect yesButtonRect;
	Rect noButtonRect;
	Rect doYouwantToExitRect;


	Rect guiTextureRect;
	public Texture2D backgroundIMG;
	MenuSelector currentMenu;


	void Start () {
		updateView ();
		currentMenu = MenuSelector.MAIN;
	}

	void Update () {
		if (Screen.height != ScrHeight || Screen.width != ScrWidth)
			updateView ();	//W przypadku gdyby nastapila zmiana rozdzielczosci
	}

	void updateView()
	{
		float verticalButtonsPos = Screen.height / 2 - 0.5f * height;
		float verticalOffset = 1.3f * height;
		float horizontallOffset = 1.3f * width;
		float horizontalButtonPos = Screen.width / 2 - 0.5f * width;
		guiTextureRect = new Rect (0, 0, Screen.width, Screen.height);
		startButtonRect   = new Rect (horizontalButtonPos,verticalButtonsPos-verticalOffset, width, height);
		optionsButtonRect = new Rect (horizontalButtonPos,verticalButtonsPos, width, height);
		exitButtonRect    = new Rect (horizontalButtonPos,verticalButtonsPos+verticalOffset, width, height);
		yesButtonRect = new Rect (horizontalButtonPos-0.5f*horizontallOffset,verticalButtonsPos,width,height);
		noButtonRect = new Rect (horizontalButtonPos+0.5f*horizontallOffset,verticalButtonsPos,width,height);
		doYouwantToExitRect = new Rect (horizontalButtonPos, verticalButtonsPos-0.4f*verticalOffset, width, 0.4f*height);
		previousButtonRect = new Rect (horizontalButtonPos - 0.8f*horizontallOffset, verticalButtonsPos + verticalOffset, width, height);
	}

	void OnGUI()
	{
		GUI.DrawTexture (guiTextureRect, backgroundIMG, ScaleMode.ScaleAndCrop);
		switch (currentMenu) {
			case MenuSelector.MAIN:
				if (GUI.Button (startButtonRect, "PLAY")) 	   currentMenu = MenuSelector.PLAY;		
				if (GUI.Button (optionsButtonRect, "OPTIONS")) currentMenu = MenuSelector.OPTIONS;
				if (GUI.Button (exitButtonRect, "EXIT"))	   currentMenu = MenuSelector.EXIT;
				break;
			case MenuSelector.EXIT:
				if(GUI.Button(yesButtonRect,"YES"))					Application.Quit ();
				if(GUI.Button(noButtonRect,"NO"))					currentMenu = MenuSelector.MAIN;	
				GUI.Box(doYouwantToExitRect,"DO YOU WANT TO EXIT");
				break;
			case MenuSelector.PLAY:
				if (GUI.Button (previousButtonRect, "BACK"))   		currentMenu = MenuSelector.MAIN;
				break;
			case MenuSelector.OPTIONS:
				if (GUI.Button (previousButtonRect, "BACK"))   		currentMenu = MenuSelector.MAIN;
				break;
			}	
	}
}
