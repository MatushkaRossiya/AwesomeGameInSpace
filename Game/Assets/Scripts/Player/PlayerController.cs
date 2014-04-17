using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
    public float acceleration = 10.0f;
    public float walkSpeed = 10.0f;
    public float sprintSpeed = 20.0f;
    public float jumpHeight = 1.0f;

    public float standingViewHeight = 0.75f;
    public float crouchingViewHeight = -0.25f;
    public float crouchingDelay = 0.3f;
    public float stepLength;

    public List<AudioClip> walkOnMetalSounds;

    private FirstPersonCameraController firstPersonCameraController;

    private bool isTouchingGround = false;
    private bool isCrouching = false;
    private float crouchingTimeLeft = 0;
    private float distance = 0.0f;


    CapsuleCollider playerCollider;

    void Start()
    {
        firstPersonCameraController = transform.GetComponentInChildren<FirstPersonCameraController>();
        playerCollider = GetComponent<CapsuleCollider>();
    }

    void FixedUpdate()
    {
        Crouch();
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

        bool sprinting = false;

        // check if sprinting
        if (isCrouching) {
            input *= 0.5f;
        }else if (Input.GetAxis("Sprint") > 0.5f) {
            input.y = sprintSpeed / walkSpeed;
            input.x *= 0.5f;
            sprinting = true;
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

        if (isTouchingGround) {
            distance += Mathf.Sqrt(rigidbody.velocity.magnitude) * Time.fixedDeltaTime;
            if (distance > stepLength) {
                distance = 0.0f;
                audio.PlayOneShot(walkOnMetalSounds[Random.Range(0, walkOnMetalSounds.Count)]);
            }
        } else {
            distance = 0.0f;
        }
    }

    void Jump() {
        // check if jump button is pressed
        if (isTouchingGround && Input.GetAxis("Jump") > 0.5f) {
            // jump
            rigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.VelocityChange);
        }

        isTouchingGround = false;
    }


    void Crouch() {
        if (crouchingTimeLeft < 0.0f) crouchingTimeLeft = 0.0f;

        if (!isCrouching && Input.GetKey(KeyCode.LeftControl)) {
            isCrouching = true;
            crouchingTimeLeft = crouchingDelay - crouchingTimeLeft;
        } else if (isCrouching && !Input.GetKey(KeyCode.LeftControl) && !Physics.Raycast(transform.position, Vector3.up, 1.0f)) {
            isCrouching = false;
            crouchingTimeLeft = crouchingDelay - crouchingTimeLeft;
        }
        float T = crouchingTimeLeft / crouchingDelay;
        if (!isCrouching) T = 1 - T;

        float height = MathfX.sinerp(crouchingViewHeight, standingViewHeight, T);
        firstPersonCameraController.transform.localPosition = new Vector3(0, height, 0);
        height = MathfX.sinerp(-0.5f, 0, T);
        playerCollider.center = new Vector3(0, height, 0);
        height = MathfX.sinerp(1, 2, T);
        playerCollider.height = height;

        crouchingTimeLeft -= Time.fixedDeltaTime;
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
