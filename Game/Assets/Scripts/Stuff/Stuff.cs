using UnityEngine;
using System.Collections;

public class Stuff : Interactive
{
    float deathTime;

    void Start()
    {
        deathTime = Time.fixedTime + Random.Range(10.0f, 20.0f);
    }

    void FixedUpdate()
    {
        if (Time.fixedTime > deathTime)
            Destroy(gameObject);
    }

    public override void Action()
    {
        DataBank.stuffCount++;
        Destroy(gameObject);
    }

    public override string message
    {
        get { return "PICK STUFF UP"; }
    }
}
