using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainCharacter : MonoBehaviour
{
    private Animator animator;
    private bool punchRight;
    private bool punchLeft;

    private Rigidbody body;
    private Vector3 defaultColliderSize;
    private Vector3 defaultColliderCenter;
    private BoxCollider boxCollider;

    private float dashSpeed = 12f;
    private float dashTime = 0.2f;
    private bool isDashing = false;
    private float dashStartTime = 0f;
    private Vector3 direction;

    private Vector3 rightRotation;
    private Vector3 leftRotation;

    public Camera mainCam;

    void Awake() {
        body = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();
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

        defaultColliderCenter = boxCollider.center;
        defaultColliderSize = boxCollider.size;
    }

    // Update is called once per frame
    void Update()
    {
        mainCam.transform.position = new Vector3(transform.position.x, mainCam.transform.position.y,  mainCam.transform.position.z);

        if(isDashing) {
            if (direction.x == 1) Punch(true, direction);
            if (direction.x == -1) Punch(false, direction);

            if(Time.time >= dashStartTime + dashTime){
                animator.SetBool("PunchRight", false);
                animator.SetBool("PunchLeft", false);
                isDashing = false;
                body.velocity = Vector3.zero;
                boxCollider.center = defaultColliderCenter;
                boxCollider.size = defaultColliderSize;
            }
        }
    }

    public void StartDash(InputAction.CallbackContext context)
    {
        if(!isDashing && context.started) {
            isDashing = true;
            dashStartTime = Time.time;
            Vector2 inputVector = context.ReadValue<Vector2>();
            inputVector = Vector3.ClampMagnitude(inputVector, 1);
            direction = new Vector3(inputVector.x, inputVector.y, 0);
        }
    }

    private void Punch(bool right, Vector3 direction) {
        if(direction.x == 1) transform.rotation = Quaternion.Euler(rightRotation);
        if(direction.x == -1) transform.rotation = Quaternion.Euler(leftRotation);

        body.velocity = direction * dashSpeed * ((Time.time - dashStartTime)*dashSpeed);
        
        if(right) {
            boxCollider.center = new Vector3(boxCollider.center.x, boxCollider.center.y, 0.045f);
            boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y, 0.15f);
            animator.SetBool("PunchRight", true);
        }
        else {
            boxCollider.center = new Vector3(boxCollider.center.x, boxCollider.center.y, 0.045f);
            boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y, 0.15f);
            animator.SetBool("PunchLeft", true);
        }
    }
}
