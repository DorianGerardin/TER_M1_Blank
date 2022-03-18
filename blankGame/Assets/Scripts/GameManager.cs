using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public GameObject spawner1;
    public GameObject spawner2;
    public GameObject player;
    public float speed=180.0F;
    public int timeSpent=0;
    public int wave=0;
    public float ratioIncrement=0.9F;

    // Start is called before the first frame update
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        timeSpent++;
	if(timeSpent==300){
	    timeSpent=0;
            wave++;
	    if(speed>30){
	    	speed*=ratioIncrement;
		spawner1.GetComponent<EnnemySpawn>().setSpeed(speed);
		spawner2.GetComponent<EnnemySpawn>().setSpeed(speed);
             	spawner1.GetComponent<EnnemySpawn>().setDelay(spawner1.GetComponent<EnnemySpawn>().spawnDelay*ratioIncrement);
		spawner2.GetComponent<EnnemySpawn>().setDelay(spawner2.GetComponent<EnnemySpawn>().spawnDelay*ratioIncrement);
			    
	    }
	    Debug.Log("wave : " + wave);        
        }
    }
}
