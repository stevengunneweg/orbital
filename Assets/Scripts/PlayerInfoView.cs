﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInfoView : MonoBehaviour {

    [SerializeField]
    TextMeshProUGUI money;
    [SerializeField]
    TextMeshProUGUI nrOfSatellites;
    [SerializeField]
    TextMeshProUGUI currentSatelliteName;

    public void DrawInfo(int money, int nrOfSatellites, string satelliteName, float satellitePrice)
    {
        this.money.text = "$" + string.Format("{00:00}", money);
        this.nrOfSatellites.text = "x" + nrOfSatellites.ToString();
        this.currentSatelliteName.text = satelliteName + " ($" + satellitePrice.ToString("0") + ")";
    }

}
