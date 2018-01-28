using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour {

    public TextMeshProUGUI timeText;

	public void SetTime(float time)
    {
        var totalSeconds = time;
        int numberOfSmallTimeUnitsInOneLargeTimeUnit = 53;
        var smallTimeUnit = Mathf.Floor(totalSeconds % numberOfSmallTimeUnitsInOneLargeTimeUnit);
        var largeTimeUnit = Mathf.Floor(totalSeconds / numberOfSmallTimeUnitsInOneLargeTimeUnit);
        timeText.text = largeTimeUnit + " year" + (largeTimeUnit == 1 ? "" : "s") + ", " + smallTimeUnit + " week" + (smallTimeUnit == 1 ? "" : "s");
    }
	
}
