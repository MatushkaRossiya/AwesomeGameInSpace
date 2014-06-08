using UnityEngine;
using System.Collections;

public class HelmetGlassEffects : MonoBehaviour {
	public Material postProcessMaterial;
	public Camera helmetEffectCamera;

	void Start() {
		RenderTexture tex = new RenderTexture(Screen.width, Screen.height, 24);
		helmetEffectCamera.targetTexture = tex;
		postProcessMaterial.SetTexture("_Glass", tex); 
	}
	
	void OnRenderImage(RenderTexture source, RenderTexture destination) {

		//mat is the material containing your shader
		Graphics.Blit(source, destination, postProcessMaterial);
	}
}
