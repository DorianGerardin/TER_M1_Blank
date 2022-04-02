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
    private int HP=3;
    private int timeSpent=0;

    // Start is called before the first frame update
    void Start()
    {
        wave=1;
        HP=3;
        launchPattern1();       
    }

    // Update is called once per frame
    void Update()
    {   
        if(SceneManager.GetActiveScene().name=="Game"){   
            if(spawner1.isFinished() && spawner2.isFinished()){
                if(timeSpent==60){
                    wave++;
                    Debug.Log("Wave : "+wave);
                    launchPattern1();
                    timeSpent=0;
                }
                else{
                    timeSpent++;
                }
            }
        }
    }

    public void loseHP(){
        HP--;
        //Debug.Log("HP : " + HP);
    }

    public void gameOver(){

    }

    public void launchPattern1(){
        float[] pattern1={1.0F,1.0F,1.0F};
        float[] pattern2={1.5F,1.5F,1.5F};
        for(int i=0;i<3;i++){
            if(wave>1){
                pattern1[i]/=(ratioIncrement*(wave-1));
                pattern2[i]/=(ratioIncrement*(wave-1));
            }
        }
        spawner1.startPattern(pattern1);
        spawner2.startPattern(pattern2);
    }
}
