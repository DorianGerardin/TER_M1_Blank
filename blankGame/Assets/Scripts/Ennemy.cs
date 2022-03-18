using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Ennemy : MonoBehaviour
{

    public GameObject target;
    private Vector3 path;
    public int step=0;
    public float speed=128.0f;

    // Start is called before the first frame update
    void Start()
    {
        path = Quaternion.Euler(0, -transform.eulerAngles.y, 0) * (target.transform.position-transform.position);
        Debug.Log(target.transform.eulerAngles.y);
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
