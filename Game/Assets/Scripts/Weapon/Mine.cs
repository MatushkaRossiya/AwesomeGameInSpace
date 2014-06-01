using UnityEngine;
using System.Collections;
using System.Linq;

//Landmine has taken my sight

public class Mine : MonoBehaviour {

    //Taken my speech

    public GameObject explosionParticle;
    public float force = 200.0f;
    private bool exploded = false;
    private AudioSource audio;

    //Taken my hearing

	void Start() {
        audio = GetComponent<AudioSource>();
	}

    //Taken my arms

    void Update()
    {
        if (exploded && !audio.isPlaying) Destroy(this.gameObject);
    }

    //Taken my legs

    void OnCollisionEnter(Collision col)
    {
         if(!exploded) if (col.gameObject.tag == "Alien") Boom();
    }

    //Taken my soul

    private void Boom()
    {
        Instantiate(explosionParticle , transform.position, Quaternion.identity);
        audio.Play();
        
        Debug.DrawLine(transform.position, transform.position + Vector3.right + Vector3.right, Color.green, 5.0f);
        int layerMask = 1 << 9;
        Collider[] cols = Physics.OverlapSphere(transform.position, 2.0f, layerMask);
        var killAlienz = cols.Where(n => n.GetComponent<Alien>()).Select(n => n.GetComponent<Alien>());
        foreach (var alien in killAlienz) alien.Kill();
        foreach (Collider hit in cols)
        {

            if (hit && hit.rigidbody )
            {
                hit.rigidbody.AddForce((hit.transform.position - transform.position).normalized * force, ForceMode.Impulse);               
            }
        }
        exploded = true;
        var m = GetComponentsInChildren<MeshRenderer>();
        foreach (var a in m) a.enabled = false;
    }

}
//Left me with life in hell
