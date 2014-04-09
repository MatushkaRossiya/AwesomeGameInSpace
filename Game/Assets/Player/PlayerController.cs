using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float acceleration = 10.0f;
    public float walkSpeed = 10.0f;
    public float sprintSpeed = 20.0f;
    public float jumpHeight = 1.0f;

    private FirstPersonCameraController firstPersonCameraController;

    private bool isTouchingGround = false;

    void Start()
    {
        firstPersonCameraController = transform.GetComponentInChildren<FirstPersonCameraController>();
    }

    void FixedUpdate()
    {
        Walk();
        Jump();
    }

    void Walk() {
        // apply mouse movement
        transform.localRotation = Quaternion.Euler(new Vector3(0, firstPersonCameraController.horizontalAngle, 0));

        // read input
        Vector3 input = new Vector3();
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        // normalize if needed to avoid going faster diagonally
        if (input.sqrMagnitude > 1.0f) input.Normalize();

        // check if sprinting
        if (Input.GetAxis("Sprint") > 0.5f) {
            Debug.Log("Sprint");
            input.y = sprintSpeed / walkSpeed;
            input.x *= 0.5f;
        }

        // calculate desired velocity
        input = transform.forward * input.y + transform.right * input.x;

        // calculate difference between desired velocity and current velocity
        Vector3 deltaVelocity = input * walkSpeed - rigidbody.velocity;
        Vector3 relativeDeltaVelocity = deltaVelocity / walkSpeed;
        // ignore vertical component (e.i. falling and jumping
        relativeDeltaVelocity.y = 0;

        // calculate final acceleration
        Vector3 acc = relativeDeltaVelocity.normalized * Mathf.Pow(relativeDeltaVelocity.magnitude, 0.1f) * acceleration;

        float maxAcceleration = deltaVelocity.magnitude / Time.fixedDeltaTime;

        if (acc.magnitude > maxAcceleration) {
            acc = acc.normalized * maxAcceleration;
        }

        // apply acceleration
        rigidbody.AddForce(acc, ForceMode.Acceleration);
    }

    void Jump() {
        // check if jump button is pressed
        if (isTouchingGround && Input.GetAxis("Jump") > 0.5f) {
            // jump
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
