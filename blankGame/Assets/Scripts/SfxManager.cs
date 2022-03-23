using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;


public class SfxManager : MonoBehaviour
{
    
    public AudioClip MainAudioClip;
    public AudioClip MenuAudio;
    public AudioClip GameAudio;
    public AudioClip ClickAudio;
    public static SfxManager sfxInstance;

    [Range(0f, 3f)]
    public float pitch;

    [Range(1f, 0f)]
    public float volume;


    private AudioSource Audio;   

    

    public void playClick()
    {
        Audio.PlayOneShot(ClickAudio);
    }

    public void setPitch(float pitch)
    {
        this.pitch = pitch;
        // Audio.pitch = pitch;
    }

    public void setVolume(float volume)
    {
        this.volume = volume;
        // Audio.volume = volume;
    }

    //doesn't stop the previous music work
    public void gameAudio(){
        Audio.Stop();
        // Audio.clip = GameAudio;
        // Audio.Play();
    }

    public void menuAudio(){
    //     Audio = MenuAudio;
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

        Audio = gameObject.AddComponent<AudioSource>();
        Audio.clip = MainAudioClip;
    }

    private void Start()
    {
        Play();
    }

    private void Play(){
        Audio.Play();
    }
}
