using UnityEngine;
using System.Collections;

public class HUD : MonoSingleton<HUD>
{
	//public textures
	public GUIStyle syfTextStyle;
	public GUIStyle timeTextStyle;
	public GUIStyle hintTextStyle;
	public GUIStyle newRoundTextStyle;
	public GUIStyle tutorialTextStyle;

	public Texture2D glassTex;  //po prostu maska od srodka
	public Texture2D healtBarTexEmpty; //szary pasek
	public Texture2D healthBarTex; // ten taki wygiety pasek (poziomo tak jak u marka) (BIALY! kolory ustawione beda w kodzie dla odpowienich stanow)
	public Texture2D timeBackgroundTex; //jakis prostokąt albo tło za czcionką czasu który będzie sie zmeiniac( chyba ze ladniej bedzie bez tla)
	public Texture2D syfTex; //textura syfu z tlem (nie wiem czy czcionka ma byc na tle czy nie (obczaj jak bedize ladniej)
	public Texture2D hintTex;
	public Texture2D syringeTex;

	private Texture2D tutorialIcon = null;

	public AudioClip newRoundSound;
	public AudioClip tutorialSound;

	bool displayTime;
	Color healthBarColor;   //aktualny kolor statusut zdrowie (cz,zolt,zielony)
	//private rects
	Rect healthbarRect; //statyczny zwykly pusty pasek
	Rect healtbarRectCurrentRect;   //zmniejacy dlugosc aktualny apse kzdrowia
	Rect timeBackgroundRect;
	Rect syfTextRect;
	Rect syfRect;   //lewy dolny rog (ilosc syfu)
	Rect timeRect;  //prway gorny rog (Czas)
	Rect syringeRect1;
	Rect syringeRect2;
	Rect syringeRect3;
	LTRect hintRect;
	LTRect tutorialRect;
	string syfAmount;
	bool hintVisible;
	bool tutorialVisible;
	string hintString;
	string tutorialString;
	float timeHintVisible;
	float timeTutorialVisible;
	//float timeHintOpacity = 1;

	private Rect newRoundTextRect;
	private int currentRoundNumber = 0;
	private bool showNewRoundNumber = false;
	private float newRoundTextTime;
	private float newRoundTextTimeDelay;
	private float newRoundTextTimeDelayEnd;
	private Color newRoundTextColor0;
	private Color newRoundTextColor1;

	//metody do wywolywania z zewnatrz. Mozna w sumie stworzyc jakies custom eventy
	public void updateSyf(int syfAmountt)
	{ 
		syfAmount = syfAmountt.ToString();
	}

	public void updateHealth(float ratioo)
	{   //parametr = currHealth/maxHealth
		healtbarRectCurrentRect = new Rect(healthbarRect);
		healtbarRectCurrentRect.width = healthbarRect.width * ratioo;
		if (ratioo >= 0.61f)
			healthBarColor = new Color(0.1f, 1, 0.2f);
		else if (ratioo >= 0.31f)
			healthBarColor = new Color(1, 1, 0.1f);
		else
			healthBarColor = new Color(1, 0, 0.1f);
	}

	//INIT
	void Start()
	{
		initView();
		LeanTween.alpha(hintRect, 0, 0.01f);
		LeanTween.alpha(tutorialRect, 0, 0.01f);
		updateSyf(100);//TO DO CHANGE
		updateHealth(1.0f);
		newRoundTextColor0 = newRoundTextStyle.normal.textColor;
		newRoundTextColor0.a = 0.0f;
		newRoundTextColor1 = newRoundTextStyle.normal.textColor;
		newRoundTextColor1.a = 1.0f;
	}

	void initView()
	{
		float h = Screen.height;
		float w = Screen.width;
		timeTextStyle.fontSize = Screen.height / 18;
		syfTextStyle.fontSize = Screen.height / 20;
		hintTextStyle.fontSize = Screen.height / 25;
		healthbarRect = new Rect(0.1f * w, 0.07f * h, 0.19f * w, 0.11f * h);
		syringeRect1 = new Rect(0.15f * w, 0.125f * h, 0.025f * w, 0.025f * w);
		syringeRect2 = new Rect(0.175f * w, 0.125f * h, 0.025f * w, 0.025f * w);
		syringeRect3 = new Rect(0.2f * w, 0.125f * h, 0.025f * w, 0.025f * w);
		syfRect = new Rect(0.14f * w, 0.83f * h, 0.23f * w, 0.11f * h);
		timeBackgroundRect = new Rect(0.74f * w, 0.07f * h, 0.13f * w, 0.055f * h);
		hintRect = new LTRect(0.5f * (w - (0.3f * w)), 0.075f * (h - (0.075f * h)), 0.3f * w, 0.075f * h);
		tutorialRect = new LTRect(0.5f * (w - (0.4f * w)), 0.075f * (h - (0.2f * h)), 0.4f * w, 0.2f * h);
		syfTextRect = syfRect;
		syfTextRect.x += 0.082f * w;
		syfTextRect.y -= 0.02f * h;
		newRoundTextRect = new Rect(0.5f * (w - (0.25f * w)), 0.5f * (h - (0.25f * h)), 0.25f * w, 0.25f * h);
	}

	public void setHintvisible(string stringToDisplay, float secondsToBeVisible)
	{
		hintString = stringToDisplay;
		timeHintVisible = secondsToBeVisible;
		if (hintRect != null)
			LeanTween.alpha(hintRect, 1, 0.5f);
		hintVisible = true;
	}

	public void setTutorialVisible(string stringToDisplay, float secondsToBeVisible)
	{
		tutorialString = stringToDisplay;
		timeTutorialVisible = secondsToBeVisible;
		if (tutorialRect != null)
			LeanTween.alpha(tutorialRect, 1, 0.5f);
		tutorialVisible = true;
		audio.PlayOneShot(tutorialSound, 1.0f);
		tutorialIcon = null;
	}

	public void setTutorialVisible(string stringToDisplay, Texture2D icon, float secondsToBeVisible)
	{
		tutorialString = stringToDisplay;
		timeTutorialVisible = secondsToBeVisible;
		if (tutorialRect != null)
			LeanTween.alpha(tutorialRect, 1, 0.5f);
		tutorialVisible = true;
		audio.PlayOneShot(tutorialSound, 1.0f);
		tutorialIcon = icon;
	}

	public void showRoundNumber(int roundNumber)
	{
		currentRoundNumber = roundNumber;
		showNewRoundNumber = true;
		newRoundTextTime = 7.0f;
		newRoundTextTimeDelay = 1.4f;
		newRoundTextTimeDelayEnd = 0.85f;
		newRoundTextStyle.normal.textColor = newRoundTextColor0;
		audio.PlayOneShot(newRoundSound, 1.0f);
	}

	void FixedUpdate()
	{
		if (hintVisible)
		{
			if (timeHintVisible > 0.0f)
				timeHintVisible -= Time.fixedDeltaTime;
			else
			{
				LeanTween.alpha(hintRect, 0, 0.5f);
				hintVisible = false;
			}
		}

		if (tutorialVisible)
		{
			if (timeTutorialVisible > 0.0f)
				timeTutorialVisible -= Time.fixedDeltaTime;
			else
			{
				LeanTween.alpha(tutorialRect, 0, 0.5f);
				tutorialVisible = false;
			}
		}

		updateHealth(PlayerStats.instance.healthPercentage);

		if (showNewRoundNumber)
		{
			if (newRoundTextTimeDelay > 0.0f)
			{
				newRoundTextTimeDelay -= Time.fixedDeltaTime;
			}
			else
			{
				newRoundTextTime -= Time.fixedDeltaTime; 
				newRoundTextStyle.normal.textColor = Color.Lerp(newRoundTextStyle.normal.textColor, newRoundTextColor1, Time.fixedDeltaTime);
			}
			if (newRoundTextTime <= 0.0f)
			{
				newRoundTextTimeDelayEnd -= Time.fixedDeltaTime;
				newRoundTextStyle.normal.textColor = Color.Lerp(newRoundTextStyle.normal.textColor, newRoundTextColor0, Time.fixedDeltaTime * 4.0f);
				if (newRoundTextTimeDelayEnd <= 0.0f)
				{
					showNewRoundNumber = false;
				}
			}
		}
	}

	//UI    
	void OnGUI()
	{
		GUI.DrawTexture(syfRect, syfTex);
		GUI.Label(syfTextRect, syfAmount, syfTextStyle);
		GUI.DrawTexture(timeBackgroundRect, timeBackgroundTex);
		GUI.Label(timeBackgroundRect, GameMaster.instance.TimeToDayEnd, timeTextStyle);
		GUI.DrawTexture(healthbarRect, healtBarTexEmpty);

		if (PlayerStats.instance.syringes > 0)
		{
			GUI.DrawTexture(syringeRect1, syringeTex);
		}
		if (PlayerStats.instance.syringes > 1)
		{
			GUI.DrawTexture(syringeRect2, syringeTex);
		}
		if (PlayerStats.instance.syringes > 2)
		{
			GUI.DrawTexture(syringeRect3, syringeTex);
		}

		GUI.DrawTexture(hintRect.rect, hintTex);
		GUI.Label(hintRect.rect, hintString, hintTextStyle);

		GUI.DrawTexture(tutorialRect.rect, hintTex);
		if (tutorialIcon != null)
		{
			GUI.Label(tutorialRect.rect, new GUIContent(tutorialString, tutorialIcon), tutorialTextStyle);
		}
		else
		{
			GUI.Label(tutorialRect.rect, tutorialString, tutorialTextStyle);
		}

		GUI.color = healthBarColor;
		GUI.BeginGroup(healtbarRectCurrentRect);
		GUI.DrawTexture(new Rect(0, 0, healthbarRect.width, healthbarRect.height), healthBarTex);
		GUI.EndGroup();

		if (showNewRoundNumber)
		{
			GUI.Label(newRoundTextRect, currentRoundNumber.ToString(), newRoundTextStyle);
		}
	}
}