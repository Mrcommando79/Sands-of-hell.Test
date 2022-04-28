using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPew : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(destroyBullet());
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player")
        {
            Health playerHealth = other.GetComponent<Health>();
            playerHealth.TakeDamage(1);
        }

        Destroy(gameObject);
    }

    IEnumerator destroyBullet()
    {
        yield return new WaitForSecondsRealtime(5f);

        Destroy(gameObject);

    }
}
