using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float playerAcceleration = 10.0f;
    public float playerSpeed = 1.0f;
    public float jumpHeight = 1.0f;

    private FirstPersonCameraController firstPersonCameraController;

    private bool isTouchingGround = false;

    void Start()
    {
        firstPersonCameraController = transform.GetComponentInChildren<FirstPersonCameraController>();
    }

    void FixedUpdate()
    {
        transform.localRotation = Quaternion.Euler(new Vector3(0, firstPersonCameraController.horizontalAngle, 0));
        Vector3 input = new Vector3();
        input += transform.forward * Input.GetAxis("Vertical");
        input += transform.right * Input.GetAxis("Horizontal");
        if (input.sqrMagnitude > 1.0f) input.Normalize();

        Vector3 relativeDeltaVelocity = input - rigidbody.velocity / playerSpeed;
        relativeDeltaVelocity.y = 0;
        Vector3 acceleration = relativeDeltaVelocity.normalized * Mathf.Pow(relativeDeltaVelocity.magnitude, 0.1f) * playerAcceleration;

        rigidbody.AddForce(acceleration, ForceMode.Acceleration);

        if (isTouchingGround && Input.GetAxis("Jump") > 0.5f)
        {
            rigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.VelocityChange);
        }
        isTouchingGround = false;
    }

    void OnTriggerEnter(Collider other)
    {
        isTouchingGround = true;
    }

    void OnTriggerStay(Collider other)
    {
        isTouchingGround = true;
    }
}
