using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    [SerializeField]
    Image healthBar;
    [SerializeField]
    Health health;

    // Use this for initialization
    void Start () {
        health.OnValueChanged += OnHealthValueChanged;
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void OnHealthValueChanged(float newValue, float maxValue) {
        healthBar.fillAmount = newValue / maxValue;

        if (newValue == 0)
            gameObject.SetActive(false);
        else if (!gameObject.activeSelf)
            gameObject.SetActive(true);
    }
}
