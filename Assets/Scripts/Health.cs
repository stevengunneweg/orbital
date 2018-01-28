using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    public delegate void HealthChangeAction(float newValue, float maximumValue);
    public event HealthChangeAction OnValueChanged;

    [SerializeField]
    private float amount = 100;
    private float maximumValue;

    private void Start()
    {
        maximumValue = amount;
    }

    public void IncreaseHealth(int amount)
    {
        this.amount += amount;
        if (this.amount > maximumValue)
            this.amount = maximumValue;

        if (OnValueChanged != null)
            OnValueChanged(this.amount, maximumValue);
    }
    public void DecreaseHealth(int amount)
    {
        this.amount -= amount;
        if (this.amount < 0)
            this.amount = 0;

        if (OnValueChanged != null)
            OnValueChanged(this.amount, maximumValue);
    }
    public float GetHealthAmount()
    {
        return amount;
    }

}
