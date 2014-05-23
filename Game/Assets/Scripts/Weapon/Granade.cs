using UnityEngine;
using System.Collections;

public class Granade : MonoBehaviour
{
    public float fuse = 3.0f;
    public float radius = 3.0f;
    public float damage = 3.0f;
    public GameObject explosion;
    private float detonationTime = float.PositiveInfinity;
    bool detonated = false;

    // Use this for initialization
    void Start()
    {
        detonationTime = Time.time + fuse;
    }

    void FixedUpdate()
    {
        if (detonated)
        {
            if (!audio.isPlaying)
                Destroy(gameObject);
        }
        else if (Time.time >= detonationTime)
        {
            Detonate();
        }
    }

    void Detonate()
    {
        detonated = true;
        Instantiate(explosion, this.transform.position, this.transform.rotation);
        audio.Play();
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<Collider>());
        Destroy(GetComponent<Renderer>());

        var colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var col in colliders)
        {
            Damageable dam = col.gameObject.GetComponent<Damageable>();
            if (dam != null)
            {
                Vector3 dir = col.transform.position - transform.position;
                dam.DealDamage(dir.normalized * (1 - dir.magnitude / radius) * damage);
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        Detonate();
    }
}
