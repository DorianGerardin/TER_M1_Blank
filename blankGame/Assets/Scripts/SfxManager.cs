using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SfxManager : MonoBehaviour
{
    
    public AudioClip MenuAudioMusic;
    public AudioClip GameAudioMusic;
    public AudioClip ClickAudio;
    public AudioClip PunchCollisionAudio;
    public AudioClip PunchAirAudio;
    public AudioClip YellAudio;
    public static SfxManager sfxInstance;

    [SerializeField] Slider volumeSlider;
    [SerializeField] Slider squareVolumeSlider;
    private float volumeSliderValue ;

    [Range(0f, 3f)]
    public float pitchGame = 1f;
    [Range(0f, 3f)]
    public float pitchMenu = 1f;

    [Range(0f, 1f)]
    public float volumeMenu = 1f;
    [Range(0f, 1f)]
    public float volumeGame = 1f;


    private AudioSource Audio;
    private AudioSource AudioMenu;  
    private AudioSource AudioGame;  
    private AudioSource AudioEffects; 
    private bool isPlayingGame;
    public float gameSpeedMultiplier = 1f;

    public void ChangeVolume(float volumeSliderValue)
    {
        this.volumeSliderValue = volumeSliderValue;
        AudioMenu.volume = volumeSliderValue * volumeMenu;
        AudioGame.volume = volumeSliderValue * volumeGame;
        AudioEffects.volume = volumeSliderValue;
    }

    public void PlayClick()
    {
        PlayClip(ClickAudio);
    }


    public void SpeedUpGameAudio(){
        SpeedUpGameAudio(0.1f); 
    }

    public void SpeedUpGameAudio(float deltaSpeed){
        if (gameSpeedMultiplier +  deltaSpeed < 1.35f ) 
            gameSpeedMultiplier += deltaSpeed; 
        
        Debug.Log("sfxManager SpeedUpGameAudio:" + AudioGame.pitch);
    }

    public void RevertGameAudio(){
        gameSpeedMultiplier = 1; 
    }

    

    public void PunchOnCollision()
    {
        PlayClipRandomPitch(PunchCollisionAudio);
    }
    
    public void PunchNoCollision()
    {
        PlayClipRandomPitch(PunchAirAudio);
    }

    public void Yell()
    {
        PlayClipRandomPitch(YellAudio, 0.5f, 1.5f );
    }

    public void SwitchToPlayScene(){
        isPlayingGame = false;
        SwitchScene();   
    }
    public void SwitchToPlayScene(float timeToFade ){
        isPlayingGame = false;
        SwitchScene(timeToFade);   
    }

    public void SwitchScene(){
        float timeToFade = 3.25f;
        SwitchScene(timeToFade);    
    }

    public void SwitchScene(float timeToFade ){
        StopAllCoroutines();
        StartCoroutine(FadeAudioToOtherScene(timeToFade));      
    }

    private IEnumerator FadeAudioToOtherScene(float timeToFade){
        float timeElapsed = 0;
        if (isPlayingGame){
            AudioMenu.Play();
            
            while (timeElapsed < timeToFade){
                AudioMenu.volume = Mathf.Lerp(0 , volumeMenu * volumeSliderValue, timeElapsed / timeToFade);
                AudioGame.volume = Mathf.Lerp(volumeGame * volumeSliderValue, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            AudioGame.Stop();
        }else{
            AudioGame.Play();
            
            while (timeElapsed < timeToFade){
                AudioGame.volume = Mathf.Lerp(0 , volumeGame * volumeSliderValue, timeElapsed / timeToFade);
                AudioMenu.volume = Mathf.Lerp(volumeMenu * volumeSliderValue, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            AudioMenu.Stop();
        }   
        isPlayingGame = !isPlayingGame;
    }

 
    private void Awake()
    {
        if (sfxInstance != null && sfxInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        sfxInstance = this;
        DontDestroyOnLoad(this);


        volumeSliderValue = (float) PlayerPrefs.GetFloat("Volume", 0.5f);
        // if((float)Screen.width / (float)Screen.height < 1.45f ) {
        //     volumeSliderValue = squareVolumeSlider.value;
        // }else{
        //     // volumeSliderValue = volumeSlider.value;
        //     volumeSlider = GameObject.FindGameObjectWithTag("VolumeSlider").transform.GetComponent<Slider>();
        // }
        
        AudioMenu = gameObject.AddComponent<AudioSource>();
        AudioMenu.clip = MenuAudioMusic;
        AudioMenu.loop = true;
        AudioMenu.volume = volumeSliderValue * volumeMenu;
        AudioMenu.pitch = pitchMenu;

        AudioGame = gameObject.AddComponent<AudioSource>();
        AudioGame.clip = GameAudioMusic;
        AudioGame.loop = true;
        AudioGame.volume = volumeSliderValue * volumeGame;   
        AudioGame.pitch = pitchGame;

        AudioEffects = gameObject.AddComponent<AudioSource>();
        AudioEffects.volume = volumeSliderValue;

        isPlayingGame = false;

        
    }

    private void PlayClip(AudioClip clip){
        AudioEffects.pitch = 1f;
        AudioEffects.PlayOneShot(clip);
    }

    private void PlayClipRandomPitch(AudioClip clip, float min = (float) 0.7, float max = (float) 2.0){
        AudioEffects.pitch = Random.Range(min, max);
        // float oldVOlume = AudioEffects.volume;
        // AudioEffects.volume = oldVOlume * volumeSliderValue;
        AudioEffects.PlayOneShot(clip);
    }




    private void Start()
    {
        Play();
    }

    private void Play(){
        if (isPlayingGame){
            AudioGame.Play();
        }else{
            AudioMenu.Play();
        }
    }

    private void Stop(){
        if (isPlayingGame){
            AudioGame.Stop();
        }else{
            AudioMenu.Stop();
        }
    }

    private void Update(){
        // Debug.Log("volumeSlider", volumeSlider);
        if (isPlayingGame){
            AudioGame.pitch = gameSpeedMultiplier * Time.timeScale;
        }else{
            AudioMenu.pitch = Time.timeScale;
        }
    }
}
