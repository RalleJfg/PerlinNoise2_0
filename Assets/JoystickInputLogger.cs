using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickInputLogger : MonoBehaviour
{
    void Update()
{
    Debug.Log($"Throttle: {Input.GetAxis("Throttle")}");
    Debug.Log($"Yaw: {Input.GetAxis("Yaw")}");
    Debug.Log($"Pitch: {Input.GetAxis("Pitch")}");
    Debug.Log($"Roll: {Input.GetAxis("Roll")}");
}
}
