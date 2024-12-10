using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    public static VehicleManager Instance;

    public int selectedVehicle;
    public GameObject[] vehicles;

    void Awake()
    {
        Instance = this;
        selectedVehicle = PlayerPrefs.GetInt("selectedVehicle", 0);
        ChangeVehicle();
    }

    void ChangeVehicle()
    {
        for (int i = 0; i < vehicles.Length; i++)
        {
            if (i == selectedVehicle)
            {
                vehicles[i].SetActive(true);
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
    }

    public void FPV()
    {
        selectedVehicle = 1;
        PlayerPrefs.SetInt("selectedVehicle", 1);
        ChangeVehicle();
    }

    public void Airplane()
    {
        selectedVehicle = 2;
        PlayerPrefs.SetInt("selectedVehicle", 2);
        ChangeVehicle();
    }

}
