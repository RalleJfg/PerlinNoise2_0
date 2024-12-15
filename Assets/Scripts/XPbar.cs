using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class XPbar : MonoBehaviour
{
    public static XPbar instance;
    public Slider slider;
    public int maxValue;
    public int level;
    public Text levelText;
    public Text xpText;
    public int localXP;


    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
        
        level = PlayerPrefs.GetInt("level", 1);
        levelText.text = level.ToString();
        
        localXP = PlayerPrefs.GetInt("xp", 0);
        slider.value = localXP;
        
        maxValue = PlayerPrefs.GetInt("xpToNextLevel", 20);
        slider.maxValue = maxValue;
        
        

        // if(PlayerPrefs.GetInt("level") == 0)
        // {
        //     level = 1;
        //     levelText.text = level.ToString();
        // }
        // if(PlayerPrefs.GetInt("xpToNextLevel") == 0)
        // {
        //     maxValue = 20;
        //     slider.maxValue = maxValue;
        //     print(PlayerPrefs.GetInt("xpToNextLevel"));
        // }
    }

    void Update()
    {
            slider.value = localXP;

            xpText.text = localXP + "/" + maxValue;
    }   
    public void SetXP(int xp)
    {
        localXP += xp;
        if(localXP >= slider.maxValue)
        {
            
            level++;
            levelText.text = level.ToString();
            maxValue += 3;
            slider.maxValue = maxValue;

            PlayerPrefs.SetInt("xpToNextLevel", maxValue);

            localXP = 0;
            slider.value = 0;
        }
        slider.value = localXP;
        //slider.maxValue = maxValue;
        PlayerPrefs.SetInt("xp", localXP);
        PlayerPrefs.SetInt("level", level);
        
    }



    
}
