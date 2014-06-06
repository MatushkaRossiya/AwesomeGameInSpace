using UnityEngine;
using System.Collections;
using System.Linq;


public class Mine : MonoBehaviour
{

    public GameObject explosionParticle;
    public GameObject explosionMark;
    public float force = 200.0f;
    public float radius = 2.0f;
    private bool exploded = false;
    private static float offset = 0.0001f;
    void FixedUpdate()
    {
        if (exploded && !audio.isPlaying) Destroy(this.gameObject);
    }
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

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, Layers.damage);
        var killAlienz = colliders.Where(n => n.GetComponentInParent<Alien>()).Select(n => n.GetComponentInParent<Alien>());
        foreach (Alien alien in killAlienz) alien.MineHit();
        foreach (Collider col in colliders)
        {
            if (col && col.rigidbody)
            {
                Vector3 disturbance = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
                col.rigidbody.AddForce(((col.transform.position - transform.position).normalized + disturbance) * force * Random.Range(0.95f, 1.05f), ForceMode.Impulse);
            }
        }

        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer meshRenderer in meshRenderers) meshRenderer.enabled = false;

    }
}
