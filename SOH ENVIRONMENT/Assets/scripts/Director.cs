using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    public bool isPaused = false;
    [SerializeField] Canvas pauseMenu;

    void Start()
    {
        pauseMenu.gameObject.SetActive(false);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.gameObject.SetActive(false);
    }
    
    public void Quit()
    {
        Application.Quit();
    }

    public void Pause()
    {
        isPaused = true;
        Cursor.lockState = CursorLockMode.Confined;
        pauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0;
    }


}
