using UnityEngine;
using System.Collections;

public class DoubleBlastDoor : RemoteActor
{
    public float closingTime;
    public DamagableBlastDoor left, right;
    public float openingWidth;
    public bool isOpen;
    public float hitPoints;
    float direction;
    float phase;

    public override void Action()
    {
        isOpen = !isOpen;
        direction = -direction;
    }

    void Start()
    {
        direction = 1 / closingTime * Time.fixedDeltaTime;

        if (isOpen)
            phase = 1.0f;
        else
        {
            phase = 0.0f;
            direction = -direction;
        }
    }

    void FixedUpdate()
    {
        phase += direction;
        if (phase < 0)
            phase = 0;
        else if (phase > 1)
            phase = 1;

        float t = MathfX.sinerp(0, openingWidth, phase);
        Vector3 temp = left.transform.localPosition;
        temp.x = t * openingWidth;
        left.transform.localPosition = temp;

        temp = right.transform.localPosition;
        temp.x = -t * openingWidth;
        right.transform.localPosition = temp;
    }
}
