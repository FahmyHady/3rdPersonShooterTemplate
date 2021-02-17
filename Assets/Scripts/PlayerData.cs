using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "SO/PlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField] private int cash;
    public int currentLevel;

    public int Cash
    {
        get => cash;
        set
        {
            cash = value;
            SaveCash();
            EventManager.TriggerEvent("Cash Updated", cash);
        }
    }
    public void SaveData()
    {
        SaveCash();
        SaveLevel();
    }

    public void LoadData()
    {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel",1);
        Cash = PlayerPrefs.GetInt("Cash");
    }
    public void SaveLevel()
    {
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
    }
    public void SaveCash()
    {
        PlayerPrefs.SetInt("Cash", Cash);
    }

}
