using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class HelmetGlassEffects : MonoBehaviour {
	public Camera helmetEffectCamera;
	public Shader refractionShader;
	public Shader blurShader;
	public Texture steamTexture;
	public Texture noise;
	[Range(0, 1)]
	public float steaminess;
	[Range(0, 3)]
	public int downSample;
	[Range(1, 5)]
	public int blurIteratons;
	[Range(0.001f, 0.1f)]
	public float blurSize;
	[Range(0.0001f, 0.005f)]
	public float refractionSampleSize;
	[Range(-0.001f, 0.001f)]
	public float refractionMagnitude;
	
	private Material _refractionMat;
	private Material refractionMat{
		get{
			if(_refractionMat == null){
				_refractionMat = new Material(refractionShader);
				refractionMat.SetTexture("_Glass", helmetEffectCamera.targetTexture);
				refractionMat.SetTexture("_Steam", steamTexture);
			}
			return _refractionMat;
		}
	}

	private Material _blurMat;
	private Material blurMat{
		get{
			if (_blurMat == null) {
				_blurMat = new Material(blurShader);
				_blurMat.SetTexture("_Steam", steamTexture);
			}
			return _blurMat;
		}
	}

	void Init() {
		if (helmetEffectCamera.targetTexture == null) {
			helmetEffectCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
		}
	}

	void Release() {
		if (refractionMat != null) {
			DestroyImmediate(_refractionMat);
		}
		if (blurMat != null) {
			DestroyImmediate(_blurMat);
		}
		if (helmetEffectCamera != null && helmetEffectCamera.targetTexture != null) {
			DestroyImmediate(helmetEffectCamera.targetTexture);
			helmetEffectCamera.targetTexture = null;
		}
	}
	
	void OnRenderImage(RenderTexture source, RenderTexture destination) {
		Init();
		int actualWidth = Screen.width >> downSample;
		int actualHeight = Screen.height >> downSample;


		float size = blurSize;
		RenderTexture blurred1 = RenderTexture.GetTemporary(actualWidth, actualHeight, 0, source.format);
		RenderTexture blurred2 = RenderTexture.GetTemporary(actualWidth, actualHeight, 0, source.format);

		float aspect = (float) Screen.height / (float) Screen.width;
		blurMat.SetFloat("_Size", size);
		blurMat.SetFloat("_Steaminess", steaminess);
		Graphics.Blit(source, blurred1, blurMat, 0);
		Graphics.Blit(blurred1, blurred2, blurMat, 1);
		for (int i = 1; i < blurIteratons; ++i) {
			size *= Mathf.Pow(2, -1.0f / (blurIteratons - 1));
			blurMat.SetFloat("_Size", size);
			Graphics.Blit(blurred1, blurred2, blurMat, 0);
			blurMat.SetFloat("_Size", size / aspect);
			Graphics.Blit(blurred2, blurred1, blurMat, 1);
		}
		refractionMat.SetTexture("_Blurred", blurred1);

		refractionMat.SetFloat("_Delta", refractionSampleSize);
		refractionMat.SetFloat("_Magnitude", refractionMagnitude);
		refractionMat.SetFloat("_Steaminess", steaminess);

		RenderTexture.ReleaseTemporary(blurred2);
		Graphics.Blit(source, destination, refractionMat);
		RenderTexture.ReleaseTemporary(blurred1);
	}

	void OnDisable() {
		Release();
	}
}
