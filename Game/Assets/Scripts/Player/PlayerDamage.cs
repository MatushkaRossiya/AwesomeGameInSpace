using UnityEngine;
using System.Collections;

public class PlayerDamage : Damageable
{
    PlayerStats playerHealth;
    public AudioClip[] scream;

    private Transform flashlighObject;
    private Flashlight flashlight;

    // Use this for initialization
    void Start()
    {
        playerHealth = GetComponent<PlayerStats>();

        flashlighObject = transform.FindChild("FirstPersonCamera").FindChild("Flashlight");
        if (flashlighObject != null) flashlight = flashlighObject.GetComponent<Flashlight>();
    }

    public override void DealDamage(Vector3 damage)
    {
        audio.PlayOneShot(scream[Random.Range(0, scream.Length)], 10.0f);
        playerHealth.health -= damage.magnitude;
        HUDEffects.instance.showEffect(playerHealth.health);
        playerHealth.rigidbody.AddForce(damage * 100.0f);
        if (flashlight != null)
            flashlight.Flicker(damage.magnitude);
    }
}
