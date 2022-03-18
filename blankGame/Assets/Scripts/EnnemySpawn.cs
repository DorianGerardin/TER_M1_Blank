using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemySpawn : MonoBehaviour
{

    public GameObject spawnee;
    public GameObject target;
    private Ennemy enemy;

    public bool stopSpawn=false;
    public float spawnTime;
    public float spawnDelay;
    public float speed=180.0F;

    // Start is called before the first frame update
    void Start()
    {
        enemy = spawnee.GetComponent<Ennemy>();
        InvokeRepeating("SpawnObject",spawnTime,spawnDelay);
        enemy.setTarget(target);
        enemy.setSpeed(speed);
    }

    // Update is called once per frame
    void Update()
    {
        //spawnee.GetComponent<Ennemy>().setSpeed(speed);
    }

    public void SpawnObject(){
        
        if(stopSpawn){
            CancelInvoke("SpawnObject");
        }
        else{
            Instantiate(spawnee, transform.position, transform.rotation);
        }
    }

    public void setSpeed(float s){
	    speed=s;    
    }

    public void setDelay(float d){
        spawnDelay=d;
        CancelInvoke("SpawnObject");
        InvokeRepeating("SpawnObject",1,spawnDelay); 
    }
}
