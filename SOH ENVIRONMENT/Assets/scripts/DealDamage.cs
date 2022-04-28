using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField] bool isProjectile = true;

    Rigidbody bulletBody;
    [SerializeField] float bulletSpeed = 20f;
    [SerializeField] float bulletLifetime = 5f;
    void Start()
    {
        if(isProjectile)
        {
            bulletBody = GetComponent<Rigidbody>();
            bulletBody.AddRelativeForce(0 , 0 , bulletSpeed);
            StartCoroutine(destroyBullet());
        }
        
    }

    void OnTriggerEnter(Collider other) 
    {
        if((other.tag == "ENEMY" || other.tag == "Player") && !isProjectile)
        {
            other.GetComponent<Health>().TakeDamage(10);
        }
        else if(other.tag == "ENEMY")
        {
            other.GetComponent<Health>().TakeDamage(5);
            Destroy(gameObject);
        }

    }

    IEnumerator destroyBullet()
    {
        yield return new WaitForSecondsRealtime(bulletLifetime);

        Destroy(gameObject);

    }


}
