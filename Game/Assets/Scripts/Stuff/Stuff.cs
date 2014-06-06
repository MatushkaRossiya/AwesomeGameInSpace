using UnityEngine;
using System.Collections;

public class Stuff : Interactive
{
    private float deathTime;

    void Start()
    {
        deathTime = Random.Range(10.0f, 20.0f);
    }

    void FixedUpdate()
    {
        deathTime -= Time.fixedDeltaTime;
        if (deathTime <= 0.0f)
            Destroy(gameObject);
    }

    public override void MomentaryAction()
    {
        DataBank.stuffCount++;
        Destroy(gameObject);
    }

    public override string message
    {
        get { return "PICK STUFF UP"; }
    }
}
