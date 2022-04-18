
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject menuFirstButton, settingsFirstButton;
    public GameObject mainMenu, settingsMenu;
    
    [SerializeField]  Slider volumeSlider;

    private SfxManager sfxManager;
    public bool hasSfxManager = false;

    public void PlayGame()
    {
      Debug.Log(SceneManager.GetActiveScene().buildIndex);
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);  
        sfxManager.SwitchScene();
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

    public void ChangeVolume()
    {
        sfxManager.ChangeVolume((float)volumeSlider.value);
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

      sfxManager = GameObject.FindGameObjectWithTag("sfxManager").transform.GetComponent<SfxManager>();
      if (sfxManager != null) hasSfxManager = true;

    }

    public void PlayClick(){
      sfxManager.PlayClick();
    }

    public void Update(){
      if (sfxManager != null) hasSfxManager = true;
      else hasSfxManager = false;

      
    }
}
