using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemySpawn : MonoBehaviour
{

    public GameObject spawnee;
    private Ennemy enemy;

    public bool stopSpawn=false;
    public float spawnTime;
    public float spawnDelay;
    private float speed=50.0F;

    // Start is called before the first frame update
    void Start()
    {
        enemy=spawnee.GetComponent<Ennemy>();
        enemy.setSpeed(speed);
        InvokeRepeating("SpawnObject",spawnTime,spawnDelay);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnObject(){
        
        if(stopSpawn){
            CancelInvoke("SpawnObject");
        }
        else{
            enemy.setSpeed(speed);
            Instantiate(spawnee, transform.position, spawnee.transform.rotation);
        }
    }

    public void setSpeed(float s){
        speed=s;   
        CancelInvoke("SpawnObject");
        InvokeRepeating("SpawnObject",1,spawnDelay);  
    }

    public void setDelay(float d){
        spawnDelay=d;
        CancelInvoke("SpawnObject");
        InvokeRepeating("SpawnObject",1,spawnDelay); 
    }
}
