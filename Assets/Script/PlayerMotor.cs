using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private const float LANE_DISTANE = 2.5f;
    private const float TURN_SPEED = 0.05f;

    private bool isRuning = false;

   //animation
   private Animator anim;



    // movements
    private CharacterController controller;
    private float jumpforce = 10.0f;
    private float gravity = 12.0f;
    private float verticalVelocity;
    private int desiredLane = 1;


    // speed modifires
    private float originalSpeed = 7.0f;
    private float speed = 7.0f;
    private float speedIncreaseLastTick;
    private float speedIncreaseTime = 2.5f;
    private float speedIncreaseAmount = 0.1f;


   
    public void Start()
    {
        speed = originalSpeed;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

   
    }

    private void Update()
    {

        if (!isRuning)
            return;

        if (Time.time -speedIncreaseLastTick> speedIncreaseTime)
        {
            speedIncreaseLastTick = Time.time;
            speed += speedIncreaseAmount;
            GameManager.Instance.updateModifier(speed -originalSpeed);

        }
        
        //gather the input on which lane we should be
        if (MobileInput.Instance.SwipeLeft)
            MoveLane(false);
        if (MobileInput.Instance.SwipeRight)
            MoveLane(true);

        //calculate where we should be 
  
        Vector3 targetPosition = transform.position.z * Vector3.forward;


        if (desiredLane == 0)
            targetPosition += Vector3.left * LANE_DISTANE;
        else if (desiredLane == 2)
            targetPosition += Vector3.right * LANE_DISTANE;

        //lets calculate our move delta
        Vector3 moveVector = Vector3.zero;
        moveVector.x = (targetPosition - transform.position).normalized.x * speed;

        bool isGrounded = IsGrounded();
        anim.SetBool("Grounded", isGrounded);

        if (isGrounded)
        {
            verticalVelocity = -0.1f;

            if (MobileInput.Instance.SwipeUp)
            {
                anim.SetTrigger("Jump");
                verticalVelocity = jumpforce;

            }
            else if (MobileInput.Instance.SwipeDown)
            {
                StartSliding();
                Invoke("StopSliding", 1.0f);
            }
        }
        else
        {
            verticalVelocity -= (gravity * Time.deltaTime);

            if (MobileInput.Instance.SwipeDown)
            {
                verticalVelocity = -jumpforce;
            }
        
        }
        moveVector.y = verticalVelocity;
        moveVector.z = speed;

        //move the pengu
        controller.Move(moveVector * Time.deltaTime);


        //rotate the penu to where it is going 
        Vector3 dir = controller.velocity;
        if (dir != Vector3.zero)
        {
            dir.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, dir, TURN_SPEED);
        }
    }


    private void MoveLane(bool goingRight)
    {
        desiredLane += (goingRight) ? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);

    }
    private bool IsGrounded()
    {
        
        Ray groundRay = new Ray(new Vector3(controller.bounds.center.x, (controller.bounds.center.y - controller.bounds.extents.y) + 0.2f, controller.bounds.center.z), Vector3.down);

        Debug.DrawRay(groundRay.origin, groundRay.direction, Color.blue, 1.0f);


        return Physics.Raycast(groundRay, 0.2f + 0.1f);

    }

    public void StartRunning()
    {
        isRuning = true;
        anim.SetTrigger("StartRunning");
    }

    private void StartSliding()
    {
        anim.SetBool("Sliding",true);
        controller.height /= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y / 2, controller.center.z);
    }

    private void StopSliding()
    {
        anim.SetBool("Sliding", false);
        controller.height *= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y * 2, controller.center.z);
    }
    
    private void Crash()
    {
        anim.SetTrigger("Death");
        isRuning = false;
        GameManager.Instance.OnDeath();
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Obstacle":
                Crash();
                break;
        }
    }
}


