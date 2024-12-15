using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject canvas;
    public Animator animator;
    public bool paused = false;
    public bool controller = false;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!canvas.activeInHierarchy)
        {
            canvas.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            OpenSettings();
        }

        CheckForController();
        

    }

    void CheckForController()
    {
        float deadZone = 0.2f;

        // Detect significant movement on controller axes
        bool controllerAxisUsed =
            Mathf.Abs(Input.GetAxis("Horizontal")) > deadZone ||
            Mathf.Abs(Input.GetAxis("Vertical")) > deadZone;

        // Check for joystick button presses
        bool controllerButtonPressed = false;
        for (int i = 0; i < 20; i++) // Checking up to 20 buttons
        {
            if (Input.GetKey(KeyCode.JoystickButton0 + i))
            {
                controllerButtonPressed = true;
                break;
            }
        }

        // Final controller usage check
        if (controllerAxisUsed || controllerButtonPressed)
        {
            Debug.Log("Controller is being used!");
        }
        else if (Input.anyKey)
        {
            Debug.Log("Keyboard or Mouse is being used!");
        }
    }

    

    public void OpenSettings()
    {
        paused = !paused;
        
        animator.SetTrigger("Settings");
    }
}
