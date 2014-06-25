using UnityEngine;
using System.Collections;

public class CowSpin : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler (new Vector3 (0.0f, this.transform.rotation.y + 0.01f, 0.0f));
	}
}
