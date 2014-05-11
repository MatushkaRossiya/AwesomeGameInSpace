using UnityEngine;
using System.Collections;

public class DamagableBlockade : Damageable
{
    private bool destroyed = false;
    private const float forceMultiplier = 50.0f;
    public float hitPoints = 200.0f;

    public override void DealDamage(Vector3 damage)
    {
        if (!destroyed)
        {
            hitPoints -= damage.magnitude;
            if (hitPoints <= 0.0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
