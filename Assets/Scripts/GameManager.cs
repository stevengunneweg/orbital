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

    public SateliteFactory.SatelliteType currentSatelliteType;
    public float currentSatelliteCost = 125;

    public void ChangeToDefensiveSatellite()
    {
        currentSatelliteType = SateliteFactory.SatelliteType.Defense;
    }

    public void ChangeToSelfRepairingSatellite()
    {
        currentSatelliteType = SateliteFactory.SatelliteType.SelfRepairing;
    }

    public void ChangeToTransmissionSatellite()
    {
        currentSatelliteType = SateliteFactory.SatelliteType.Transmit;
    }

    public void ChangeToRailgunSatellite()
    {
        currentSatelliteType = SateliteFactory.SatelliteType.Attack;
    }

    private void Start()
    {
        currentPlayer = new Player(150);
        launchPad.OnBoughtSatellite += BuySatellite;
    }

    private void OnDestroy()
    {
        launchPad.OnBoughtSatellite -= BuySatellite;
    }

    private void Update()
    {
        playerInfoView.DrawInfo((int)currentPlayer.Score.CurrentScore, population.NrOfSatellites(), currentSatelliteType.ToString(), currentSatelliteCost);
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
