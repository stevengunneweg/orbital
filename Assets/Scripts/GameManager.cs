using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {


    Player currentPlayer;
    [SerializeField]
    Population population;
    [SerializeField]
    LaunchPad launchPad;
    [SerializeField]
    PlayerInfoView playerInfoView;

    public SatelliteChoserUI satelliteChoser;
    [HideInInspector]
    public SatelliteChoserPanel lastSatelliteChoice;
    public GameObject endScreen;

    private Timer? gameTimer = null;

    public float noSatalliteDecreaseCost = 0.01f;
    private static bool _gameRunning;

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
        currentPlayer = new Player(220);
        launchPad.OnBoughtSatellite += BuySatellite;
    }

    private void OnDestroy()
    {
        launchPad.OnBoughtSatellite -= BuySatellite;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
            RestartGame();
        playerInfoView.DrawInfo((int)currentPlayer.Score.CurrentScore, population.NrOfSatellites(), lastSatelliteChoice);
        foreach(var m in population.GetAllMinions())
        { 
            currentPlayer.Score.AddScore(m.GetCurrentScore());
        }
        if (_gameRunning && population.ActivePlayerTransmitSatellites().Count <= 0)
        {
            if (EnableDecrease)
                currentPlayer.Score.DecreaseScore(noSatalliteDecreaseCost);

            var lost = !satelliteChoser.CanPayAnySatellite();
            if (lost)
                EndGame();
        }

        if (_gameRunning && gameTimer == null)
        {
            gameTimer = new Timer(0);
        }
    }

    public void BuySatellite(GameObject satelliteObject)
    {
		lastSatelliteChoice = null;

        Satelite sat = satelliteObject.GetComponent<Satelite>();
        if (currentPlayer.Score.CurrentScore >= sat.GetValues().GetCost())
        {
            currentPlayer.Score.DecreaseScore(sat.GetValues().GetCost());
            if (!GameRunning)
                GameRunning = true;
        }
        else
        {
            Destroy(satelliteObject);
        }
    }
    private void EndGame()
    {
        GameRunning = false;
        endScreen.SetActive(true);
        endScreen.GetComponent<EndScreen>().SetTime(TimeTheGameIsRunningInSeconds);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public static bool EnableDecrease
    {
        get; set;
    }
    public static bool GameRunning
    {
        get { return _gameRunning; } set { _gameRunning = value; EnableDecrease = value; }
    }

    public float TimeTheGameIsRunningInSeconds
    {
        get
        {
            return gameTimer.HasValue ? gameTimer.Value.time : 0;
        }
    }
}
