using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SatelliteChoserPanel : MonoBehaviour {

    public SateliteFactory.SatelliteType satelliteType;
    public string shortName = "<NO NAME>";
    public TextMeshProUGUI costText;

    public float GetCostFromText()
    {
        float value;
        if (float.TryParse(costText.text, out value))
            return value;
        else
            return 0;
    }

    public void ShowAsTooExpensive()
    {
        costText.color = Color.red;
    }

    public void ShowAsBuyable()
    {
        costText.color = Color.green;
    }

}
