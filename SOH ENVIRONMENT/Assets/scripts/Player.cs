using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody player;
    Vector2 moveInput;
    float lookInputX;
    float lookInputY;
    BoxCollider axeHitBox;
    LayerMask groundLayer;
    int currentWeapon = 0;
    Director dir;
    
    [SerializeField] Camera playerView;
    [SerializeField] float moveSpeed = 1000f;
    [SerializeField] float jumpHeight = 20f;
    [SerializeField] float senseX = 17f;
    [SerializeField] float senseY = 17f;
    [SerializeField] GameObject cameraHolder;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] Transform railBulletSpawnPoint;
    [SerializeField] GameObject bullet;
    [SerializeField] float gravityScale = 100f;
    [SerializeField] List<GameObject> weapons;
    [SerializeField] ParticleSystem railParticle;

    Health health;
    [SerializeField] Slider healthBar;

    [SerializeField] float railTimer = 1f;
    [SerializeField] Slider railCharger;
    bool ableToFire = true;
    
    bool inventoryOpen;
    [SerializeField] GameObject Inventory;


    void Start()
    {
        player = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
        dir = FindObjectOfType<Director>();
        Cursor.lockState = CursorLockMode.Locked;
        inventoryOpen = false;
        Inventory.SetActive(inventoryOpen);
        groundLayer = LayerMask.GetMask("GROUND");
        IntializeWeapons();
        railCharger.maxValue = railTimer;
        healthBar.maxValue = health.GetHealth();
        healthBar.value = healthBar.maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        MoveCamera();
        limitRotation();
        CheckDeath();
        healthBar.value = health.GetHealth();
        if(!ableToFire)
        {
            UpdateRailCharger();
        }
    }

    void FixedUpdate() 
    {
        player.AddForce(Physics.gravity * gravityScale);
    }

    void Run()
    {  
        Vector3 playerMovement = new Vector3(moveInput.x * moveSpeed, 0, moveInput.y * moveSpeed);
        player.AddRelativeForce(playerMovement * Time.deltaTime, ForceMode.Impulse);
    }

    void MoveCamera()
    {
        transform.Rotate(0, lookInputX * senseX * Time.deltaTime, 0);
        cameraHolder.transform.Rotate(-lookInputY * senseY * Time.deltaTime, 0, 0);
    }


    void OnJump(InputValue value)
    {
        if(value.isPressed && Physics.Raycast(transform.position, Vector3.down, 1f))
        {
            player.velocity += new Vector3 (0f, jumpHeight, 0f);
        }
        //Debug.Log();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        //Debug.Log(moveInput);
    }

    void OnMouseX(InputValue value)
    {
        lookInputX = value.Get<float>();
        //Debug.Log(cameraHolder.transform.rotation.x);
    }

    void OnMouseY(InputValue value)
    {
        lookInputY = value.Get<float>();
        //Debug.Log(lookInputY);
    }

    void OnInteract(InputValue value)
    {
        RaycastHit hit;

        if(Physics.Raycast(playerView.transform.position, playerView.transform.forward, out hit, 2f))
        {
            Interactable item =  hit.transform.GetComponent<Interactable>();
            if(item != null && item.isChest)
            {
                item.OpenChest();
            }
            else if (item != null && item.isBossWall)
            {
                item.OpenBossWall();
            }

            Debug.Log(hit);
        }
    }

    void IntializeWeapons()
    {
        weapons[1].SetActive(true);
        for(int i = 1; i < weapons.Count; i++)
        {
            weapons[i].SetActive(false);
            if(weapons[i].GetComponent<BoxCollider>() != null)
            {
                axeHitBox = weapons[i].GetComponent<BoxCollider>();
                axeHitBox.enabled = false;
            }
        }
    }

    void OnFire(InputValue value)
    {
        RaycastHit hit;    

        if(currentWeapon == 0 && ableToFire)
        {
           if(Physics.Raycast(playerView.transform.position, playerView.transform.forward, out hit, 100f))
           {
                Health enemy = hit.transform.GetComponent<Health>();
                if(enemy != null && enemy.tag == "ENEMY")
                {
                    enemy.TakeDamage(10);
                }
                ParticleSystem hitEffect = Instantiate(railParticle, hit.point, Quaternion.identity);
                Destroy(hitEffect.gameObject, 2f);
           }
            StartCoroutine(RailCooldown());
        }
        else if(currentWeapon == 1)
        {
            Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        }
        else if(currentWeapon == 2)
        {
            AxeAttack();
        }
        //Debug.Log(value);
    }

    IEnumerator RailCooldown()
    {
        ableToFire = false;
        railCharger.value = 0;
        yield return new WaitForSeconds(railTimer);
        ableToFire = true;
    }

    void UpdateRailCharger()
    {
        railCharger.value += Time.deltaTime;
    }

    void AxeAttack()
    {
        axeHitBox.enabled = true;
        Animator anim = weapons[2].GetComponent<Animator>();
        anim.SetTrigger("isSwing");
        StartCoroutine(DisableCollider());
    }

    IEnumerator DisableCollider()
    {
        yield return new WaitForSeconds(1f);
        axeHitBox.enabled = false;
    }

    void OnSwapWeapon(InputValue value)
    {
        weapons[currentWeapon].SetActive(false);
        if(currentWeapon < 2)
        {
            currentWeapon++;
            weapons[currentWeapon].SetActive(true);
        }
        else
        {
            currentWeapon = 0;
            weapons[currentWeapon].SetActive(true);
        }
    }

    void OnInventory(InputValue value)
    {
        if(!inventoryOpen)
        {
            inventoryOpen = true;
            Inventory.SetActive(inventoryOpen);
        }
        else
        {
            inventoryOpen = false;
            Inventory.SetActive(inventoryOpen);
        }
    }

    void limitRotation()
    {
       Vector3 headEulerAngles = cameraHolder.transform.rotation.eulerAngles;

       headEulerAngles.x = (headEulerAngles.x > 180) ? headEulerAngles.x - 360 : headEulerAngles.x;
       headEulerAngles.x = Mathf.Clamp(headEulerAngles.x , -40 , 60);

       cameraHolder.transform.rotation = Quaternion.Euler(headEulerAngles);
    }

    void CheckDeath()
    {
        if(health.CheckDeath())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void OnPause(InputValue value)
    {
        if(value.isPressed && !dir.isPaused)
        {
            Debug.Log("Test");
            dir.Pause();
        }
    }


}
