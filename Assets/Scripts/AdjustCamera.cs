using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustCamera : MonoBehaviour {

    public Population population;
    private Camera cam;

    public float minSize = 7;
    public float changeSpeed = 0.5f;

    protected void Start()
    {
        cam = GetComponent<Camera>();
    }

    protected void Update()
    {
        var targetSize = minSize;

        foreach (var satelite in population.GetAllSatelites())
        {
            if (!satelite.SatelliteActivated)
                continue;

            var distance = satelite.transform.position.magnitude + 1f;

            if (distance > targetSize)
                targetSize = distance;
        }

        var currentSize = cam.orthographicSize;
        cam.orthographicSize = Mathf.MoveTowards(currentSize, targetSize, Time.deltaTime * changeSpeed);
    }

}
