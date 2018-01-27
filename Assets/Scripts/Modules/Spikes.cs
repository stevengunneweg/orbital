using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(DecreaseHealthOnCollision))]
public class Spikes : MonoBehaviour {
    [SerializeField]
    private int spikeDamage = 80;

    private void Start()
    {
        GetComponent<DecreaseHealthOnCollision>().SetHealthDecrease(spikeDamage);
    }

}
