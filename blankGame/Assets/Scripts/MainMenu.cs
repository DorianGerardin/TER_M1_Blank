
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject menuFirstButton, settingsFirstButton;
    public GameObject mainMenu, settingsMenu;

    public void PlayGame()
    {
      Debug.Log(SceneManager.GetActiveScene().buildIndex);
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);  
    }

    public void ActivateSettingsMenu()
    {
      mainMenu.SetActive(false);
      settingsMenu.SetActive(true);
      
      EventSystem.current.SetSelectedGameObject(null);
      EventSystem.current.SetSelectedGameObject(settingsFirstButton);
    }
    
    public void ActivateMainMenu()
    {
      settingsMenu.SetActive(false);
      mainMenu.SetActive(true);
      
      EventSystem.current.SetSelectedGameObject(null);
      EventSystem.current.SetSelectedGameObject(menuFirstButton);
    }

    public void QuitGame()
    {
      Debug.Log("Quit");
      Application.Quit();
    }

    public void Awake()
    {
      EventSystem.current.SetSelectedGameObject(null);
      EventSystem.current.SetSelectedGameObject(menuFirstButton);
    }
}
