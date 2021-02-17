using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "SO/Level")]
public class LevelData : ScriptableObject
{
    public int levelNumber;
    public int maxScore;
    public int scoreMultiplier = 1;
    public int currentScore;

    public int CalculateStars()
    {
        if (maxScore > 0)
        {
            int percent = Mathf.RoundToInt(currentScore * 100 / maxScore);
            if (percent > 80)
            {
                return 3;
            }
            else if (percent > 60)
            {
                return 2;
            }
            else if (percent > 40)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 3;
        }
    }

}
