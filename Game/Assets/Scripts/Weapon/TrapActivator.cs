using UnityEngine;
using System.Collections;

public class TrapActivator : MonoBehaviour {

    public Trap trap;
    void OnTriggerEnter(Collider col)
    {
        if (!trap.triggered && !trap.used)
        {
            if ((col.gameObject.GetComponentInParent<Alien>()))
            {
                if ((col.gameObject.GetComponentInParent<Alien>()).isDead) return;
            }
            trap.triggered = true;
            trap.OnTriggerEnter(col);
        }
    }
}
