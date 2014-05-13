using UnityEngine;
using System.Collections;

public abstract class RagdollAnimation : MonoBehaviour
{
    public abstract bool isRagdoll { get; set; }

    public abstract void DealDamage(BodyPart bodyPart, float rawDamage);
	public abstract void Kill();
}
