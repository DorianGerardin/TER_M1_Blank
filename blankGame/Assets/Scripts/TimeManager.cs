using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class TimeManager: MonoBehaviour
{

    [Tooltip ("The altered time scale. ")]
    public float timeScale;


    bool timeToggle = false;

    float defaultTimeScale;
    float defaultFixedDeltaTime;

    void Start()
    {
        gameObject.SetActive(true);
        defaultTimeScale = Time.timeScale;
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void SlowDownTime(float timeToFade){
        StopAllCoroutines();
        StartCoroutine(FadeToLag(timeToFade));   
  
    }

    private IEnumerator FadeToLag(float timeToFade){
        float timeElapsed = 0;
        float timePerInterval = timeToFade/3;
        while (timeElapsed < timePerInterval){
            Debug.Log("time elapsed: " +timeElapsed +"From: " +defaultTimeScale + " ; To: "+ timeScale  + "; Current :" + Time.timeScale);
            if (timeToggle)
                Time.timeScale = Mathf.Lerp(defaultTimeScale , timeScale , timeElapsed / timePerInterval);
            else
                Time.timeScale = Mathf.Lerp(timeScale , defaultTimeScale, timeElapsed / timePerInterval);
            timeElapsed += defaultFixedDeltaTime;
            
            yield return null;
        }
        yield return new WaitForSecondsRealtime(timePerInterval);
        timeToggle = !timeToggle;
        if (!timeToggle)
            StartCoroutine(FadeToLag(timePerInterval));
    }

}