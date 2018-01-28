using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecreaseHealthOnCollision : MonoBehaviour {

    [SerializeField]
    Health health;

    int decreaseHealth = 50;

    public void SetHealthDecrease(int nr)
    {
        decreaseHealth = nr;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        health.DecreaseHealth(decreaseHealth);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        health.DecreaseHealth(decreaseHealth);
    }
}
