using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trap : MonoBehaviour
{


    public GameObject center;
    public GameObject field;

    public ParticleSystem trapParticles;
    public float activeTime = 7.0f;
    private float time;
    private bool used = false;
    private bool _activated = false;
    internal bool activated
    {
        set
        {
            _activated = value;
            if (value)
            {
                trapParticles.Play();
                audio.Play();
                StartCoroutine(turnOn());
                time = Time.time + activeTime;
            }
        }
        get { return _activated; }
    }

    private List<GameObject> trappedThings = new List<GameObject>();
    private HashSet<Rigidbody> trappedRigidbodies = new HashSet<Rigidbody>();
    private HashSet<Alien> trappedAliens = new HashSet<Alien>();
    private bool playerTrapped = false;

    internal void OnTriggerEnter(Collider col)
    {
        if (activated)
        {
            if (col.gameObject.GetComponentInParent<Alien>()) trapAlien(col.gameObject.GetComponentInParent<Alien>());
            else if (col.GetComponent<PlayerController>()) trapPlayer();
            else if (col.rigidbody || col.gameObject.GetComponentsInChildren<Rigidbody>().Length > 0 || col.gameObject.GetComponentsInParent<Rigidbody>().Length > 0) trapOther(col.transform.root.gameObject);
        }
    }

    private void trapOther(GameObject thing)
    {
        if (thing.rigidbody)
        {
            thing.rigidbody.useGravity = false;
            trappedRigidbodies.Add(thing.rigidbody);
        }
        if (thing.GetComponentsInChildren<Rigidbody>().Length > 0)
        {
            foreach (Rigidbody body in thing.GetComponentsInChildren<Rigidbody>())
            {
                body.useGravity = false;
                trappedRigidbodies.Add(body);
            }
        }
        if (thing.GetComponentsInParent<Rigidbody>().Length > 0)
        {
            foreach (Rigidbody body in thing.GetComponentsInParent<Rigidbody>())
            {
                body.useGravity = false;
                trappedRigidbodies.Add(body);
            }
        }
        trappedThings.Add(thing);
    }

    private void trapPlayer()
    {
        playerTrapped = true;
        PlayerController.instance.canMove = false;
        PlayerController.instance.gameObject.rigidbody.useGravity = false;
        trappedRigidbodies.Add(PlayerController.instance.gameObject.rigidbody);
        PlayerController.instance.gameObject.rigidbody.AddForce(Vector3.up * 5.0f, ForceMode.Impulse);
    }
    private float r = 0.35f;
    void Start()
    {
        trapParticles.time = activeTime;
    }


    // Update is called once per frame
    void FixedUpdate()
    {

        if (!activated && !used) center.renderer.material.color = new Color(0.4f * Mathf.Sin(2.0f * Time.time) + 0.6f, 0.0f, 0.0f);


        if (activated)
        {
            if (Time.time > time)
            {
                used = true;
                StartCoroutine(turnOff());
            }
            else
            {
                foreach (Rigidbody body in trappedRigidbodies)
                {
                    if (body)
                    {
                        body.AddForce(Vector3.up * Mathf.Sin(0.5f * Time.time) * Random.Range(0.0f, 0.05f), ForceMode.Acceleration);
                        body.transform.position = new Vector3(
                             Mathf.Clamp(body.transform.position.x, transform.position.x - r, transform.position.x + r),
                             body.transform.position.y,
                             Mathf.Clamp(body.transform.position.z, transform.position.z - r, transform.position.z + r));

                    }
                }
            }
        }
    }

    private void ReleaseAll()
    {
        foreach(Rigidbody body in trappedRigidbodies)
        {
           if(body) body.AddForce(( body.transform.position - transform.position).normalized * 15.5f, ForceMode.VelocityChange);
        }


        if (playerTrapped)
        {
            PlayerController.instance.canMove = true;
           
        }

        foreach (Rigidbody body in trappedRigidbodies) if (body) body.useGravity = true;

        foreach (Alien alien in trappedAliens)
        {
            if (!alien.isDead) alien.Untrap();

        }
    }


    private void trapAlien(Alien alien)
    {
        alien.gameObject.GetComponent<AlienAnimation>().isRagdoll = true;
        foreach (Rigidbody body in alien.gameObject.GetComponentsInChildren<Rigidbody>()) body.useGravity = false;
        alien.GetComponent<NavMeshAgent>().enabled = false;
        alien.gameObject.GetComponent<BaseFSM>().enabled = false;
        trappedRigidbodies.Add(alien.gameObject.transform.FindChild("Bip001").FindChild("Bip001 Pelvis").rigidbody);
        trappedRigidbodies.Add(alien.gameObject.transform.FindChild("Bip001").FindChild("Bip001 Pelvis").FindChild("Bip001 Spine").FindChild("Bip001 Spine1").rigidbody);

        alien.gameObject.transform.FindChild("Bip001").FindChild("Bip001 Pelvis").rigidbody.AddForce(Vector3.up * 0.5f, ForceMode.Impulse);
        trappedAliens.Add(alien);

    }


    void OnBecameInvisible()
    {
        if (used) Destroy(this.gameObject);
    }
    IEnumerator turnOn()
    {
        for (int i = 0; i <= 200; ++i)
        {
            field.renderer.material.color = new Color(1.0f - (i / 200.0f), (i / 200.0f), (i / 200.0f), 0.1f + 0.3f * (i / 200.0f));
            center.renderer.material.color = new Color(Mathf.Clamp01(center.renderer.material.color.r - 1 / 200.0f), (i / 200.0f), (i / 200.0f), 1.0f);
            yield return new WaitForSeconds(0.01f);
        }
        yield return null;
    }

    IEnumerator turnOff()
    {
        activated = false;
        for (int i = 100; i >= 0; --i)
        {
            audio.volume = (i / 100.0f);
            center.renderer.material.color = new Color(
                (100 - i) / 200.0f,
                 (100 + i) / 200.0f,
                 (100 + i) / 200.0f,
                i / 100.0f);
            field.renderer.material.color = new Color(0.0f, 1.0f, 1.0f, 0.4f * (i / 100.0f));

            if(i==50) ReleaseAll();

            yield return new WaitForSeconds(0.01f);

        }

        audio.Stop();
        Destroy(field);
        
        yield return null;

    }
}
