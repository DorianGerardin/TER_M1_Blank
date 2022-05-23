
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject menuFirstButton, settingsFirstButton;
    public GameObject squareMenuFirstButton, squareSettingsFirstButton;
    public GameObject mainMenu, settingsMenu, mainMenuSquare, settingsMenuSquare;
    
    [SerializeField]  Slider volumeSlider, volumeSliderSquare;

    private SfxManager sfxManager;
    public bool hasSfxManager = false;

    public Image image;
    public Canvas menuCanva;
    public Sprite squarredSprite;
    public Sprite largeSprite;

    public void PlayGame()
    {
      Debug.Log(SceneManager.GetActiveScene().buildIndex);
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);  
      sfxManager.SwitchToPlayScene();
    }

    public void ActivateSettingsMenu()
    {
      mainMenu.SetActive(false);
      mainMenuSquare.SetActive(false);

      if((float)Screen.width / (float)Screen.height < 1.45f ) {
        settingsMenuSquare.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(squareSettingsFirstButton);
      } else {
        settingsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsFirstButton);
      }
    }
    
    public void ActivateMainMenu()
    {
      settingsMenu.SetActive(false);
      settingsMenuSquare.SetActive(false);

      if((float)Screen.width / (float)Screen.height < 1.45f ) {
        mainMenuSquare.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(squareMenuFirstButton);

      } else {
        mainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(menuFirstButton);
      }
    }

    public void ChangeVolume()
    {
      
      sfxManager = GameObject.FindGameObjectWithTag("sfxManager").transform.GetComponent<SfxManager>();
      if((float)Screen.width / (float)Screen.height < 1.45f ) {
        sfxManager.ChangeVolume((float)volumeSliderSquare.value);
        PlayerPrefs.SetFloat("Volume", (float)volumeSliderSquare.value);
      } else {
        sfxManager.ChangeVolume((float)volumeSlider.value);
        PlayerPrefs.SetFloat("Volume", (float)volumeSlider.value);
      }
        
    }

    public void QuitGame()
    {
      Debug.Log("Quit");
      Application.Quit();
    }

    public void Awake()
    {
      if((float)Screen.width / (float)Screen.height < 1.45f ) {
        mainMenuSquare.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(squareMenuFirstButton);

      } else {
        mainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(menuFirstButton);
      }

      sfxManager = GameObject.FindGameObjectWithTag("sfxManager").transform.GetComponent<SfxManager>();
      if (sfxManager != null) hasSfxManager = true;
      
      if((float)Screen.width / (float)Screen.height < 1.45f ) {       
        volumeSliderSquare.value = PlayerPrefs.GetFloat("Volume", 0.47f);
      } else {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.47f);
      }
      ChangeVolume();
    }

    public void PlayClick(){
      sfxManager = GameObject.FindGameObjectWithTag("sfxManager").transform.GetComponent<SfxManager>();
      sfxManager.PlayClick();
    }

    public void Update(){
      if (sfxManager != null) hasSfxManager = true;
      else hasSfxManager = false;
      if((float)Screen.width / (float)Screen.height < 1.45f ) {
        image.sprite = squarredSprite;
      } else {
        image.sprite = largeSprite;
      }
    }
}
