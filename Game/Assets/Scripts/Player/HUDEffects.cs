using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class HUDEffects : MonoSingleton<HUDEffects> {

	//textures
	public Texture2D blood1;
	public Texture2D blood2;
	public Texture2D blood3;
	public Texture2D crack1;
	public Texture2D crack2;
	public Texture2D crack3;
	//rects
	public LTRect bloodRect1;
	public LTRect bloodRect2;
	public LTRect bloodRect3;
	public LTRect crackRect1;
	public LTRect crackRect2;
	public LTRect crackRect3;

	private Dictionary<Texture2D,LTRect> effects;
	private Dictionary<LTRect,bool> activeEffects;
	// Use this for initialization
	void Start () 
	{
		setupRects ();
		effects = new Dictionary<Texture2D, LTRect> ();
		effects.Add (blood1, bloodRect1);
		effects.Add (blood2, bloodRect2);
		effects.Add (blood3, bloodRect3);
		effects.Add (crack1, crackRect1);
		effects.Add (crack2, crackRect2);
		effects.Add (crack3, crackRect3);
		foreach (var rect in effects.Values) {
			LeanTween.alpha(rect,0,0.01f);
		}
	}
	void setupRects()
	{
		float varrx = Random.Range (-Screen.width * 0.2f, Screen.width * 0.2f);
		float varry = Random.Range (-Screen.height * 0.2f, Screen.height * 0.2f);
		bloodRect1 = new LTRect(Screen.width/15+varrx,Screen.height/2+varry,Screen.width*0.52f,Screen.height*0.61f);
		bloodRect2 = new LTRect(Screen.width/2+varrx,Screen.height/2+varry,Screen.width*0.28f,Screen.height*0.61f);
		bloodRect3 = new LTRect(varrx,varry,Screen.width*0.33f,Screen.height*0.48f);
		crackRect1 = new LTRect(0,0,Screen.width,Screen.height);
		crackRect2 = new LTRect(0,0,Screen.width,Screen.height);
		crackRect3 = new LTRect(0,0,Screen.width,Screen.height);
	}
	// Update is called once per frame
	void Update () 
	{
	
	}
	void OnGUI()
	{
		foreach (var effect in effects) {
			GUI.DrawTexture(effect.Value.rect,effect.Key);
		}
	}
	public void showEffect(float damage)
	{
		int randomElement = Random.Range (0, effects.Count);
		List <LTRect> lizt = new List<LTRect>(effects.Values);
		LTRect losowy;
		bool istniejeniewyswietlony=false;
		foreach (var rect in effects.Values) 
		{
			if(rect.alpha==0) istniejeniewyswietlony=true;
		}
		if (!istniejeniewyswietlony) return;
		do
		losowy = lizt [Random.Range (0, lizt.Count)];
		while (losowy.alpha > 0);
		StartCoroutine(startShowEffect(losowy));
	}
	IEnumerator startShowEffect(LTRect effect)
	{
		LeanTween.alpha (effect, 1, 0.2f);
		yield return new WaitForSeconds(2);
		LeanTween.alpha (effect, 0, 6);
	}
}
