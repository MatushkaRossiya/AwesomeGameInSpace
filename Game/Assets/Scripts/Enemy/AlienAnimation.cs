using UnityEngine;
using System.Collections;

public sealed class AlienAnimation : RagdollAnimation
{
    Rigidbody[] skeleton;
    Animator animator;
    CharacterAI characterAI;
	public GameObject syfPrefab;
	public int syfDropAmount;

    public override bool isRagdoll
    {
        get { return _isRagdoll; }
        set
        {
            if (value && !_isRagdoll)
            {
                _isRagdoll = true;
                animator.enabled = false;
                foreach (var bone in skeleton)
                {
                    bone.isKinematic = false;
                }

            }
            else if (!value && _isRagdoll)
            {
                _isRagdoll = false;
                animator.enabled = true;
                foreach (var bone in skeleton)
                {
                    bone.isKinematic = true;
                }
            }
        }
    }

    private bool isDead { get { return characterAI.isDead; } }

    void Start()
    {
        skeleton = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        characterAI = GetComponent<CharacterAI>();
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            float speed = characterAI.velocity.magnitude / 1.513f;
            if (speed < 0.1 || speed > 1.0f)
                animator.speed = 1.0f;
            else
                animator.speed = speed;
            animator.SetFloat("Speed", speed);
        }
    }

    public override void DealDamage(BodyPart bodyPart, float rawDamage)
    {
        float damage;
        switch (bodyPart.type)
        {
            case BodyPart.Part.arm:
                damage = 0.5f * rawDamage;
                break;
            case BodyPart.Part.head:
                damage = 10.0f * rawDamage;
                break;
            case BodyPart.Part.leg:
                damage = 0.5f * rawDamage;
                break;
            case BodyPart.Part.torso:
                damage = rawDamage;
                break;
            default:
                damage = 0;
                break;
        }
        characterAI.DealDamage(damage);
    }

	void OnDestroy() {
		float prob = (float) syfDropAmount / (float) skeleton.Length;
		foreach (var bone in skeleton) {
			if (Random.value < prob) {
				SyfCollectible syf = (Instantiate(syfPrefab, bone.position, Random.rotation) as GameObject).GetComponent<SyfCollectible>();
				syf.value = 1;
			}
		}
	}
}
