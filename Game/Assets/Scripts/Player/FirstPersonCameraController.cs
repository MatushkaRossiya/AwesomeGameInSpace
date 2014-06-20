using UnityEngine;
using System.Collections;
using System.Reflection;

public class FirstPersonCameraController : MonoSingleton<FirstPersonCameraController>
{
	public float mouseSensitivity = 1.0f;
	public AnimationCurve speedCurve;
	public float gamepadSensitivity = 3.0f;
	private float effectiveSensitivity = 1.0f;
	private bool gamepadAimAssist = false;
	private bool prevGamepadAimAssist = false;
	private float gamepadAimAssistDelay = 0.5f;
	private float gamepadAimAssistTimeLeft = 0;
	private bool isZoomed = false;
	private Component dof;
	private PropertyInfo dofEnable;
	private float _horizontalAngle;

	public float horizontalAngle
	{
		get
		{
			return _horizontalAngle;
		}
		set
		{
			_horizontalAngle = Mathf.Repeat(value, 360);
		}
	}

	private float _verticalAngle;

	public float verticalAngle
	{
		get
		{
			return _verticalAngle;
		}
		set
		{
			_verticalAngle = Mathf.Clamp(value, -90, 90);
			transform.localRotation = Quaternion.Euler(new Vector3(-_verticalAngle, 0, 0));
		}
	}

	void Start()
	{
		camera.depthTextureMode = DepthTextureMode.Depth;
		dof = GetComponent("DepthOfFieldScatter");
		dofEnable = dof.GetType().GetProperty("enabled");
		dofEnable.SetValue(dof, false, null);
	}

	void Update()
	{
		if (Gamepad.instance.isConnected())
		{
			isZoomed = Gamepad.instance.leftTrigger() > 0.75f;
		}
		else if (Input.GetMouseButtonDown(1))
		{
			isZoomed = !isZoomed;
		}

		LaserRifle.instance.isZoomed = isZoomed;
		PlayerController.instance.isZoomed = isZoomed;

		if (isZoomed)
		{
			Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 25.0f, Time.deltaTime * 4.0f);
			dofEnable.SetValue(dof, true, null);
		}
		else
		{
			Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 55.0f, Time.deltaTime * 4.0f);
			dofEnable.SetValue(dof, false, null);
		}

#if UNITY_EDITOR
        if (!Input.GetKey(KeyCode.Escape))
        {
#endif
		// lock and hide cursor
		Screen.lockCursor = true;
		gamepadAimAssist = false;
		if (gamepadAimAssistTimeLeft < 0.0f)
		{
			gamepadAimAssistTimeLeft = 0.0f;
		}

		if (Gamepad.instance.isConnected())
		{
			RaycastHit hitInfo;
			Vector3 start = Camera.main.transform.position;
			Vector3 dir = Camera.main.transform.forward;

			bool hit = Physics.SphereCast(start, 0.1f, dir, out hitInfo, 20.0f);

			if (hit)
			{
				Alien ai = (Alien)hitInfo.collider.transform.root.GetComponent<Alien>();
                    
				if (ai != null)
				{
					if (!ai.isDead)
					{
						gamepadAimAssist = true;
					}
				}
			}
		}

		if (prevGamepadAimAssist != gamepadAimAssist)
		{
			prevGamepadAimAssist = gamepadAimAssist;
			gamepadAimAssistTimeLeft = gamepadAimAssistDelay - gamepadAimAssistTimeLeft;
		}

		float T = gamepadAimAssistTimeLeft / gamepadAimAssistDelay;

		if (gamepadAimAssist)
		{
			effectiveSensitivity = MathfX.sinerp(0.25f, 1.0f, T);
		}
		else
		{
			T = 1.0f - T;
			effectiveSensitivity = MathfX.sinerp(0.25f, 1.0f, T);
		}

		gamepadAimAssistTimeLeft -= Time.deltaTime;

		if (isZoomed)
		{
			effectiveSensitivity *= 0.75f;
		}

		// read input
		Vector2 input;
		input.x = Input.GetAxisRaw("Mouse X") * mouseSensitivity + Gamepad.instance.rightStick().x * gamepadSensitivity;
		input.y = Input.GetAxisRaw("Mouse Y") * mouseSensitivity + Gamepad.instance.rightStick().y * gamepadSensitivity;

		if (input != Vector2.zero)
		{
			float speed = input.magnitude / Time.deltaTime;
			speed *= speedCurve.Evaluate(speed);
			input = Time.deltaTime * speed * effectiveSensitivity * input.normalized;
			input *= effectiveSensitivity;
			if (!float.IsNaN(input.x) && !float.IsNaN(input.y))
			{
				horizontalAngle += input.x;
				verticalAngle += input.y;
			}
		}
            
#if UNITY_EDITOR
        }
        else
        {
            Screen.lockCursor = false;
        }
#endif
	}
}
