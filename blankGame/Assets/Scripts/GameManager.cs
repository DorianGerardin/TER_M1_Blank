using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    
    public EnnemySpawn spawner1;
    public EnnemySpawn spawner2;
    private int wave;
    public float ratioIncrement;
    private int timeSpent=0;
    private long score;
    private int consequentHits;
    private int numberOfPattern=2;

    // Start is called before the first frame update
    void Start()
    {
        score=0;
        consequentHits=0;
        wave=1;
        launchRandomPattern();       
    }

    // Update is called once per frame
    void Update()
    {   
        if(SceneManager.GetActiveScene().name=="Game"){   
            if(spawner1.isFinished() && spawner2.isFinished()){
                if(timeSpent==60){
                    wave++;
                    Debug.Log("Wave : "+wave);
                    launchRandomPattern();
                    timeSpent=0;
                }
                else{
                    timeSpent++;
                }
            }
        }
    }

    public void increaseScore(){
        consequentHits++;
        score+=100*consequentHits*wave;
    }

    public void gameOver(){
        Debug.Log("Game Over, sadge");
    }

    public void launchPattern1(){
        Debug.Log("Launching Pattern 1");
        float[] pattern1={1.0F,1.0F,1.0F};
        float[] pattern2={1.5F,1.5F,1.5F};
        for(int i=0;i<pattern1.Length;i++){
            if(wave>1){
                pattern1[i]/=(ratioIncrement*(wave-1));
                pattern2[i]/=(ratioIncrement*(wave-1));
            }
        }
        spawner1.startPattern(pattern1);
        spawner2.startPattern(pattern2);
    }

    public void launchPattern2(){
        Debug.Log("Launching Pattern 2");
        float[] pattern1={1.0F,2.0F,1.0F};
        float[] pattern2={2.0F,0.5F,1.5F};
        for(int i=0;i<pattern1.Length;i++){
            if(wave>1){
                pattern1[i]/=(ratioIncrement*(wave-1));
                pattern2[i]/=(ratioIncrement*(wave-1));
            }
        }
        spawner1.startPattern(pattern1);
        spawner2.startPattern(pattern2);
    }

    public void launchRandomPattern(){
        int patternNumber=Random.Range(1,numberOfPattern+1);
        Invoke("launchPattern"+patternNumber,0.0F);
    }
}
