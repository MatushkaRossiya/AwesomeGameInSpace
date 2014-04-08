using UnityEngine;
using System.Collections;

public abstract class AICharacter : MonoBehaviour{
    public abstract void DealDamage(BodyPart bodyPart, float rawDamage);
    public abstract bool isAnimatorActive { get; set; }
    public abstract bool isDead { get; set; }
    public abstract void Kill();
}
