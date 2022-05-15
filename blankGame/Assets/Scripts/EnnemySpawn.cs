using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnnemySpawn : MonoBehaviour
{

    public Ennemy spawnee;
    private float spawnSpeedRatio;
    private float[] pattern;
    private int currentArrayPosition;
    private bool hasPattern=false;
    private bool finishedPattern;
    private int timeSpent=0;
    private float timeBeetween=0.0F;
    private float deltaTime = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        deltaTime += (Time.deltaTime);
        deltaTime/=2.0F;
        float fps = 1.0f / deltaTime;

        // Debug.Log("fps : " + fps);

        if(hasPattern){
            if(currentArrayPosition<pattern.Length){
                if(timeSpent>=timeBeetween){
                    timeBeetween=pattern[currentArrayPosition]*fps;
                    timeSpent=0;
                    Invoke("SpawnObject",pattern[currentArrayPosition]);
                    currentArrayPosition++;
                }
                else{
                    timeSpent++;
                }
            }
            else{
                finishedPattern=true;
                hasPattern=false;
            }
        }
    }

    public void SpawnObject(){
        //Debug.Log("je spawn");
        Instantiate(spawnee.gameObject, transform.position + Vector3.forward, spawnee.gameObject.transform.rotation);
    }

    public void startPattern(float[] p){
        pattern=p;
        currentArrayPosition=0;
        finishedPattern=false;
        hasPattern=true;
        timeSpent=0;
        timeBeetween=0.0F;
    }

    public bool isFinished(){
        return finishedPattern;
    }

    public void accelerateSpawnSpeed(float s){
        spawnSpeedRatio*=s;
    }
}
