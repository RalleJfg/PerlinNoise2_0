using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    public static VehicleManager Instance;
    public int selectedVehicle;
    public GameObject[] vehicles;
    public GameObject viewer;

    void Awake()
    {
        Instance = this;
        selectedVehicle = PlayerPrefs.GetInt("selectedVehicle", 0);
        
        ChangeVehicle();
    }

    void Update()
    {
        viewer.transform.position = vehicles[selectedVehicle].transform.position;
    }

    void ChangeVehicle()
    {
        for (int i = 0; i < vehicles.Length; i++)
        {
            if (i == selectedVehicle)
            {
                vehicles[i].SetActive(true);
                PauseScript.instance.PauseStart();
            }
            else
            {
                vehicles[i].SetActive(false);
            }
        }
    }

    public void Car()
    {
        selectedVehicle = 0;
        PlayerPrefs.SetInt("selectedVehicle", 0);
        ChangeVehicle();
        
        GameManager.instance.OpenSettings();

    }

    public void FPV()
    {
        selectedVehicle = 1;
        PlayerPrefs.SetInt("selectedVehicle", 1);
        ChangeVehicle();
        
        GameManager.instance.OpenSettings();
    }

    public void Airplane()
    {
        selectedVehicle = 2;
        PlayerPrefs.SetInt("selectedVehicle", 2);
        ChangeVehicle();
        
        GameManager.instance.OpenSettings();
    }

}
