using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
    public Score Score { get; private set; }  
    public Player(int startScore)
    {
        Score = new Score();
        Score.SetAmount(startScore);
    }

    public void BuySatellite(float cost)
    {
        if(Score.CurrentScore >= cost)
        {
            Score.DecreaseScore(cost);
            //Buy Satellite
        }
    }
}
