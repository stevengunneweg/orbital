﻿using System.Collections;
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

    public SateliteFactory.SatelliteType currentSatelliteType;

    public void ChangeToDefaultSatellite()
    {
        currentSatelliteType = SateliteFactory.SatelliteType.Default;
    }

    public void ChangeToRailgunSatellite()
    {
        currentSatelliteType = SateliteFactory.SatelliteType.Railgun;
    }

    private void Start()
    {
        currentPlayer = new Player(50);
        launchPad.OnBoughtSatellite += BuySatellite;
    }

    private void OnDestroy()
    {
        launchPad.OnBoughtSatellite -= BuySatellite;
    }

    private void Update()
    {
        playerInfoView.DrawInfo((int)currentPlayer.Score.CurrentScore, population.NrOfSatellites(), currentSatelliteType.ToString());
        foreach(var m in population.GetAllMinions())
        { 
            currentPlayer.Score.AddScore(m.GetCurrentScore());
        }
    }

    public void BuySatellite(GameObject satelliteObject)
    {
        Satelite sat = satelliteObject.GetComponent<Satelite>();
        if (currentPlayer.Score.CurrentScore >= sat.GetValues().GetCost())
        {
            currentPlayer.Score.DecreaseScore(sat.GetValues().GetCost());
        }
        else
        {
            Destroy(satelliteObject);
        }
    }
}
