using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageRadius : MonoBehaviour
{
    [SerializeField] ParticleSystem explosion;
    Coroutine explodeCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        explodeCoroutine = StartCoroutine(ExplodeTimer());
    }

    void Explode()
    {
        Collider[] hitCollider = Physics.OverlapSphere(transform.position, 5f);
        explosion.Play();

        foreach(Collider hit in hitCollider)
        {
            Health instance = hit.GetComponent<Health>();
            if(instance != null)
            {
                instance.TakeDamage(2);
            }
        }

        Destroy(gameObject, 0.3f);
    }

    void OnCollisionEnter(Collision other) 
    {
        StopCoroutine(explodeCoroutine);
        Explode();
    }

    IEnumerator ExplodeTimer()
    {

        yield return new WaitForSeconds(1.5f);
        Explode();
    }



}
