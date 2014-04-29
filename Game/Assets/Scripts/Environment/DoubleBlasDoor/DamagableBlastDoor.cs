using UnityEngine;
using System.Collections;

public class DamagableBlastDoor : Damageable {
    public DoubleBlastDoor door;
	private bool destroyed = false;
	private const float forceMultiplier = 100.0f;

    public override void DealDamage(Vector3 damage) {
        if (!destroyed) {
            door.hitPoints -= damage.magnitude;
			if (door.hitPoints < 0) {
				door.left.destroyed = true;
				door.right.destroyed = true;
				damage *= forceMultiplier;
                Rigidbody temp = door.left.gameObject.AddComponent<Rigidbody>();
                temp.mass = 100;
                temp.AddForce(damage, ForceMode.Impulse);

                temp = door.right.gameObject.AddComponent<Rigidbody>();
                temp.mass = 100;
                temp.AddForce(damage, ForceMode.Impulse);

                Destroy(door.left.transform.GetChild(0).gameObject);
                Destroy(door.right.transform.GetChild(0).gameObject);

                Destroy(door);
            }
        } else {
			damage *= forceMultiplier;
            rigidbody.AddForce(damage, ForceMode.Impulse);
        }
    }
}
