using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody bulletBody;
    [SerializeField] float bulletSpeed = 20f;
    void Start()
    {
        bulletBody = GetComponent<Rigidbody>();
        bulletBody.AddRelativeForce(0 , 0 , bulletSpeed);
        StartCoroutine(destroyBullet());
        
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "ENEMY")
        {
            Destroy(other.gameObject);
        }

        Destroy(gameObject);
    }

    IEnumerator destroyBullet()
    {
        yield return new WaitForSecondsRealtime(5f);

        Destroy(gameObject);

    }
}
