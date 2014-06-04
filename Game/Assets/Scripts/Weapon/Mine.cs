using UnityEngine;
using System.Collections;
using System.Linq;

//Landmine has taken my sight

public class Mine : MonoBehaviour {

    //Taken my speech

    public GameObject explosionParticle;
    public GameObject explosionMark;
    public float force = 200.0f;
    public float radius = 2.0f;
    private bool exploded = false;
    private static float offset = 0.0001f;

    //Taken my hearing

	void Start()
    {
	}

    //Taken my arms

    void FixedUpdate()
    {
        if (exploded && !audio.isPlaying) Destroy(this.gameObject);
    }

    //Taken my legs

    void OnTriggerEnter(Collider col)
    {
        if (!exploded)
        {
            if ((1 << col.gameObject.layer & Layers.enemy) != 0) 
            {
                Boom();
            }
        }
    }

    //Taken my soul

    private void Boom()
    {
        exploded = true;
        audio.Play();
        Instantiate(explosionParticle, transform.position, Quaternion.identity);

        RaycastHit hitInfo;
        Vector3 start = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
        Vector3 direction = -transform.up;
        
        bool hit = Physics.Raycast(start, direction, out hitInfo, radius + 0.1f, Layers.environment);

        if (hit)
        {
            GameObject mark = Instantiate(explosionMark, hitInfo.point + offset * hitInfo.normal, Quaternion.LookRotation(hitInfo.normal, Random.onUnitSphere)) as GameObject;
            mark.transform.parent = hitInfo.collider.transform;
            ExplosionMarkManager.instance.AddExplosionMark(mark);
            offset += 0.0001f;
            if (offset > 0.001f) offset = 0.0001f;
        }
        
        //Debug.DrawLine(transform.position, transform.position + Vector3.right + Vector3.right, Color.green, 5.0f);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, Layers.damage);
        foreach (var col in colliders) {
            Damageable dam = col.gameObject.GetComponent<Damageable>();
            if (dam != null) {
                Vector3 dir = col.transform.position - transform.position;
                dam.DealDamage(dir.normalized * (1 - dir.magnitude / radius) * force);
            }
        }

        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer meshRenderer in meshRenderers) meshRenderer.enabled = false;
    }

}
//Left me with life in hell
