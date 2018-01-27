using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score {

    public float CurrentScore { get; private set; }

    public void AddScore(float value)
    {
        CurrentScore += value;
    }

    public void SetAmount(int score)
    {
        CurrentScore = score;
    }

    public void DecreaseScore(float value)
    {
        CurrentScore -= value;
    }
}
