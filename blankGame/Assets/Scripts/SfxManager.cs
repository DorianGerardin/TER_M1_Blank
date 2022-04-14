using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;


public class SfxManager : MonoBehaviour
{
    
    public AudioClip MenuAudioMusic;
    public AudioClip GameAudioMusic;
    public AudioClip ClickAudio;
    public AudioClip PunchCollisionAudio;
    public AudioClip PunchAirAudio;
    public static SfxManager sfxInstance;

    [SerializeField] Slider volumeSlider;

    [Range(0f, 3f)]
    public float pitchGame = 0.1f;
    [Range(0f, 3f)]
    public float pitchMenu = 1f;

    [Range(0f, 1f)]
    public float volumeMenu = 0.6f;
    [Range(0f, 1f)]
    public float volumeGame = 1f;


    private AudioSource Audio;
    private AudioSource AudioMenu;  
    private AudioSource AudioGame;  
    private AudioSource AudioEffects; 
    private bool isPlayingGame;

    public void ChangeVolume()
    {
        // Debug.Log("volume changed to " + volumeSlider.value );
        AudioMenu.volume = volumeSlider.value * volumeMenu;
        AudioGame.volume = volumeSlider.value * volumeGame;
    }

    public void PlayClick()
    {
        PlayClip(ClickAudio);
    }


    public void SpeedUpGameAudio(){
        SpeedUpGameAudio(0.1f); 
    }

    public void SpeedUpGameAudio(float deltaSpeed){
        AudioGame.pitch += deltaSpeed; 
    }

    public void freezeWithPunch(){

    }

    public void PunchOnCollision()
    {
        PlayClip(PunchCollisionAudio);
    }
    
    public void PunchNoCollision()
    {
        // float old_pitch = PunchAirAudio.pitch;
        // PunchAirAudio.pitch = Random.Range(-1.0f, 1.0f);
        PlayClip(PunchAirAudio);
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
                AudioMenu.volume = Mathf.Lerp(0 , volumeMenu * volumeSlider.value, timeElapsed / timeToFade);
                AudioGame.volume = Mathf.Lerp(volumeGame * volumeSlider.value, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            AudioGame.Stop();
        }else{
            AudioGame.Play();
            
            while (timeElapsed < timeToFade){
                AudioGame.volume = Mathf.Lerp(0 , volumeGame * volumeSlider.value, timeElapsed / timeToFade);
                AudioMenu.volume = Mathf.Lerp(volumeMenu * volumeSlider.value, 0, timeElapsed / timeToFade);
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

        AudioMenu = gameObject.AddComponent<AudioSource>();
        AudioMenu.clip = MenuAudioMusic;
        AudioMenu.loop = true;
        AudioMenu.volume = volumeSlider.value * volumeMenu;
        AudioMenu.pitch = pitchMenu;

        AudioGame = gameObject.AddComponent<AudioSource>();
        AudioGame.clip = GameAudioMusic;
        AudioGame.loop = true;
        AudioGame.volume = volumeSlider.value * volumeGame;
        AudioGame.pitch = pitchGame;

        AudioEffects = gameObject.AddComponent<AudioSource>();

        isPlayingGame = false;
    }

    private void PlayClip(AudioClip clip){
        AudioEffects.PlayOneShot(clip);
        // if (isPlayingGame){
        //     AudioGame.PlayOneShot(clip);
        // }else{
        //     AudioMenu.PlayOneShot(clip);
        // }
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
        if (isPlayingGame){
            AudioGame.pitch = Time.timeScale;
        }else{
            AudioMenu.pitch = Time.timeScale;
        }
    }
}
