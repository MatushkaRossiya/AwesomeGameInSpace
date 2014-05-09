using UnityEngine;
using System.Collections.Generic;
using XInputDotNetPure;

public class PlayerController : MonoSingleton<PlayerController>
{
    public float acceleration = 10.0f;
    public float walkSpeed = 10.0f;
    public float sprintSpeed = 20.0f;
    public float jumpHeight = 1.0f;
    public float standingViewHeight = 0.75f;
    public float crouchingViewHeight = -0.25f;
    public float crouchingDelay = 0.3f;
    public float stepLength;
    public List<AudioClip> walkOnMetalSounds;
    public List<AudioClip> dropSounds;
    public List<AudioClip> jumpSounds;
    public List<AudioClip> breathSounds;
    private FirstPersonCameraController firstPersonCameraController;
    private bool isTouchingGround = false;

    public bool isCrouching
    {
        get;
        private set;
    }

    public float speed
    {
        get
        {
            return rigidbody.velocity.magnitude;
        }
    }

    private float crouchingTimeLeft = 0;
    private float distance = 0.0f;
    private bool canJump;
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
    CapsuleCollider playerCollider;

    void Start()
    {
        firstPersonCameraController = transform.GetComponentInChildren<FirstPersonCameraController>();
        playerCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        canJump |= isTouchingGround && (Time.fixedTime > nextJump) && ((Input.GetAxis("Jump") > 0.5f) || (state.Buttons.A == ButtonState.Pressed));
        Breathe();
    }

    void FixedUpdate()
    {
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

        Crouch();
        Walk();
        Jump();

        isTouchingGround = false;
    }

    void Walk()
    {
        // apply mouse movement
        transform.localRotation = Quaternion.Euler(new Vector3(0, firstPersonCameraController.horizontalAngle, 0));

        // read input
        Vector3 input = new Vector3();
        input.x = Input.GetAxis("Horizontal") + state.ThumbSticks.Left.X;
        input.y = Input.GetAxis("Vertical") + state.ThumbSticks.Left.Y;

        // normalize if needed to avoid going faster diagonally
        if (input.sqrMagnitude > 1.0f)
            input.Normalize();

        // check if sprinting
        if (isCrouching)
        {
            input *= 0.5f;
        } 
        else if ((Input.GetAxis("Sprint") > 0.5f) || (state.Buttons.LeftStick == ButtonState.Pressed))
        {
            input.y = sprintSpeed / walkSpeed;
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

        if (acc.magnitude > maxAcceleration)
        {
            acc = acc.normalized * maxAcceleration;
        }

        // apply acceleration
        rigidbody.AddForce(acc, ForceMode.Acceleration);

        if (isTouchingGround)
        {
            distance += Mathf.Sqrt(rigidbody.velocity.magnitude) * Time.fixedDeltaTime;
            if (distance > stepLength)
            {
                distance = 0.0f;
                PlayRandomAudio(walkOnMetalSounds);
            }
        } 
        else
        {
            distance = 0.0f;
        }
    }

    private float nextJump = -10000.0f;

    void Jump()
    {
        if (canJump)
        {
            nextJump = Time.fixedTime + 0.5f;
            canJump = false;
            rigidbody.AddForce(Vector3.up * Mathf.Sqrt(2.0f * 9.81f * jumpHeight), ForceMode.VelocityChange);
            PlayRandomAudio(jumpSounds);
        }
    }

    void Crouch()
    {
        if (crouchingTimeLeft < 0.0f)
            crouchingTimeLeft = 0.0f;

        if (!isCrouching && (Input.GetKey(KeyCode.LeftControl) || state.Buttons.Y == ButtonState.Pressed))
        {
            isCrouching = true;
            crouchingTimeLeft = crouchingDelay - crouchingTimeLeft;
        } 
        else if (isCrouching && !(Input.GetKey(KeyCode.LeftControl) || state.Buttons.Y == ButtonState.Pressed) && !Physics.Raycast(transform.position, Vector3.up, 1.0f))
        {
            isCrouching = false;
            crouchingTimeLeft = crouchingDelay - crouchingTimeLeft;
        }

        float T = crouchingTimeLeft / crouchingDelay;
        if (!isCrouching)
            T = 1 - T;

        float height = MathfX.sinerp(crouchingViewHeight, standingViewHeight, T);
        firstPersonCameraController.transform.localPosition = new Vector3(0, height, 0);
        height = MathfX.sinerp(-0.5f, 0, T);
        playerCollider.center = new Vector3(0, height, 0);
        height = MathfX.sinerp(1, 2, T);
        playerCollider.height = height;

        crouchingTimeLeft -= Time.fixedDeltaTime;
    }

    private float fatigue = 0.0f;
    private float nextBreath;

    void Breathe()
    {
        // TODO make it better
        float targetFatigue = rigidbody.velocity.magnitude / sprintSpeed;
        fatigue = (fatigue - targetFatigue) * Mathf.Exp(-0.15f * Time.fixedDeltaTime) + targetFatigue;

        if (Time.time > nextBreath)
        {
            nextBreath = Time.time;
            int index = Mathf.FloorToInt(fatigue * breathSounds.Count);

            if (index >= breathSounds.Count)
                index = breathSounds.Count - 1;
            else if (index < 0)
                index = 0;

            AudioClip sound = breathSounds [index];
            audio.PlayOneShot(sound, 0.5f);
            nextBreath = Time.time + sound.length * 0.9f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        isTouchingGround = true;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.relativeVelocity.y > 4f)
        {
            PlayRandomAudio(dropSounds);
            PlayRandomAudio(walkOnMetalSounds);
        }
    }

    void OnTriggerStay(Collider other)
    {
        isTouchingGround = true;
    }

    void PlayRandomAudio(List<AudioClip> list)
    {
        int count = list.Count;
        if (count != 0)
            audio.PlayOneShot(list [Random.Range(0, count)]);
    }
}
