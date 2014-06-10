using UnityEngine;
using System.Collections;

public class MenuScene : MonoBehaviour {

	GameObject kamera;
	bool left;
	public float cameraRotationSpeed;
	// Use this for initialization
	void Start () {
		cameraRotationSpeed=3.0f;
		left = false;
		kamera = GameObject.FindObjectOfType<Camera> ().gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (left && kamera.transform.rotation.y > -0.94)
		{

			kamera.transform.RotateAround (kamera.transform.position, transform.up, -Time.deltaTime*cameraRotationSpeed);
		}
		else if(left==false && kamera.transform.rotation.y<-0.4f)
		{

			kamera.transform.RotateAround (kamera.transform.position, transform.up, Time.deltaTime*cameraRotationSpeed);
		}
		else
		{
			left = !left;
		}

	}
}
