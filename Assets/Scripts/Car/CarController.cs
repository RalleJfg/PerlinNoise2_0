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
    public TrailRenderer[] whiteTrails;
    public GameObject cloudPrefab;


    public LayerMask whatIsGround;
    public float groundRayLength = 0.5f;
    public Transform groundRayPoint;

    private bool acceleration;
    public float jumpForce = 1500f; // The force to apply when jumping
    public int maxJumps = 2; // Total number of jumps allowed
    public int currentJumps = 0; // Track how many jumps have been used

    //public Transform rotation;

    void Start()
    {
        rb.transform.parent = null;
        CineShakeScript.Instance.ShakeCamera(0, 0);
    }

    void Update()
    {
        // if(GameManager.instance.paused)
        // {
        //     return;
        // }

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


        Accelerate();
        
        RotateCarInAir();
        
        Jump();
    }

    void Jump()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (grounded) // First jump: straight up
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                grounded = false; // Car is no longer grounded
                currentJumps = 0; // First jump used
                Instantiate(cloudPrefab, new Vector3(transform.position.x, transform.position.y + 1.2f, transform.position.z), Quaternion.Euler(transform.rotation.eulerAngles.x - 90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
            }
            else if (!grounded && currentJumps < maxJumps) // Second jump: based on car's orientation
            {
                Vector3 jumpDirection = transform.up; // Use the car's current orientation
                rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
                currentJumps++; // Second jump used
            }
        }
    }

    void RotateCarInAir()
    {
        float input = Input.GetAxis("Vertical");

        if (input != 0 && grounded == false)
        {
            // Calculate rotation change in local space
            float rotationAmount = input * 150f * Time.deltaTime;

            // Rotate around the X-axis by applying local rotation
            transform.Rotate(rotationAmount, 0, 0, Space.Self);
        }

        if(Input.GetAxis("Horizontal") > 0 && grounded == false)
        {
            
            // Define the target rotation
            Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 10f, transform.rotation.eulerAngles.z);

            // Smoothly interpolate to the target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
        else if(Input.GetAxis("Horizontal") < 0 && grounded == false)
        {
            // Define the target rotation
            Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 10f, transform.rotation.eulerAngles.z);

            // Smoothly interpolate to the target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    void Accelerate()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            acceleration = true;
        }
        if(Input.GetMouseButtonUp(0))
        {
            acceleration = false;
        }

        if(acceleration == true && grounded && Mathf.Abs(speedInput) > 0)
        {
            forwardAcc = 16;
            CineShakeScript.Instance.ShakeCamera(3.5f, 99999);
        }
        else if(acceleration == false)
        {
            forwardAcc = 8;
            CineShakeScript.Instance.ShakeCamera(0, 0);
        }
        

        if(Mathf.Abs(speedInput) < 4001)
        {
            CineShakeScript.Instance.ShakeCamera(0, 0);
        }
    }

    void FixedUpdate()
    {
        // if(GameManager.instance.paused)
        // {
        //     return;
        // }
        
        rb.drag = dragOnGround;

        RaycastHit hit;

        if(Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            grounded = true;

            
            
            // Calculate the target rotation based on the surface normal
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

            // Smoothly interpolate towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

            // Reset jumps when the car lands
            currentJumps = 0;
        }
        else
        {
            grounded = false;
            CineShakeScript.Instance.ShakeCamera(0, 0);
        }

        emissionRate = 0;

        if((turnInput == 1 || turnInput == -1) && speedInput == 8000 && grounded)
        {
           
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

        if((speedInput >= 8001))
        {
           
            for(int i = 0; i < whiteTrails.Length; i++)
            {
                whiteTrails[i].emitting = true;
            }
        }
        else
        {
            for(int i = 0; i < whiteTrails.Length; i++)
            {
                whiteTrails[i].emitting = false;
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
