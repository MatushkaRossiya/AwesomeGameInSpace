using UnityEngine;
using System.Collections;

public class DamagableBlastDoor : Damageable {
    public DoubleBlastDoor door;


    public override void DealDamage(Vector3 damage) {
        if (door != null) {
            door.hitPoints -= damage.magnitude;
            if (door.hitPoints < 0) {
                damage *= 100;
                Rigidbody temp = door.left.gameObject.AddComponent<Rigidbody>();
                temp.mass = 100;
                temp.AddForce(damage, ForceMode.Impulse);

                temp = door.right.gameObject.AddComponent<Rigidbody>();
                temp.mass = 100;
                temp.AddForce(damage, ForceMode.Impulse);

                Destroy(door.left.transform.GetChild(0).gameObject);
                Destroy(door.right.transform.GetChild(0).gameObject);

                DestroyImmediate(door);
            }
        } else {
            damage *= 100;
            rigidbody.AddForce(damage, ForceMode.Impulse);
        }
    }
}
