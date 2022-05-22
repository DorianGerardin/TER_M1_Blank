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
    private float timeSpent;
    private float timeBeetween=0.0F;

    // Start is called before the first frame update
    void Start()
    {
        timeSpent=Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(hasPattern){
            if(currentArrayPosition<pattern.Length){
                if(Time.time>=timeSpent+timeBeetween){
                    timeSpent=Time.time;
                    SpawnObject();
                    timeBeetween=pattern[currentArrayPosition];
                    currentArrayPosition++;
                }
            }
            else{
                finishedPattern=true;
                hasPattern=false;
            }
        }
    }

    public void SpawnObject(){
        Instantiate(spawnee.gameObject, transform.position + Vector3.forward, spawnee.gameObject.transform.rotation);
    }

    public void startPattern(float[] p){
        pattern=p;
        currentArrayPosition=0;
        finishedPattern=false;
        hasPattern=true;
        timeSpent=0;
        timeBeetween=pattern[0];
    }

    public bool isFinished(){
        return finishedPattern;
    }

    public void accelerateSpawnSpeed(float s){
        spawnSpeedRatio*=s;
    }
}
