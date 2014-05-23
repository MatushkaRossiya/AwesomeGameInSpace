using UnityEngine;
using System.Collections;

public class PlayerDamage : Damageable
{
    PlayerStats playerHealth;

    private Transform flashlighObject;
    private Flashlight flashlight;

    // Use this for initialization
    void Start()
    {
        playerHealth = GetComponent<PlayerStats>();

        flashlighObject = transform.FindChild("Flashlight");
        if (flashlighObject) flashlight = flashlighObject.GetComponent<Flashlight>();
    }

    public override void DealDamage(Vector3 damage)
    {
        playerHealth.health -= damage.magnitude;
        playerHealth.rigidbody.AddForce(damage * 100.0f);
        if (flashlight)
            flashlight.Flicker(damage.magnitude);
    }
}
