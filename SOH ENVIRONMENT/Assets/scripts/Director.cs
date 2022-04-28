using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Director : MonoBehaviour
{
    public bool isPaused = false;
    [SerializeField] Canvas pauseMenu;

    void Start()
    {
        pauseMenu.gameObject.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isPaused = false;
        Time.timeScale = 1;
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
