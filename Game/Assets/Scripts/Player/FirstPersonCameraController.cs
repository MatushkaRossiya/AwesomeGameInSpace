using UnityEngine;
using System.Collections;

public class FirstPersonCameraController : MonoSingleton<FirstPersonCameraController> {
	public float mouseSensitivity = 1.0f;
	public AnimationCurve speedCurve;
	public float gamepadSensitivity = 3.0f;

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

    private float effectiveSensitivity;
    private bool isZoomed = false;


    void Start()
    {
		camera.depthTextureMode = DepthTextureMode.Depth;
    }

	void Update() {
		if (Gamepad.instance.isConnected()) {
			isZoomed = Gamepad.instance.leftTrigger() > 0.25f;
		}else if (Input.GetMouseButtonDown(1)) {
			isZoomed = !isZoomed;
		}

		LaserRifle.instance.isZoomed = isZoomed;
		PlayerController.instance.isZoomed = isZoomed;

		if (isZoomed) {
			Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 25.0f, Time.deltaTime * 4.0f);
		}
		else {
			Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 55.0f, Time.deltaTime * 4.0f);
		}
	}
    
    void FixedUpdate()
    {
#if UNITY_EDITOR
        if (!Input.GetKey(KeyCode.Escape))
        {
#endif
            // lock and hide cursor
            Screen.lockCursor = true;

			effectiveSensitivity = 1;

            if(Gamepad.instance.isConnected())
            {
                RaycastHit hitInfo;
                Vector3 start = Camera.main.transform.position;
                Vector3 dir = Camera.main.transform.forward;

                bool hit = Physics.SphereCast(start, 0.1f, dir, out hitInfo, 20.0f, Layers.enemy);

                if (hit)
                {
                    Alien ai = (Alien)hitInfo.collider.transform.root.GetComponent<Alien>();
                    
                    if (ai != null)
                    {
                        if (!ai.isDead) effectiveSensitivity *= 0.1f;
                    }
                }
            }

			if (isZoomed) effectiveSensitivity *= 0.75f;

            // read input
            Vector2 input;
            input.x = Input.GetAxisRaw("Mouse X") * mouseSensitivity + Gamepad.instance.rightStick().x * gamepadSensitivity;
            input.y = Input.GetAxisRaw("Mouse Y") * mouseSensitivity + Gamepad.instance.rightStick().y * gamepadSensitivity;

			if (input != Vector2.zero) {
				float speed = input.magnitude / Time.fixedDeltaTime;
				speed *= speedCurve.Evaluate(speed);
				input = Time.fixedDeltaTime * speed * effectiveSensitivity * input.normalized;
				input *= effectiveSensitivity;
				horizontalAngle += input.x;
				verticalAngle += input.y;
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
