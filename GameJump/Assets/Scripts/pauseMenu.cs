using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pause;
    public Button pauseMenuButton;

    void Start()
    {
        pause.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        pause.SetActive(!pause.activeSelf);
        Time.timeScale = pause.activeSelf ? 0 : 1;
    }

    public void PauseOff()
    {
        pause.SetActive(false);
        Time.timeScale = 1;
    }

    public void Menu()
    {
        //SceneManager.LoadScene(0);
        //Time.timeScale = 1;
    }

    public void RestartLvl()
    {

    }
}
