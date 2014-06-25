using UnityEngine;
using System.Collections;

public class AlienController : MonoBehaviour
{
    private float nextAttack;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void AttackFast(float alien)
    {
        if (Time.fixedTime > nextAttack) animator.SetBool("FastAttack", true);
        Attack(1.0f, 5.0f * alien);
    }

    public void AttackStrong(float alien)
    {
        if (Time.fixedTime > nextAttack) animator.SetBool("StrongAttack", true);
        Attack(2.0f, 15.0f * alien);
    }

    public void Dodge()
    {
        if (!GetComponent<Alien>().isDead)
        {          
           
            
            bool hit = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), 1.0f);

            if (hit) animator.SetBool("DodgeLeft", true);
            else animator.SetBool("DodgeRight", true);
        }
    }

    IEnumerator dodging(bool dir)
    {
        for (int i = 0; i < 10; ++i)
        {
            transform.position += transform.TransformDirection(dir ? Vector3.left : Vector3.right) / 10.0f;
            yield return new WaitForSeconds(0.03f);
        }
        yield return null;
    }

    private void Attack(float cooldown, float multiplier)
    {
        if (Time.fixedTime > nextAttack)
        {
            nextAttack = Time.fixedTime + cooldown;
            RaycastHit[] hits = Physics.SphereCastAll(transform.position + new Vector3(0, 1.5f, 0), 0.5f, transform.forward, 2.0f, Layers.enemyAttack);
            //Debug.DrawRay(transform.position + new Vector3(0, 1.5f, 0), transform.forward, Color.cyan, 1.0f);
            if (hits.Length > 0)
            {
                //  Debug.Log(hits.Length);
                foreach (var hit in hits)
                {
                    Damageable obj = hit.collider.GetComponent<Damageable>();
                    //Debug.Log(obj.tag);
                    if (obj != null)
                    {                        
                        obj.DealDamage(transform.forward * multiplier);
                        if (obj.gameObject.name == "Player")
                        {
                            GetComponent<BaseFSM>().damageDealt += (transform.forward * multiplier).magnitude;
                            MusicMaster.instance.startFightMusic();
                        }
                    }
                }
            }
        }


    }


    internal void DodgeBack()
    {
        animator.SetBool("DodgeBack", true);
    }
}
