using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour {

    public delegate void BuySatelliteEvent(GameObject satelliteObject);
    public static event BuySatelliteEvent OnBuySattelite;
    

	public void BuySatellite()
    {
        if (OnBuySattelite != null)
        {
            OnBuySattelite(SateliteFactory.FabricateDefaultSatelite());
        }
    }
}
