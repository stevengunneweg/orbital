using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    [SerializeField]
    private float amount = 100;

    public void IncreaseHealth(int amount)
    {
        this.amount += amount;
    }
    public void DecreaseHealth(int amount)
    {
        this.amount -= amount;
    }
    public float GetHealthAmount()
    {
        return amount;
    }

}
