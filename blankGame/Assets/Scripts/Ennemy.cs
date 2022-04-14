using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Ennemy : MonoBehaviour
{

    private MainCharacter target;
    private Rigidbody body;
    public float speed=50.0f;
    private Animator animator;
    private bool punchBool;
    private string punchAnimation;
    public float hitOffset;
    private bool attacking;

    void Awake() {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<MainCharacter>();

        attacking = false;

        if(this.name == "EnnemyRight(Clone)") {
            hitOffset = 18f;
            punchBool = animator.GetBool("PunchLeft"); 
            punchAnimation = "PunchLeft";
        } else if(this.name == "EnnemyLeft(Clone)")  {
            hitOffset = 18f;
            punchBool = animator.GetBool("PunchRight");
            punchAnimation = "PunchRight";
        }
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log("kinematic : " + target.getBody().isKinematic);

        if((Mathf.Abs(body.position.x - target.getBody().position.x) <= hitOffset) && !target.isPunching && !attacking) {
            attacking = true;
            //updateMainCharacterKinematic();
            //target.getBody().isKinematic = true;
            animator.SetBool(punchAnimation, true);
            Debug.Log("enemy attack");
            body.velocity = Vector3.zero;
            target.Invoke("takeDamage", 0.6f);
            Destroy(this.gameObject, 0.7f);
            //Invoke("updateMainCharacterKinematic", 0.7f);
            //StartCoroutine("OnAnimationComplete", punchAnimation);
            //target.getBody().isKinematic = false;
        } else if(attacking) body.velocity = Vector3.zero;
        else body.velocity = (Quaternion.Euler(0,transform.eulerAngles.y,0) * (Vector3.forward) * speed);
        
    }

    public void updateMainCharacterKinematic() {
        target.getBody().isKinematic = !target.getBody().isKinematic;
    }

    public void setTarget(MainCharacter t){
    	target=t;
    }

    public void setSpeed(float s){
	    speed=s;
    }

    public void takeDamage() {
        Destroy(this.gameObject);
    }

    IEnumerator OnAnimationComplete(string name)
    {
        Debug.Log("nom animation :" + name);

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            //wait
            Debug.Log("l'animation pas terminée");
            yield return null;
        }

        Debug.Log("je destroy");
        Destroy(this.gameObject, 0.6f);

        yield return null;
    }

    // void OnCollisionEnter(Collision col){
    //     if(col.gameObject.name == "mainCharacter"){
    //         //col.rigidbody.isKinematic = true;
    //         //Destroy(this.gameObject);
    //         //col.rigidbody.isKinematic = false;
    //     }
    // }
}
