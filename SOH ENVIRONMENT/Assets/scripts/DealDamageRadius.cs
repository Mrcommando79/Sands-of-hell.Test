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
        Collider[] hitCollider = Physics.OverlapSphere(transform.position, 3f);
        ParticleSystem instance = Instantiate(explosion, transform.position, Quaternion.identity);
        instance.Play();
        Destroy(instance.gameObject, 0.5f);

        foreach(Collider hit in hitCollider)
        {
            Health damage = hit.GetComponent<Health>();
            if(damage != null)
            {
                damage.TakeDamage(2);
            }
        }

        Destroy(gameObject);
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
