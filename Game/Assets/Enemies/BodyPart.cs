using UnityEngine;
using System.Collections;

public class BodyPart : Damageable {
    public enum Part {
        head, torso, leg, arm
    }

    public Part type;


    public override void DealDamage(Vector3 damage) {
        RagdollCharacter ragdoll = transform.root.GetComponent<RagdollCharacter>();
        ragdoll.DealDamage(this, damage.magnitude);
        rigidbody.AddForce(damage, ForceMode.Impulse);
    }
}
