using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public EnnemySpawn spawner1;
    public EnnemySpawn spawner2;
    public float speed=50f;
    private int timeSpent=0;
    private int wave=0;
    public float ratioIncrement=2.0F;
    private int HP=3;

    // Start is called before the first frame update
    void Start()
    {
        HP=3;       
    }

    // Update is called once per frame
    void Update()
    {   
        if(HP>0){
            timeSpent++;
            if(timeSpent==6000){
    	       timeSpent=0;
               wave++;
    	       speed*=ratioIncrement;
               spawner1.spawnDelay/=ratioIncrement;
               spawner2.spawnDelay/=ratioIncrement;
               spawner1.setSpeed(speed);
               spawner2.setSpeed(speed);
    	       Debug.Log("wave : " + wave);        
            }
        }
    }

    public void loseHP(){
        HP--;
        Debug.Log("HP : " + HP);
    }
}
