using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Health))]
public class Repairing : MonoBehaviour {

    [SerializeField]
    private int repairValue = 1;

    [SerializeField]
    private float repairIntervalTime = 2;//In Seconds
    private Health health;

    private void Start()
    {
        health = GetComponent<Health>();
        StartCoroutine(RepairRoutine());
    }

    private IEnumerator RepairRoutine()
    {
        while (true)
        {
        yield return new WaitForSeconds(repairIntervalTime);
            health.IncreaseHealth(repairValue);
        }
    }
}
