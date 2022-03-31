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
    Camera playerView;

    int currentWeapon = 1;
    [SerializeField] float moveSpeed = 1000f;
    [SerializeField] float jumpHeight = 20f;
    [SerializeField] float senseX = 17f;
    [SerializeField] float senseY = 17f;
    [SerializeField] GameObject cameraHolder;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] GameObject bullet;
    [SerializeField] float gravityScale = 100f;

    [SerializeField] GameObject gun;
    [SerializeField] GameObject axe;

    int playerHealth = 10;
    [SerializeField] Slider healthBar;
    
    bool inventoryOpen;
    [SerializeField] GameObject Inventory;


    void Start()
    {
        player = GetComponent<Rigidbody>();
        playerView = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        gun.SetActive(true);
        axe.SetActive(false);
       
        inventoryOpen = false;
        Inventory.SetActive(inventoryOpen);
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        MoveCamera();
        limitRotation();
        CheckDeath();
        healthBar.value = playerHealth;
    }

    void FixedUpdate() 
    {
        player.AddForce(Physics.gravity * gravityScale);
    }

    void Run()
    {   Vector3 playerMovement = new Vector3(moveInput.x * moveSpeed, 0, moveInput.y * moveSpeed);
        player.AddRelativeForce(playerMovement * Time.deltaTime, ForceMode.Impulse);
    }

    void MoveCamera()
    {
        transform.Rotate(0, lookInputX * senseX * Time.deltaTime, 0);
        cameraHolder.transform.Rotate(-lookInputY * senseY * Time.deltaTime, 0, 0);
    }


    void OnJump(InputValue value)
    {
        if(value.isPressed)
        {
            player.velocity += new Vector3 (0f, jumpHeight, 0f);
        }
        //Debug.Log();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
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

    void OnFire(InputValue value)
    {
        if(currentWeapon == 1)
        {
            Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            //Instantiate(bullet);
        }
        else
        {
            AxeAttack();
        }
        //Debug.Log(value);
    }

    void AxeAttack()
    {
        Animator anim = axe.GetComponent<Animator>();
        anim.SetTrigger("isSwing");
    }

    IEnumerable AxeCooldown()
    {
        yield return new WaitForSeconds(1);

    }

    void OnSwapWeapon(InputValue value)
    {
        Debug.Log(value);
        Debug.Log(currentWeapon);
        if(currentWeapon == 1)
        {
            currentWeapon = 2;
        }
        else
        {
            currentWeapon = 1;
        }

        switchWeapon();
    }

    void switchWeapon()
    {
        if(currentWeapon == 1)
        {
            gun.SetActive(true);
            axe.SetActive(false);
        }
        else
        {
            gun.SetActive(false);
            axe.SetActive(true);
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


    public void DamagePlayer()
    {
        playerHealth--;
    }

    void CheckDeath()
    {
        if(playerHealth < 1)
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
