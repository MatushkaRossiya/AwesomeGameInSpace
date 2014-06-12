using UnityEngine;
using System.Collections;

public class AlarmLight : MonoBehaviour
{
	
	void Start()
	{
	
	}

	void FixedUpdate()
	{
		transform.Rotate(0.0f, 45.0f * Time.fixedDeltaTime, 0.0f);
	}
}
