using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIHandle : MonoBehaviour
{
    [SerializeField] Text levelNumber;
    [SerializeField] Text cashOnWin;
    [SerializeField] Text cashOnLose;
    int currentLevel;



    public void UpdateCashWon(bool won, int cash)
    {
        if (won)
        {
            cashOnWin.text = cash.ToString();
        }
        else
        {
            cashOnLose.text = cash.ToString();
        }
    }


    public void UpdateLevel(int level)
    {
        currentLevel = level;
        levelNumber.text = currentLevel.ToString();
    }

}
