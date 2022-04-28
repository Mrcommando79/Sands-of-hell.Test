using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBehavior : MonoBehaviour
{
    Transform player;
    bool alreadyAttacked;
    bool alreadyTeleported;
    bool deployAxes;
    bool isAlive;

    Health health;
    [SerializeField] Slider healthBar;

    Animator animator;
    [SerializeField] GameObject axeHolder;

    [SerializeField] GameObject projectile;
    [SerializeField] float timeBetweenAttacks = 2f;
    [SerializeField] float timeBetweenTeleports = 0.5f;
    public List<Transform> teleportPoints;
    Vector3 intialPosition;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
        intialPosition = transform.position;
        alreadyAttacked = false;
        alreadyTeleported = false;
        deployAxes = true;
        isAlive = true;
        IntializeHealthBar();
        IntializeTeleportPoints();
    }

    // Update is called once per frame
    void Update()
    {
        if(health.GetHealth() > 125 && isAlive)
        {
            AttackPlayer();
        }
        else if(isAlive)
        {
            EnrageAttack();
        }

        CheckDeath();

        if(isAlive)
        {
            healthBar.value = health.GetHealth();
        }

    }

    void IntializeHealthBar()
    {
        healthBar = Instantiate(healthBar, new Vector3(0,0,0), Quaternion.identity);
        healthBar.transform.SetParent(GameObject.FindGameObjectWithTag("UI").transform, false);
        healthBar.maxValue = health.GetHealth();
        healthBar.value = health.GetHealth();
    }

    void IntializeTeleportPoints()
    {
       GameObject[] array = GameObject.FindGameObjectsWithTag("BossTeleport");

       foreach(GameObject i in array)
       {
           teleportPoints.Add(i.transform);
       }
    }

    private void AttackPlayer()
    {
        gameObject.transform.LookAt(player);

        if(!alreadyAttacked)
        {
            Invoke(nameof(Attack), 0.25f);
            Invoke(nameof(Attack), 0.50f);
            Invoke(nameof(Attack), 0.75f);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void EnrageAttack()
    {
        gameObject.transform.LookAt(player);

        if(deployAxes)
        {
            ActivateAxes();
        }

        if(!alreadyTeleported)
        {
            Attack();

            alreadyTeleported = true;
            Invoke(nameof(Teleport), timeBetweenTeleports);
        }
    }

    void ActivateAxes()
    {
        axeHolder.SetActive(true);
        animator.SetTrigger("IsSwing");

        deployAxes = false;
    }

    void Teleport()
    {
        int point = Random.Range(0, teleportPoints.Count);

        transform.position = teleportPoints[point].position;
        alreadyTeleported = false;
    }

    void Attack()
    {
        Rigidbody rb =  Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 20f, ForceMode.Impulse);
        rb.AddForce(transform.up* 5f, ForceMode.Impulse);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    void CheckDeath()
    {
        if(health.CheckDeath() && isAlive)
        {
            Destroy(healthBar.gameObject);
            isAlive = false;
            transform.position = intialPosition;
            axeHolder.SetActive(false);
            animator.SetTrigger("IsDead");
            Destroy(gameObject, 5f);            
        }
    }
}
