using UnityEngine;
using System.Collections;

public class MaskResize : MonoBehaviour
{

	void Start()
	{
        float pos = Camera.main.nearClipPlane + 0.00001f;
        transform.position = Camera.main.transform.position + Camera.main.transform.forward * pos;
        float h = Mathf.Tan(Camera.main.fieldOfView * Mathf.Deg2Rad * 0.5f) * pos * 2.0f;
        transform.localScale = new Vector3(h * Camera.main.aspect, h, 1.0f);
	}
    
}
