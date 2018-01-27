using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInfoView : MonoBehaviour {

    [SerializeField]
    TextMeshProUGUI money;
    [SerializeField]
    TextMeshProUGUI nrOfSatellites;

    public void DrawInfo(int money, int nrOfSatellites)
    {
        this.money.text = string.Format("{00:00}", money);
        this.nrOfSatellites.text = nrOfSatellites.ToString();
    }

}
