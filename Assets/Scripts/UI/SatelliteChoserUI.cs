using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SatelliteChoserUI : MonoBehaviour {


    private bool canPayAny;
    public GameManager gameManager;

    public TextMeshProUGUI timeText;

    protected void Update()
    {
        canPayAny = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var panel = child.GetComponent<SatelliteChoserPanel>();

            if (panel == null)
                return;

            if (panel.GetCostFromText() <= gameManager.GetCurrentScore())
            {
                panel.ShowAsBuyable();
                canPayAny = true;
            } else
            {
                panel.ShowAsTooExpensive();
            }
        }

        var totalSeconds = gameManager.TimeTheGameIsRunningInSeconds;
        int numberOfSmallTimeUnitsInOneLargeTimeUnit = 53;
        var smallTimeUnit = Mathf.Floor(totalSeconds % numberOfSmallTimeUnitsInOneLargeTimeUnit);
        var largeTimeUnit = Mathf.Floor(totalSeconds / numberOfSmallTimeUnitsInOneLargeTimeUnit);
        timeText.text = largeTimeUnit + " year" + (largeTimeUnit == 1 ? "" : "s") + ", " + smallTimeUnit + " week" + (smallTimeUnit == 1 ? "" : "s");
    }

    public bool CanPayAnySatellite()
    {
        return canPayAny;
    }

}
