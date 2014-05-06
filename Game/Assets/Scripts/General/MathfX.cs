using UnityEngine;
using System.Collections;

public class MathfX{
    public static float sinerp(float a, float b, float t) {
        t = (1 - Mathf.Cos(t * Mathf.PI)) * 0.5f;
        return Mathf.Lerp(a, b, t);
    }
}
