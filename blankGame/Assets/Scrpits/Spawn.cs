using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    public GameObject spawnee;
    public bool stopSpawn=false;
    public float spawnTime;
    public float spawnDelay;

    // Start is called before the first frame update
    void Start()
    {
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
		Instantiate(spawnee, transform.position, transform.rotation);
	}
    }
}
