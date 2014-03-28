using UnityEngine;
using System.Collections;

public class FirstPersonCameraController : MonoBehaviour {
    public float horizontalAngle;
    public float verticalAngle;

    [Range(0.1f, 2.0f)]
    public float mouseSensitivity = 1.0f;

	// Update is called once per frame
    void Update()
    {
        horizontalAngle += Input.GetAxis("Mouse X") * mouseSensitivity;
        verticalAngle += -Input.GetAxis("Mouse Y") * mouseSensitivity;

        if (horizontalAngle > 180) horizontalAngle -= 360;
        else if (horizontalAngle < -180) horizontalAngle += 360;
        if (verticalAngle > 90) verticalAngle = 90;
        else if (verticalAngle < -90) verticalAngle = -90;
        transform.localRotation = Quaternion.Euler(new Vector3(verticalAngle, 0, 0));
	}
}
