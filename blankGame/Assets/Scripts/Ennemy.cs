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
    private bool dead;

    private string punchAnimation;
    public float hitOffset;
    private bool attacking;

    private Vector3 expulseDirection;
    private GameObject gravityCenter;


    void Awake() {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
        // timeManager = GetComponent<TimeManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<MainCharacter>();

        attacking = false;
        dead = false;
        gravityCenter = transform.Find("gravityCenter").gameObject;

        if(this.name == "EnnemyRight(Clone)") {
            hitOffset = 18f;
            punchBool = animator.GetBool("PunchLeft"); 
            punchAnimation = "PunchLeft";
            expulseDirection = new Vector3(1, 0.6f, 0);
        } else if(this.name == "EnnemyLeft(Clone)")  {
            hitOffset = 18f;
            punchBool = animator.GetBool("PunchRight");
            punchAnimation = "PunchRight";
            expulseDirection = new Vector3(-1, 0.6f, 0);

        }
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("kinematic : " + target.getBody().isKinematic);
        if (!dead){
            if((Mathf.Abs(body.position.x - target.getBody().position.x) <= hitOffset) && !target.isPunching && !attacking) {
                attacking = true;
                //updateMainCharacterKinematic();
                //target.getBody().isKinematic = true;
                animator.SetBool(punchAnimation, true);
                Debug.Log("enemy attack");
                body.velocity = Vector3.zero;
                // target.Invoke("takeDamage", 0.6f);
                Invoke("takeDamageOnPlayer", 0.6f);
                // Destroy(this.gameObject, 0.7f);
                Destroy(this.gameObject, 1.5f);
                //Invoke("updateMainCharacterKinematic", 0.7f);
                //StartCoroutine("OnAnimationComplete", punchAnimation);
                //target.getBody().isKinematic = false;
            } else if(attacking) body.velocity = Vector3.zero;
            else body.velocity = (Quaternion.Euler(0,transform.eulerAngles.y,0) * (Vector3.forward) * speed);
        }
        else{
            Destroy(this.gameObject, 3f);
            transform.RotateAround(gravityCenter.transform.position, new Vector3(0, 0, 1), -360 * 20 * Time.deltaTime);
            // Collider[] hitColliders = Physics.OverlapSphere(gravityCenter.transform.position, 2f);
            // foreach (var hitCollider in hitColliders) {
            //     if(hitCollider.tag == "Enemy"){ 
            //         hitCollider.transform.GetComponent<Ennemy>().takeDamage();
            //     }
            // }
        }
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
        if (!dead){
            this.dead = true;

            float randomOffset = Random.Range(-0.2f, 0.2f);
            // Debug.Log("random : " + randomOffset);
            Vector3 newExpulseDirection = new Vector3(expulseDirection.x, expulseDirection.y + randomOffset, expulseDirection.z);
            body.AddForce(newExpulseDirection * 300, ForceMode.Impulse); 

        }
        // Destroy(this.gameObject);
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

    private void takeDamageOnPlayer(){
        if (!dead) target.takeDamage();
    }

    // void OnCollisionEnter(Collision col){
    //     if(col.gameObject.name == "mainCharacter"){
    //         //col.rigidbody.isKinematic = true;
    //         //Destroy(this.gameObject);
    //         //col.rigidbody.isKinematic = false;
    //     }
    // }

    public bool isDead(){
        return dead;
    } 
}
