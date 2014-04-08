using UnityEngine;
using System.Collections;

public sealed class RagdollCharacter : MonoBehaviour {
    Rigidbody[] skeleton;

    public AICharacter aiCharacter;

    public float maxHitPoints;
    float _currentHitPoints;
    float currentHitPoints {
        get{ return _currentHitPoints; }
        set {
            _currentHitPoints = value;
            if (_currentHitPoints <= 0.0f) {
                this.Kill();
            }
        }
    }

    bool isDead = false;

    bool _isRagdoll;
    public bool isRagdoll {
        get { return _isRagdoll; }
        set {
            if (value && !_isRagdoll) {
                _isRagdoll = true;
                foreach (var bone in skeleton) {
                    bone.isKinematic = false;
                }

            } else if (!value && _isRagdoll) {
                _isRagdoll = false;
                foreach (var bone in skeleton) {
                    bone.isKinematic = true;
                }
            }
            aiCharacter.isAnimatorActive = value;
        }
    }


    void Start() {
        currentHitPoints = maxHitPoints;
        skeleton = GetComponentsInChildren<Rigidbody>();
        foreach (var bone in skeleton) {
            bone.isKinematic = true;
        }
    }


    IEnumerator DelayDie(float delay) {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }

    void Kill() {
        if (!isDead) {
            Debug.Log("HIT");
            isRagdoll = true;
            isDead = true;
            aiCharacter.isDead = true;
            StartCoroutine(DelayDie(5.0f));
            aiCharacter.Kill();
        }
    }

    public void DealDamage(BodyPart bodyPart, float rawDamage) {
        float damage;
        switch (bodyPart.type) {
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
        currentHitPoints -= damage;
    }
}
