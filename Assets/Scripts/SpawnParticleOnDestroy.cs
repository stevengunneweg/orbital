using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticleOnDestroy : MonoBehaviour {
    [SerializeField]
    GameObject particlePrefab;

    bool isQuitting = false;
    void OnApplicationQuit()
    {
        isQuitting = true;
    }
    void OnDestroy()
    {
        if (!isQuitting)
        GameObject.Instantiate(particlePrefab, transform.position, Quaternion.identity);
    }
}
