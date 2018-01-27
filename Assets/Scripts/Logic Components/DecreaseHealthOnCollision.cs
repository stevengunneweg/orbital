﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecreaseHealthOnCollision : MonoBehaviour {

    [SerializeField]
    Health health;

    int decreaseHealth = 50;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        health.DecreaseHealth(decreaseHealth);
    }
}
