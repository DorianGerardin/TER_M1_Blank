using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnnemySpawn : MonoBehaviour
{

    public GameObject spawnee;
    private Ennemy enemy;

    private bool start=false;
    public bool stopSpawn=false;
    public float spawnTime;
    public float spawnDelay;
    private float speed=50.0F;

    // Start is called before the first frame update
    void Start()
    {
        enemy=spawnee.GetComponent<Ennemy>();
        enemy.setSpeed(speed);
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene()==SceneManager.GetSceneByName("Game") && !start){
            start=true;
            InvokeRepeating("SpawnObject",spawnTime,spawnDelay);        
        }
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
