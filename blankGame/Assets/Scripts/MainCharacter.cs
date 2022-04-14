using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainCharacter : MonoBehaviour
{
    private Animator animator;
    private bool punchRight;
    private bool punchLeft;
    private bool kickLeft;
    private bool kickRight;
    List<string> rightAnims = new List<string>();
    List<string> leftAnims = new List<string>();

    private Rigidbody body;
    private BoxCollider boxCollider;

    public GameObject rightCollider;
    public GameObject leftCollider;
    private GameObject currentCollider;
    public GameObject rightSpawner, leftSpawner;
    private float distanceRspawner, distanceLspawner;

    public float punchSpeed = 80f;
    public float punchDuration = 0.1f;
    public bool isPunching = false;
    private float punchStartTime = 0f;
    private Vector3 direction;
    public float timeToWaitUntilPunch = 0.12f;
    private float stunTime = 1f;

    private Vector3 rightRotation;
    private Vector3 leftRotation;

    public int healthPoints = 3;
    public bool hitEnemy;
    private bool stuned;

    public Camera mainCam;
    private SfxManager sfxManager;

    void Awake() {
        body = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();
        rightAnims = new List<string>();
        leftAnims = new List<string>();
        sfxManager = GameObject.FindGameObjectWithTag("sfxManager").transform.GetComponent<SfxManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rightRotation = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y,
            transform.eulerAngles.z
        );

        leftRotation = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y + 180,
            transform.eulerAngles.z
        );

        bool punchRight = animator.GetBool("PunchRight");
        bool punchLeft = animator.GetBool("PunchLeft");
        bool kickLeft = animator.GetBool("KickLeft");
        bool kickRight = animator.GetBool("KickRight");

        rightAnims.Add("PR");
        rightAnims.Add("KR");
        leftAnims.Add("PL");
        leftAnims.Add("KL");

        hitEnemy = false;
        stuned = false;

        distanceRspawner = Mathf.Abs(rightSpawner.transform.position.x - transform.position.x);
        distanceLspawner = Mathf.Abs(leftSpawner.transform.position.x - transform.position.x);
    }

    // Update is called once per frame
    void Update()
    {
        mainCam.transform.position = new Vector3(transform.position.x, mainCam.transform.position.y, mainCam.transform.position.z);
        //rightSpawner.transform.position = new Vector3(transform.position.x + distanceRspawner, rightSpawner.transform.position.y, rightSpawner.transform.position.z);
        //leftSpawner.transform.position = new Vector3(transform.position.x - distanceLspawner, leftSpawner.transform.position.y, leftSpawner.transform.position.z);

        //Debug.Log("stuned : " + stuned);

        if(isPunching) {

            currentCollider = direction.x == 1 ? rightCollider : leftCollider;

            if(Time.time >= punchStartTime + punchDuration || hitEnemy){

                //Debug.Log("hit ennemy : " + hitEnemy);

                if(!hitEnemy) {
                    StartCoroutine("Stun");
                }

                hitEnemy = false;
                if (direction.x == 1) animator.SetBool("PunchRight", false);
                else animator.SetBool("PunchLeft", false);
                // animator.SetBool("KickRight", false);
                // animator.SetBool("KickLeft", false);
                currentCollider.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                isPunching = false;
                body.velocity = Vector3.zero;
            }
            else {
                currentCollider.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                Collider[] hitColliders = Physics.OverlapSphere(currentCollider.transform.position, 0.5f);
                foreach (var hitCollider in hitColliders) {
                    if(hitCollider.tag == "Enemy" && hitCollider.name != transform.name){ 
                        //Debug.Log("je touche");
                        hitEnemy = true;
                        sfxManager.PunchOnCollision();
                        hitCollider.transform.GetComponent<Ennemy>().takeDamage();
                    }
                }
            }
        }
    }

    public void StartPunch(InputAction.CallbackContext context)
    {
        if(!isPunching && context.started && !stuned) {
            Debug.Log("context duration : " + context.duration);
            Vector2 inputVector = context.ReadValue<Vector2>();
            inputVector = Vector3.ClampMagnitude(inputVector, 1);
            direction = new Vector3(inputVector.x, inputVector.y, 0);
            object[] parms = new object[1]{direction};
            StartCoroutine("Punch", parms);
        }
    }

    IEnumerator Punch(object[] parms)
    {
        Vector3 direction = (Vector3)parms[0];
        if(direction.x == 1) {
            body.rotation = Quaternion.Euler(rightRotation);
            //setRandomRightAnimation();
            animator.SetBool("PunchRight", true);
        }
        else {
            body.rotation = Quaternion.Euler(leftRotation);
            //setRandomLeftAnimation();
            animator.SetBool("PunchLeft", true);
        }

        yield return new WaitForFixedUpdate();

        isPunching = true;
        punchStartTime = Time.time;
        body.AddForce(direction * punchSpeed * 200, ForceMode.Acceleration);

        yield return null;
    }

    IEnumerator Stun()
    {   
        sfxManager.PunchNoCollision();
        stuned = true;
        yield return new WaitForSeconds(stunTime);
        stuned = false;
        yield return null;
    }

    public Rigidbody getBody() {
        return body;
    }

    public void takeDamage() {
        this.healthPoints--;
    }

    private void setRandomRightAnimation() {
        int randomIndex = Random.Range(0, rightAnims.Count);
        Debug.Log(randomIndex);
        string value = rightAnims[randomIndex];
        switch(value) {
            case "PR":
                animator.SetBool("PunchRight", true);
                break;
            case "KR":
                animator.SetBool("KickRight", true);
                break;
            default:
                break;
        }
    }

    private void setRandomLeftAnimation() {
        int randomIndex = Random.Range(0, leftAnims.Count);
        Debug.Log(randomIndex);
        string value = leftAnims[randomIndex];
        switch(value) {
            case "PL":
                animator.SetBool("PunchLeft", true);
                break;
            case "KL":
                animator.SetBool("KickLeft", true);
                break;
            default:
                break;
        }
    }
}
