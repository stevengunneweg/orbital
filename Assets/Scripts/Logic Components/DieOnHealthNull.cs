using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieOnHealthNull : MonoBehaviour {

    [SerializeField]
    Health health;

    [SerializeField]
    GameObject destroyObject;

    private void Update()
    {
        if(health.GetHealthAmount() <= 0)
        {
            Destroy(destroyObject);
        }
    }
}
