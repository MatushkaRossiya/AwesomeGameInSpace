using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
    float opened = 0.0f;
    Vector3 basePosition;
    public Vector3 offset;

    void Start()
    {
        basePosition = transform.position;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, basePosition + offset * opened, Time.fixedDeltaTime);
    }

    public void Toggle()
    {
        opened = 1.0f - opened;
    }
}
