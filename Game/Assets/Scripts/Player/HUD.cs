using UnityEngine;
using System.Collections;



public class HUD : MonoBehaviour {
	//public textures
	public GUIStyle syfTextStyle;
	public GUIStyle timeTextStyle;
	public GUIStyle hintTextStyle;
	public Texture2D glassTex;	//po prostu maska od srodka
	public Texture2D healtBarTexEmpty; //szary pasek
	public Texture2D healthBarTex; // ten taki wygiety pasek (poziomo tak jak u marka) (BIALY! kolory ustawione beda w kodzie dla odpowienich stanow)
	public Texture2D timeBackgroundTex; //jakis prostokąt albo tło za czcionką czasu który będzie sie zmeiniac( chyba ze ladniej bedzie bez tla)
	public Texture2D syfTex; //textura syfu z tlem (nie wiem czy czcionka ma byc na tle czy nie (obczaj jak bedize ladniej)
	public Texture2D hintTex;
	
	bool displayTime;
	Color healthBarColor;	//aktualny kolor statusut zdrowie (cz,zolt,zielony)
	//private rects
	Rect healthbarRect;	//statyczny zwykly pusty pasek
	Rect healtbarRectCurrentRect;	//zmniejacy dlugosc aktualny apse kzdrowia
	Rect glassRect;
	Rect timeBackgroundRect;
	Rect syfTextRect;
	Rect syfRect;	//lewy dolny rog (ilosc syfu)
	Rect timeRect;	//prway gorny rog (Czas)
	Rect hintRect;
	string syfAmount;	
	bool hintVisible;
	string hintString;
	float timeHintVisible;
	float timeHintOpacity=1;

	//metody do wywolywania z zewnatrz. Mozna w sumie stworzyc jakies custom eventy
	public void updateSyf(int syfAmountt){ 
		syfAmount = syfAmountt.ToString();
	}
	public void updateHealth(float ratioo){	//parametr = currHealth/maxHealth
		healtbarRectCurrentRect = new Rect (healthbarRect);
		healtbarRectCurrentRect.width = healthbarRect.width * ratioo;
		if (ratioo >= 0.61f) healthBarColor = new Color (0.1f,1, 0.2f);
		else if (ratioo >= 0.31f) healthBarColor = new Color (1,1, 0.1f);
		else healthBarColor = new Color (1,0,0.1f);
	}

	//INIT
	void Start(){
		initView ();
		updateSyf (10);
		updateHealth (1.0f);
	}
	void initView(){
		float h = Screen.height;
		float w = Screen.width;
		timeTextStyle.fontSize = Screen.height / 18;
		syfTextStyle.fontSize = Screen.height / 20;
		hintTextStyle.fontSize = Screen.height / 17;
		healthbarRect = new Rect (0.1f * w, 0.07f * h, 0.19f * w, 0.11f * h);
		glassRect = 			new Rect (0, 0, w, h);
		syfRect = 				new Rect (0.14f * w, 0.83f * h, 0.23f * w, 0.11f * h);
		timeBackgroundRect =    new Rect (0.74f * w, 0.07f * h, 0.13f * w, 0.055f * h);
		hintRect =  			new Rect (0.306f * w, 0.388f * h, 0.383f * w, 0.105f * h);
		syfTextRect = syfRect;
		syfTextRect.x += 0.075f * w;
		syfTextRect.y -= 0.02f * h;
	}
	public void setHintvisible(string stringToDisplay,float secondsToBeVisible){
		hintString = stringToDisplay;
		timeHintVisible = secondsToBeVisible;
		hintVisible = true;
	}
	void Update(){
				if (hintVisible) {
						Debug.Log (timeHintVisible.ToString ());
						if (timeHintVisible > Time.deltaTime)
								timeHintVisible -= Time.deltaTime;
						else
								hintVisible = false;
				}
	}
	//UI	
	void OnGUI() {
		//GUI.Label(new Rect(10, 10, 200, 50), "Stuff count: " + DataBank.stuffCount,textStyle);
		GUI.DrawTexture (glassRect, glassTex);
		GUI.DrawTexture (syfRect, syfTex);
		GUI.Label (syfTextRect,syfAmount, syfTextStyle);
		GUI.DrawTexture (timeBackgroundRect, timeBackgroundTex);
		GUI.Label (timeBackgroundRect, GameMaster.instance.getTimeToDayEnd(),timeTextStyle);
		GUI.DrawTexture (healthbarRect, healtBarTexEmpty);
		if (hintVisible) {
			GUI.DrawTexture (hintRect, hintTex);
			GUI.Label (hintRect, hintString, hintTextStyle);
		}
		GUI.color = healthBarColor;
		GUI.BeginGroup (healtbarRectCurrentRect);
		GUI.DrawTexture (new Rect (0, 0, healthbarRect.width, healthbarRect.height), healthBarTex);
		GUI.EndGroup ();
	}
}