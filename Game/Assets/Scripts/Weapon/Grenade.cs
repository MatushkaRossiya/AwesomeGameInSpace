using UnityEngine;
using System.Collections;

public class Grenade : MonoBehaviour
{
    public float fuse = 3.0f;
    public float radius = 3.0f;
    public float damage = 3.0f;
    public GameObject explosion;
	public GameObject explosionMark;

    private float detonationTime = float.PositiveInfinity;
    private bool detonated = false;
    private static float offset = 0.0001f;

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
        Destroy(GetComponent<Light>());

        var colliders = Physics.OverlapSphere(transform.position, radius, Layers.damage);
		foreach (var col in colliders) {
			Damageable dam = col.gameObject.GetComponent<Damageable>();
			if (dam != null) {
				Vector3 colliderToGrenade = transform.position - col.transform.position;
				if (Physics.Raycast(col.transform.position, colliderToGrenade.normalized, colliderToGrenade.magnitude - 0.2f, Layers.environment)) continue;
				Vector3 dir = col.transform.position - transform.position;
				dam.DealDamage(dir.normalized * (1 - dir.magnitude / radius) * damage);
			}
		}
    }

    void OnCollisionEnter(Collision col)
    {
        Detonate();
        if ((1 << col.collider.gameObject.layer & Layers.environment) != 0)
        {
            GameObject mark = Instantiate(explosionMark, col.contacts [0].point + offset * col.contacts [0].normal, Quaternion.LookRotation(col.contacts [0].normal, Random.onUnitSphere)) as GameObject;
            mark.transform.parent = col.collider.transform;
            ExplosionMarkManager.instance.AddExplosionMark(mark);
            offset += 0.0001f;
            if (offset > 0.001f) offset = 0.0001f;
        }
    }
}
