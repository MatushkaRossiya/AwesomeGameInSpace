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
        // debug testing for esc
        // TODO remove in final release
        if (!Input.GetKey(KeyCode.Escape)) {
            // lock and hide cursor
            Screen.lockCursor = true;

            // read input
            horizontalAngle += Input.GetAxis("Mouse X") * mouseSensitivity;
            verticalAngle += -Input.GetAxis("Mouse Y") * mouseSensitivity;

            // normalize to proper ranges
            if (horizontalAngle > 180) horizontalAngle -= 360;
            else if (horizontalAngle < -180) horizontalAngle += 360;
            if (verticalAngle > 90) verticalAngle = 90;
            else if (verticalAngle < -90) verticalAngle = -90;

            // apply only vertical component
            // horizontal is applied in PlayerController because it makes walking easier
            transform.localRotation = Quaternion.Euler(new Vector3(verticalAngle, 0, 0));
        } else {
            Screen.lockCursor = false;
        }
	}
}
