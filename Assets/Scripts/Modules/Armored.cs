using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Health))]
public class Armored : MonoBehaviour {

    [SerializeField]
    int shieldValue = 50;

    private void Start()
    {
        GetComponent<Health>().IncreaseHealth(shieldValue);
    }
    
}
