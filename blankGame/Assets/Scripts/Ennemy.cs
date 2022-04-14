using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Ennemy : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject timeManager;
    public MainCharacter target;
    private Rigidbody body;
    private float speed=100.0f;

    // Start is called before the first frame update
    void Start()
    {
        //timeManager=
	   body=GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        body.velocity = (Quaternion.Euler(0,transform.eulerAngles.y,0) * Vector3.forward * speed);
    }

    public void setTarget(MainCharacter t){
        target=t;
    }

    public void setSpeed(float s){
	   this.speed=s;
    }

    void OnCollisionEnter(Collision col){
	   if(col.gameObject.name == "mainCharacter"){
          Destroy(this.gameObject); 
	   }
    }
}
