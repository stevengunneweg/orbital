using System.Collections;
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

	public void DrawInfo(int money, int nrOfSatellites, SatelliteChoserPanel lastSatellitePanel)
    {
        this.money.text = "$" + string.Format("{00:00}", money);
        this.nrOfSatellites.text = "x" + nrOfSatellites.ToString();
		if (lastSatellitePanel == null) {
			this.currentSatelliteName.text = "";
		} else {
			this.currentSatelliteName.text = lastSatellitePanel.shortName + " ($" + lastSatellitePanel.GetCostFromText() + ")";
		}
    }
}
