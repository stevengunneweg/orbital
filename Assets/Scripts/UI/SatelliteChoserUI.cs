using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteChoserUI : MonoBehaviour {


    public GameManager gameManager;

    protected void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var panel = child.GetComponent<SatelliteChoserPanel>();

            if (panel == null)
                return;

            if (panel.GetCostFromText() <= gameManager.GetCurrentScore())
            {
                panel.ShowAsBuyable();
            } else
            {
                panel.ShowAsTooExpensive();
            }
        }
    }


}
