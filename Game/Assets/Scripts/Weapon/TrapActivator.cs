using UnityEngine;
using System.Collections;

public class TrapActivator : MonoBehaviour {

    public Trap trap;
	void OnTriggerEnter(Collider col)
    {
        trap.activated = true;
        trap.OnTriggerEnter(col);
        Destroy(this);
    }
}
