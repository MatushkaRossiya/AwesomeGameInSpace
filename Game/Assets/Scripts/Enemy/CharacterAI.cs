using UnityEngine;
using System.Collections;

public abstract class CharacterAI : MonoBehaviour{
    public abstract bool isDead { get; set; }
    public abstract Vector3 velocity{ get; }
    public abstract void DealDamage(float damage);
}
