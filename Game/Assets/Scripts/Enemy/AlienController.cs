using UnityEngine;
using System.Collections;

public class AlienController : MonoBehaviour
{
    private float nextAttack;

    public void AttackFast(float alien)
    {
        Attack(0.35f, 5.0f * alien);
    }

    public void AttackStrong(float alien)
    {
        Attack(0.85f, 15.0f * alien);
    }

    public void Dodge()
    {
        if (!GetComponent<Alien>().isDead)
        {          
            //temporary, until we have animations
            
            bool hit = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), 1.0f);

            StartCoroutine(dodging(hit));
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
            RaycastHit[] hits = Physics.SphereCastAll(transform.position + new Vector3(0, 1.5f, 0), 1.0f, transform.forward, 2.0f, Layers.enemyAttack);
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

}
