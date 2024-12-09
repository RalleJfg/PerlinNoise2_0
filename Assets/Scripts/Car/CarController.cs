using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CarController : MonoBehaviour
{
    public Rigidbody rb;

    public float forwardAcc = 8f, reverseAcc = 4f, maxSpeed = 50f, turnStrength = 180f, gravityForce = 10f, dragOnGround = 3f;

    public float speedInput, turnInput;

    public bool grounded = false;

    private GameObject groundRotationThingy;  //bilen roterar åt detta hållet
    
    public Transform leftWheel, rightWheel;

    public float maxWheelTurn = 25f;

    public ParticleSystem[] particleSystems;
    public float maxEmission = 25f;
    private float emissionRate;

    public TrailRenderer[] trails;


    public LayerMask whatIsGround;
    public float groundRayLength = 0.5f;
    public Transform groundRayPoint;
    

    void Start()
    {
        rb.transform.parent = null;
    }

    void Update()
    {
        speedInput = 0f;
        if(Input.GetAxis("Vertical") > 0)
        {
            speedInput = Input.GetAxis("Vertical") * forwardAcc * 1000f;
        }
        else if(Input.GetAxis("Vertical") < 0)
        {
            speedInput = Input.GetAxis("Vertical") * reverseAcc * 1000f;
        }

        turnInput = Input.GetAxis("Horizontal");

        if(grounded)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Input.GetAxis("Vertical") , 0f));
        }
        
        leftWheel.localRotation = Quaternion.Euler(leftWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn) - 180, leftWheel.localRotation.eulerAngles.z);
        rightWheel.localRotation = Quaternion.Euler(rightWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, rightWheel.localRotation.eulerAngles.z);


        transform.position = rb.transform.position;


        
    }

  

    void FixedUpdate()
    {
        
        rb.drag = dragOnGround;

        RaycastHit hit;

        if(Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            grounded = true;

            //transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            
            
            // Calculate the target rotation based on the surface normal
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

            // Smoothly interpolate towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1.1f);
        }
        else
        {
            grounded = false;
        }

        emissionRate = 0;

        if((turnInput == 1 || turnInput == -1) && speedInput == 8000 && grounded)
        {
            print("not");
            for(int i = 0; i < trails.Length; i++)
            {
                trails[i].emitting = true;
            }
        }
        else
        {
            for(int i = 0; i < trails.Length; i++)
            {
                trails[i].emitting = false;
            }
        }

        if(grounded)
        {

            if(Mathf.Abs(speedInput) > 0)
            {
                rb.AddForce(transform.forward * speedInput);

                emissionRate = maxEmission;
            }
        }
        else
        {
            rb.drag = 0.1f;
            rb.AddForce(Vector3.up * -gravityForce * 100f);
            
        }

        foreach (ParticleSystem part in particleSystems)
        {
            var emissionModule = part.emission;
            emissionModule.rateOverTime = emissionRate;
        }
        
    }
}
