using UnityEngine;
using System.Collections;

public class MaterialProperties : MonoBehaviour
{
    public AudioClip[] walkSounds;

	public virtual void Hit(RaycastHit hit, Vector3 force) { }
}
