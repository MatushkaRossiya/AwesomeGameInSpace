using UnityEngine;
using System.Collections;

public class FirstPersonCameraController : MonoBehaviour {
    [HideInInspector]
    public float horizontalAngle;
    [HideInInspector]
    public float verticalAngle;
    public float acceleration = 1.0f;

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
            //horizontalAngle += Input.GetAxis("Mouse X") * mouseSensitivity;
            //verticalAngle += -Input.GetAxis("Mouse Y") * mouseSensitivity;

            Vector2 input;
            input.x = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
            input.y = -Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
            //input /= Screen.height;

            if (input != Vector2.zero) {

                float speed = input.magnitude / Time.deltaTime;
                float multiplier = Mathf.Pow(speed, acceleration);

                input = input.normalized * (multiplier * Time.deltaTime);

                horizontalAngle += input.x;
                verticalAngle += input.y;

                // normalize to proper ranges
                if (horizontalAngle > 180) horizontalAngle -= 360;
                else if (horizontalAngle < -180) horizontalAngle += 360;
                if (verticalAngle > 90) verticalAngle = 90;
                else if (verticalAngle < -90) verticalAngle = -90;

                // apply only vertical component
                // horizontal is applied in PlayerController because it makes walking easier
                transform.localRotation = Quaternion.Euler(new Vector3(verticalAngle, 0, 0));
            }
        } else {
            Screen.lockCursor = false;
        }
	}
}
