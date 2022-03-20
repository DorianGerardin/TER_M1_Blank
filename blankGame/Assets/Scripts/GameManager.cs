using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public GameObject spawner1;
    public GameObject spawner2;
    public float speed=50f;
    private int timeSpent=0;
    private int wave=0;
    public float ratioIncrement=2.0F;

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
	    speed*=ratioIncrement;
	    Debug.Log("wave : " + wave);        
        }
    }
}
