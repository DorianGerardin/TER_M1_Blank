using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class TimeManager: MonoBehaviour
{

    [Tooltip ("The altered time scale. ")]
    public float timeScale;
    private MainCharacter mainCharacter;

    bool timeToggle = false;

    float defaultTimeScale;
    float defaultFixedDeltaTime;

    void Start()
    {
        mainCharacter = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<MainCharacter>();

        defaultTimeScale = Time.timeScale;
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    void Update() {
        
    }

    public void SlowDownTimeInstantly(float newTimeSpeed = 0.5f, float time = 0.5f){
        Time.timeScale = newTimeSpeed;
        Invoke("RevertBackTime", time);
    }
    public void RevertBackTime(){
        Time.timeScale = 1;
    }
    public void SlowDownTime(float timeToFade = 3){
        StartCoroutine(FadeToLag(timeToFade));   
    }

    private IEnumerator FadeToLag(float timeToFade){
        float timeElapsed = 0;
        float slowDown = timeToFade * 0.15f;
        float speedUp = timeToFade * 0.85f;
        while (timeElapsed < slowDown){
            if (timeToggle)
                Time.timeScale = Mathf.Lerp(defaultTimeScale , timeScale , timeElapsed / slowDown);
            else
                Time.timeScale = Mathf.Lerp(timeScale , defaultTimeScale, timeElapsed / slowDown);
            timeElapsed += defaultFixedDeltaTime;
            
            yield return null;
        }
        timeToggle = !timeToggle;
        if (!timeToggle){
            StartCoroutine(FadeToLag(speedUp));
            Time.timeScale=1f;
        }
    }

}