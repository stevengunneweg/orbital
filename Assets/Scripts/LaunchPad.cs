﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPad : MonoBehaviour {
    [SerializeField]
    private Renderer launchpadRenderer;
    [SerializeField]
    private GameObject launchpadObject;
    private float distanceToEarthsCrust = 2.5f;
    private bool isActive = true;
    private bool isMoving = true;
    private bool isShooting = false;
    [SerializeField]
    private LineRenderer lineRenderer;
	[SerializeField]
	private float maxDrawingDistance = 2.0f;

    private void Start()
    {
        MenuHandler.OnBuySattelite += BuySatellite;
    }

    private void OnDestroy()
    {
        MenuHandler.OnBuySattelite -= BuySatellite;
    }

    private void BuySatellite()
    {
        isActive = true;
        isMoving = true;
    }

    private Vector3 GetMouseWorldPosition()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = 10.0f;
        var worldPos = Camera.main.ScreenToWorldPoint(mousePos);
		worldPos.z = 0;
        return worldPos;
    }

    private void Update()
    {
        transform.LookAt(new Vector3(0, 0, transform.position.z));
        transform.Rotate(new Vector3(-90, 0, 0));
        transform.Rotate(new Vector3(0, -90, 0));
        if (!isActive)
        {
            return;
        }
        if (isMoving)
        {
            UpdatePosition();
        }
        if (Input.GetMouseButtonDown(0))
        {
            trajectoryPositions = new List<Vector3>();
            trajectoryPositions.Add(launchpadObject.transform.position + launchpadObject.transform.position.normalized * 0.2f);
            isShooting = true;
            isMoving = false;
        }
        if(Input.GetMouseButton(0))
        {
            DrawTrajectory();
        }
        if(isShooting)
        {
            if(Input.GetMouseButtonUp(0))
			{
                if (trajectoryPositions.Count > 1)
                {
                    GameObject tempSat = SateliteFactory.FabricateRailgunSatelite();
                    tempSat.GetComponent<Satelite>().Spawn(trajectoryPositions);
                    tempSat.GetComponent<Satelite>().transform.Rotate(launchpadObject.transform.rotation.eulerAngles);
                }

                trajectoryPositions = new List<Vector3>();
                Clear();
                isMoving = false;
                isShooting = false;
                isActive = false;
            }
        }
    }


    public void Clear()
    {
        launchpadObject.transform.position = new Vector3(1000000, 0, 0);
        lineRenderer.positionCount = trajectoryPositions.Count;
        lineRenderer.SetPositions(trajectoryPositions.ToArray());
    }

    List<Vector3> trajectoryPositions = new List<Vector3>();
    public void DrawTrajectory()
    {
		// Set initial position
        if (trajectoryPositions.Count == 0) {
            trajectoryPositions.Add(launchpadObject.transform.position);
        }

		// Calculate currently drawn trajectory length
		float length = 0.0f;
		for (int i = 1; i < trajectoryPositions.Count; i++) {
			length += Vector3.Distance(trajectoryPositions[i - 1], trajectoryPositions[i]);
		}

		// Check if drawing is allowed
		if (length < maxDrawingDistance) {
			float remainingDistance = maxDrawingDistance - length;
			Vector3 prevPosition = trajectoryPositions[trajectoryPositions.Count - 1];
			Vector3 newPosition = GetMouseWorldPosition();

            var minDist = distanceToEarthsCrust + 0.2f;
            if (newPosition.magnitude < minDist)
                newPosition = newPosition.normalized * minDist;

			// Only add new point when position is different from previous one
			if (prevPosition != newPosition) {
				float newLength = length + Vector3.Distance(prevPosition, newPosition);
				// Limit last path length if it exceeds the maxDrawingDistance
				if (newLength > maxDrawingDistance) {
					Vector3 lastPath = newPosition - prevPosition;
					lastPath = Vector3.ClampMagnitude(lastPath, remainingDistance);
					newPosition = prevPosition + lastPath;
				}

				trajectoryPositions.Add(newPosition);
			}
		}

        lineRenderer.positionCount = trajectoryPositions.Count;
        lineRenderer.SetPositions(trajectoryPositions.ToArray());
    }

    public void UpdatePosition()
    {
        launchpadObject.transform.position = GetMouseWorldPosition().normalized * distanceToEarthsCrust;
    }
}
