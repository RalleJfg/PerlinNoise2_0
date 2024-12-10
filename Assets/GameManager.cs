using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject canvas;
    public Animator animator;

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
    }

    public void OpenSettings()
    {
        animator.SetTrigger("Settings");
    }
}