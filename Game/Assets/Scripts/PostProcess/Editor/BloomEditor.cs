using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Bloom))]
public class BloomEditor : Editor
{
	SerializedObject serObj;
	SerializedProperty bloomIntensity;
	SerializedProperty lensDirtIntensity;
	SerializedProperty lensDirtTexture;

	void OnEnable()
	{
		serObj = new SerializedObject(target);
		bloomIntensity = serObj.FindProperty("bloomIntensity");
		lensDirtIntensity = serObj.FindProperty("lensDirtIntensity");
		lensDirtTexture = serObj.FindProperty("lensDirtTexture");
	}

	public override void OnInspectorGUI()
	{
		serObj.Update();

		Bloom instance = (Bloom)target;

		if (!instance.inputIsHDR)
		{
			EditorGUILayout.HelpBox("Brak HDR!", MessageType.Warning);
		}

		EditorGUILayout.PropertyField(bloomIntensity, new GUIContent("Bloom Intensity", ""));
		EditorGUILayout.PropertyField(lensDirtIntensity, new GUIContent("Lens Dirt Intensity", ""));
		EditorGUILayout.PropertyField(lensDirtTexture, new GUIContent("Lens Dirt Texture", ""));

		serObj.ApplyModifiedProperties();
	}
}
