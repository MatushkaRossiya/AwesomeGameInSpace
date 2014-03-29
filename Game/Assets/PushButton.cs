using UnityEngine;
using System.Collections;

public class PushButton : MonoBehaviour {
    public Door door;

    void OnCollisionEnter(Collision col)
    {
        door.Toggle();
    }
}
