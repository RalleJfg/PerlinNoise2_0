using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AirplaneController : MonoBehaviour
{
    [Header("Engine & Thrust")]
    public float maxThrust = 5000f;          // Maximum engine thrust
    public float throttleSpeed = 1f;        // Speed of throttle adjustment (units per second)

    [Header("Aerodynamics")]
    public float liftCoefficient = 1.5f;    // Lift force multiplier
    public float dragCoefficient = 0.025f; // Drag force multiplier
    public float wingArea = 10f;            // Wing area (affects lift)

    [Header("Control Surfaces")]
    public float pitchTorque = 500f;        // Torque for pitch (nose up/down)
    public float rollTorque = 300f;         // Torque for roll (banking)
    public float yawTorque = 200f;          // Torque for yaw (turning)

    private Rigidbody rb;
    private float currentThrottle = 0f;     // Current throttle value (0 to 1)

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
    }

    void FixedUpdate()
    {
        // Read throttle input
        float targetThrottle = Mathf.Clamp01((Input.GetAxis("Throttle") + 1) / 2); // Map to [0, 1]

        // Gradually adjust throttle to target value
        currentThrottle = Mathf.MoveTowards(currentThrottle, targetThrottle, throttleSpeed * Time.fixedDeltaTime);

        // Apply forward thrust based on current throttle
        Vector3 thrust = transform.forward * currentThrottle * maxThrust;
        rb.AddForce(thrust);

        // Aerodynamic Lift
        float airspeed = Vector3.Dot(rb.velocity, transform.forward); // Forward velocity
        float dynamicPressure = 0.5f * airspeed * airspeed;
        float liftForce = liftCoefficient * dynamicPressure * wingArea;
        Vector3 lift = transform.up * liftForce;
        rb.AddForce(lift);

        // Aerodynamic Drag
        float dragForce = dragCoefficient * dynamicPressure * wingArea;
        Vector3 drag = -rb.velocity.normalized * dragForce;
        rb.AddForce(drag);

        // Control inputs
        float pitchInput = Input.GetAxis("Pitch"); // [-1, 1]
        float rollInput = Input.GetAxis("Roll");   // [-1, 1]
        float yawInput = Input.GetAxis("Yaw");     // [-1, 1]

        // Apply control torques
        Vector3 pitchTorqueVector = transform.right * pitchInput * pitchTorque;
        Vector3 rollTorqueVector = transform.forward * rollInput * rollTorque;
        Vector3 yawTorqueVector = transform.up * yawInput * yawTorque;

        rb.AddTorque(pitchTorqueVector);
        rb.AddTorque(-rollTorqueVector);
        rb.AddTorque(yawTorqueVector);

        // Optional: Stabilize rotation
        Stabilize();
    }

    void Stabilize()
    {
        float stabilizationFactor = 0.1f; // Adjust for desired stability
        rb.angularVelocity *= (1 - stabilizationFactor); // Reduce rotational speed
    }
}
