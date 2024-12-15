using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FPVDroneController : MonoBehaviour
{
    public float throttleForce = 10f;        // Altitude control force
    public float yawSpeed = 100f;           // Speed of yaw turning
    public float pitchSpeed = 50f;          // Forward/backward tilting speed
    public float rollSpeed = 50f;           // Side tilting speed

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Cache Rigidbody reference
    }

    void FixedUpdate()
    {
        // Read joystick input
        float rawThrottle = Input.GetAxis("Throttle"); // Get altitude control input
        float yawInput = Input.GetAxis("Yaw");         // Get yaw input (turn left/right)
        float pitchInput = Input.GetAxis("Pitch");     // Get pitch input (forward/backward tilt)
        float rollInput = Input.GetAxis("Roll");       // Get roll input (sideways tilt)

        // Map throttle input from [-1,1] to [0,1]
        float throttle = Mathf.Clamp01((rawThrottle + 1) / 2);

        // Apply force in the drone's local upward direction
        rb.AddForce(transform.up * throttle * throttleForce);

        // Apply yaw rotation (turning left or right)
        rb.AddTorque(Vector3.up * yawInput * yawSpeed * Time.fixedDeltaTime);

        // Apply pitch rotation (tilting forward/backward)
        rb.AddTorque(transform.right * pitchInput * pitchSpeed * Time.fixedDeltaTime);

        // Apply roll rotation (tilting side to side)
        rb.AddTorque(-transform.forward * rollInput * rollSpeed * Time.fixedDeltaTime);
    }
}
