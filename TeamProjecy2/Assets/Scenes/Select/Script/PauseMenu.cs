using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false; // Å© staticí«â¡

    public GameObject pausePanel;
    bool isOpen = false;
    public GameObject mainMenu;
    public GameObject volumePanel;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }
    void ToggleMenu()
    {
        isOpen = !isOpen;
        isPaused = isOpen; // Å© í«â¡
        Debug.Log("ToggleMenu: " + isOpen);
        pausePanel.SetActive(isOpen);
        Time.timeScale = isOpen ? 0f : 1f;
    }
    public void OpenVolume()
    {
        mainMenu.SetActive(false);
        volumePanel.SetActive(true);
    }
    public void BackToMenu()
    {
        volumePanel.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}