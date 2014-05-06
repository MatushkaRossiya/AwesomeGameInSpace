using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class FirstPersonCameraController : MonoSingleton<FirstPersonCameraController> {

	bool playerIndexSet = false;
	PlayerIndex playerIndex;
	GamePadState state;
	GamePadState prevState;

	private float _horizontalAngle;
	public float horizontalAngle {
		get {
			return _horizontalAngle;
		}
		set {
			_horizontalAngle = Mathf.Repeat(value, 360);
		}
	}

	private float _verticalAngle; 
	public float verticalAngle {
		get {
			return _verticalAngle;
		}
		set {
			_verticalAngle = Mathf.Clamp(value, -90, 90);
			transform.localRotation = Quaternion.Euler(new Vector3(-_verticalAngle, 0, 0));
		}
	}
    public float acceleration = 1.0f;

    public float mouseSensitivity = 1.0f;

	// Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (!Input.GetKey(KeyCode.Escape)) {
#endif
			if (!playerIndexSet || !prevState.IsConnected)
			{
				for (int i = 0; i < 4; ++i)
				{
					PlayerIndex testPlayerIndex = (PlayerIndex)i;
					GamePadState testState = GamePad.GetState(testPlayerIndex);
					if (testState.IsConnected)
					{
						Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
						playerIndex = testPlayerIndex;
						playerIndexSet = true;
					}
				}
			}
			
			prevState = state;
			state = GamePad.GetState(playerIndex);

            // lock and hide cursor
            Screen.lockCursor = true;

            // read input
            Vector2 input;
			input.x = Input.GetAxisRaw("Mouse X") * mouseSensitivity + state.ThumbSticks.Right.X;
			input.y = Input.GetAxisRaw("Mouse Y") * mouseSensitivity + state.ThumbSticks.Right.Y;
            //input /= Screen.height;

            if (input != Vector2.zero) {

                float speed = input.magnitude / Time.deltaTime;
                float multiplier = Mathf.Pow(speed, acceleration);

                input = input.normalized * (multiplier * Time.deltaTime);

                horizontalAngle += input.x;
                verticalAngle += input.y;
			}
			
#if UNITY_EDITOR
        } else {
            Screen.lockCursor = false;
        }
#endif
	}
}
