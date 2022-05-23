using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    private float stunTime = 0.5f;

    private Vector3 rightRotation;
    private Vector3 leftRotation;

    public int healthPoints = 3;
    public int maxHealthPoints = 3;
    public bool hitEnemy;
    public bool stuned;

    public Camera mainCam;
    private SfxManager sfxManager;
    public TimeManager timeManager;

    private Vector3 defaultCamPos;

    public ParticleSystem particleSystem;
    public ParticleSystem takeDamageParticleSystem;

    public Image damageEffectImage;

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

        defaultCamPos = mainCam.transform.position;

        animator.applyRootMotion = false;
    }

    // Update is called once per frame
    void Update()
    {

        mainCam.transform.position = new Vector3(transform.position.x, defaultCamPos.y, defaultCamPos.z);
        
        Vector3 distanceCam = (transform.position - defaultCamPos).normalized;

        if(isPunching) {
            currentCollider = direction.x == 1 ? rightCollider : leftCollider;

            if(Time.time >= punchStartTime + punchDuration || hitEnemy){

                if (direction.x == 1) animator.SetBool("PunchRight", false);
                else animator.SetBool("PunchLeft", false);
                body.velocity = Vector3.zero;
            }

            if(Time.time >= punchStartTime + punchDuration + 0.15f || hitEnemy){

                if(!hitEnemy) {
                    StartCoroutine("Stun");
                }
                currentCollider.GetComponent<Renderer>().material.SetColor("_Color", new Color32(42, 234, 247, 80));
                hitEnemy = false;
                isPunching = false;
            } 
            else {
                timeManager.RevertBackTime();
                currentCollider.GetComponent<Renderer>().material.SetColor("_Color", new Color32(42, 234, 247, 255));
                Collider[] hitColliders = Physics.OverlapSphere(currentCollider.transform.position, 0.5f);
                foreach (var hitCollider in hitColliders) {
                    if(hitCollider.tag == "Enemy" && hitCollider.name != transform.name &&  ! hitCollider.transform.GetComponent<Ennemy>().isDead()){ 

                        body.velocity = Vector3.zero;
                        body.angularVelocity = Vector3.zero;

                        Debug.Log("hit ennemy : " + hitEnemy);
                        hitEnemy = true;
                        sfxManager.PunchOnCollision();
                        hitCollider.transform.GetComponent<Ennemy>().takeDamage();
                        
                        timeManager.SlowDownTimeInstantly(0.2F, 0.04F);

                        var particleSystemPosition = particleSystem.transform.position;
                        particleSystemPosition = hitCollider.transform.position;
                        particleSystemPosition.x += direction.x == 1 ? 6f : -6f ;
                        particleSystemPosition.y = currentCollider.transform.position.y;
                        particleSystem.transform.position=particleSystemPosition;
                        var em = particleSystem.emission; 
                        em.enabled = true;
                        particleSystem.Play();

                    }
                }
            }
        }
    }

    public void StartPunch(InputAction.CallbackContext context)
    {
        if(!isPunching && context.started && !stuned) {
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
        if (body.velocity.x<=0.1f){
            body.AddForce(direction * punchSpeed * 200, ForceMode.Acceleration);
        }

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
        var particleSystemPosition = takeDamageParticleSystem.transform.position;
        particleSystemPosition = transform.position;
        particleSystemPosition.x += direction.x == 1 ? 1f : -1f ;
        particleSystemPosition.y = transform.position.y + 6f;
        takeDamageParticleSystem.transform.position=particleSystemPosition;
        var em = takeDamageParticleSystem.emission; 
        em.enabled = true;
        StartCoroutine(PlayDamageEffectImage(0.5f));
        takeDamageParticleSystem.Play();
        sfxManager.Yell();
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

    IEnumerator PlayDamageEffectImage(float timeToFade)
    {
        float timeElapsed = 0;
        float a;
        damageEffectImage.color = new Color(1f, 0f, 0f, 0.4f);
        while (timeElapsed < timeToFade){

            a = Mathf.Lerp(0.4f, 0f ,  timeElapsed / timeToFade);
            damageEffectImage.color = new Color(1f, 0f, 0f, a);
            
            timeElapsed += Time.fixedDeltaTime;
            
            yield return null;
        }
        damageEffectImage.color = new Color(1f, 0f, 0f, 0f);
    }
}
