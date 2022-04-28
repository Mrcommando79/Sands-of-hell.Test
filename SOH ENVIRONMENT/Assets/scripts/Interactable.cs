using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    Animator animator;
    bool alreadyOpened = false;
    public bool isChest;
    public bool isBossWall;
    [SerializeField] GameObject boss;
    [SerializeField] Transform entryPoint;
    [SerializeField] Transform bossSpawnPoint;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenChest()
    {
        if(!alreadyOpened)
        {
            animator.SetTrigger("isOpen");
            alreadyOpened = true;
            KeyHolder keyHolder = FindObjectOfType<KeyHolder>();
            keyHolder.AddKey();
        }
    }

    public void OpenBossWall()
    {
        KeyHolder keyHolder = FindObjectOfType<KeyHolder>();

        if(keyHolder.GetKeys() == 3)
        {
            keyHolder.transform.position = entryPoint.position;
            Instantiate(boss, bossSpawnPoint.position , Quaternion.identity);
        }
    }
}
