using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour {

    [Header("Reference")]
    public GameManager gameManager;

    public delegate void BuySatelliteEvent(GameObject satelliteObject);
    public static event BuySatelliteEvent OnBuySattelite;
    

	public void BuySatellite()
    {
        if (OnBuySattelite != null)
        {
            var satelite = SateliteFactory.From(gameManager.currentSatelliteType);
            OnBuySattelite(satelite.gameObject);
        }
    }
}
