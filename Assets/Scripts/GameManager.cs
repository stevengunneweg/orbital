﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {


    Player currentPlayer;
    [SerializeField]
    Population population;
    [SerializeField]
    LaunchPad launchPad;
    [SerializeField]
    PlayerInfoView playerInfoView;

    public SatelliteChoserPanel lastSatelliteChoice;

    public float noSatalliteDecreaseCost = 0.01f;

    public void SetLastSatelliteChoice(SatelliteChoserPanel lastSatelliteChoice)
    {
        this.lastSatelliteChoice = lastSatelliteChoice;
    }

    public float GetCurrentScore()
    {
        return currentPlayer.Score.CurrentScore;
    }

    private void Start()
    {
        EnableDecrease = false;
        currentPlayer = new Player(5000);
        launchPad.OnBoughtSatellite += BuySatellite;
    }

    private void OnDestroy()
    {
        launchPad.OnBoughtSatellite -= BuySatellite;
    }

    private void Update()
    {
        playerInfoView.DrawInfo((int)currentPlayer.Score.CurrentScore, population.NrOfSatellites(), lastSatelliteChoice.shortName, lastSatelliteChoice.GetCostFromText());
        foreach(var m in population.GetAllMinions())
        { 
            currentPlayer.Score.AddScore(m.GetCurrentScore());
        }
        if (population.NrOfSatellites() <= 0)
        {
            if (EnableDecrease)
                currentPlayer.Score.DecreaseScore(noSatalliteDecreaseCost);
        }
    }

    public void BuySatellite(GameObject satelliteObject)
    {
        Satelite sat = satelliteObject.GetComponent<Satelite>();
        if (currentPlayer.Score.CurrentScore >= sat.GetValues().GetCost())
        {
            currentPlayer.Score.DecreaseScore(sat.GetValues().GetCost());
            if (!EnableDecrease)
                EnableDecrease = true;
        }
        else
        {
            Destroy(satelliteObject);
        }
    }
    public bool EnableDecrease
    {
        get; set;
    }
}
