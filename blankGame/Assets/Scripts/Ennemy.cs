using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Ennemy : MonoBehaviour
{

    public GameObject target;
    private Rigidbody body;
    private float speed=50.0f;

    // Start is called before the first frame update
    void Start()
    {
	body=GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        body.velocity = (Quaternion.Euler(0,transform.eulerAngles.y,0) * Vector3.forward * speed);
    }

    public void setTarget(GameObject t){
    	target=t;
    }

    public void setSpeed(float s){
	    speed=s;
    }

    void OnCollisionEnter(Collision col){
	if(col.gameObject.name == "mainCharacter"){
		Destroy(this.gameObject);
	}
    }
}
