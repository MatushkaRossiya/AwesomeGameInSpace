using UnityEngine;
using System.Collections;

public class RedBarrel : Damageable {

    public GameObject explosionParticle;
    private bool exploded = false;
    public float radius = 3.5f;
    public float damageToDeal = 25.0f;
    public float timeToVanish = 4.0f;
    private float time;

    void FixedUpdate()
    {
        if (exploded)
        {
            if (Time.time > time)
            {
                GetComponentInChildren<MeshRenderer>().enabled = false;
                GetComponent<CapsuleCollider>().enabled = false;
                if (!audio.isPlaying) Destroy(this.gameObject);
            }
        }
    }

    public override void DealDamage(Vector3 damage)
    {
        if (!exploded)
        {
            exploded = true;
            time = Time.time + timeToVanish;
            audio.Play();
            Instantiate(explosionParticle, transform.position, Quaternion.identity);
            var colliders = Physics.OverlapSphere(transform.position, radius, Layers.damage);
            foreach (var col in colliders)
            {
                Damageable dam = col.gameObject.GetComponent<Damageable>();
                if (dam != null)
                {
                    Vector3 dir = col.transform.position - transform.position;                   
                    dam.DealDamage(dir.normalized * (1 - dir.magnitude / radius) * damageToDeal);
                }

            }
        }
    }
      
}
