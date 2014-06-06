using UnityEngine;
using System.Collections;

public class HeavyAlien : Alien
{
    private bool exploded = false;

    void Start()
    {
        base.Start();
    }
    
    public override void MineHit()
    {
        if (!exploded)
        {
            currentHitPoints -= maxHitPoints * 0.5f;
            if (currentHitPoints < 0)
                Kill();
            else
                StartCoroutine(ExplosionShock());
        }
    }

    public override void Kill()
    {
        if (!isDead)
        {
            GetComponent<BaseFSM>().currentState = BaseFSM.State.Patrol;
            GetComponent<HeavyAlienFSM>().brain.Fitness(GetComponent<HeavyAlienFSM>().timeAttacking, GetComponent<HeavyAlienFSM>().damageDealt);
        }
        base.Kill();
    }

    IEnumerator ExplosionShock()
    {
        exploded = true;
        
        GetComponent<HeavyAlienFSM>().enabled = false;
        navMeshAgent.enabled = false;
        ragdollAnimation.isRagdoll = true;
        yield return new WaitForSeconds(3.0f);
        transform.position = transform.FindChild("Bip001").FindChild("Bip001 Pelvis").position;
        navMeshAgent.enabled = true;
        ragdollAnimation.isRagdoll = false;
        GetComponent<HeavyAlienFSM>().enabled = true;
        exploded = false;
        yield return null;

    }
}
