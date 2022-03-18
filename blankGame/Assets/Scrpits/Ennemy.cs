using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour
{

    public GameObject target;
    public Vector3 path;
    public int step=0;
    public float speed=180.0F;

    // Start is called before the first frame update
    void Start()
    {
        path=target.transform.position-transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(path * (float)1/speed);
	step++;
	if((float)step>=speed){
		//Debug.Log("allo");
		Destroy(this.gameObject);
	}
    }

    public void setTarget(GameObject t){
    	target=t;
    }

    public void setSpeed(float s){
	speed=s;
    }
}
