using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AirplaneController : MonoBehaviour
{
    public float throttleForce = 50f;       // Controls forward thrust
    public float liftForceMultiplier = 2f; // Multiplies lift based on speed
    public float pitchSpeed = 50f;         // Speed for pitch (up/down tilt)
    public float rollSpeed = 50f;          // Speed for roll (side tilt)
    public float yawSpeed = 25f;           // Speed for yaw (left/right turn)

    private Rigidbody rb;

    void Start()
    {
        // Cache Rigidbody reference
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Read joystick input
        float rawThrottle = Input.GetAxis("Throttle"); // Throttle input
        float yawInput = Input.GetAxis("Yaw");         // Yaw input (turn left/right)
        float pitchInput = Input.GetAxis("Pitch");     // Pitch input (nose up/down)
        float rollInput = Input.GetAxis("Roll");       // Roll input (banking)

        // Map throttle input from [-1,1] to [0,1]
        float throttle = Mathf.Clamp01((rawThrottle + 1) / 2);

        // Apply forward thrust (simulate airplane engine)
        rb.AddForce(transform.forward * throttle * throttleForce);

        // Simulate lift (based on speed)
        float liftForce = rb.velocity.magnitude * liftForceMultiplier;
        rb.AddForce(Vector3.up * liftForce);

        // Apply pitch (nose up/down)
        rb.AddTorque(transform.right * pitchInput * pitchSpeed * Time.fixedDeltaTime);

        // Apply roll (banking)
        rb.AddTorque(-transform.forward * rollInput * rollSpeed * Time.fixedDeltaTime);

        // Apply yaw (turning)
        rb.AddTorque(Vector3.up * yawInput * yawSpeed * Time.fixedDeltaTime);
    }
}
