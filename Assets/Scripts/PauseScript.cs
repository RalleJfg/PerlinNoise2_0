using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public static PauseScript instance;
    public GameObject[] playerObjects;
    public List<Rigidbody> playerRigidbodies = new List<Rigidbody>();

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        PauseStart();
    }
    public void PauseStart()
    {
        // Cache all objects with the tag "Player" at the start
        playerObjects = GameObject.FindGameObjectsWithTag("Player");
        
        // Cache all their rigidbodies
        foreach (GameObject obj in playerObjects)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                playerRigidbodies.Add(rb);
            }
        }
    }

    void Update()
    {
        if (GameManager.instance.paused)
        {
            DisableScriptsOnPlayers();
            PausePhysics();
        }
        else
        {
            EnableScriptsOnPlayers();
            ResumePhysics();
        }
    }

    void DisableScriptsOnPlayers()
    {
        foreach (GameObject obj in playerObjects)
        {
            MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = false;
            }
        }
    }

    void EnableScriptsOnPlayers()
    {
        foreach (GameObject obj in playerObjects)
        {
            MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = true;
            }
        }
    }

    void PausePhysics()
    {
        foreach (Rigidbody rb in playerRigidbodies)
        {
            if (rb != null)
            {
                rb.isKinematic = true; // Stop physics simulation
            }
        }
    }

    void ResumePhysics()
    {
        foreach (Rigidbody rb in playerRigidbodies)
        {
            if (rb != null)
            {
                rb.isKinematic = false; // Resume physics simulation
            }
        }
    }
}
