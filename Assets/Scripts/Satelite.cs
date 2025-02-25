﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satelite : MonoBehaviour
{
    public delegate void EntityEvent(Satelite satelite);
    public static event EntityEvent OnCreated;
    public static event EntityEvent OnDestroyed;

    [SerializeField]
    SateliteValues values;
    int turningDirection = 1; // 0 == Counter Clockwise -> 1 == Clockwise
    List<Vector3> launchRoute = new List<Vector3>();
	GameObject pivot;
	float currentOrbitalVelocity = 0.05f;
	public bool SatelliteActivated { get; private set; }
    [SerializeField]
    GameObject rocket;
    public int TeamId { get { return values.TeamId; } }
    public bool IsPlayer { get { return values.TeamId == 1; } }
    

    private void updateLayer(bool isPlayer)
    {
        SetLayer(gameObject, isPlayer ? LayerMask.NameToLayer("Player") : LayerMask.NameToLayer("Enemy"));
    }
    private void SetLayer(GameObject root, int layer)
    {
        root.layer = layer;
        for (int i = 0; i < root.transform.childCount; ++i)
            SetLayer(transform.GetChild(i).gameObject, layer);
    }

    private GameObject cone;

	public void Spawn(List<Vector3> launchRoute)
    {
        this.launchRoute = launchRoute;
        pivot = new GameObject("Satelite Pivot");
        transform.parent = pivot.transform;

        transform.position = launchRoute[0];
        DetermineDirection();
        SatelliteActivated = true;
    }

    private void OnDestroy()
    {
        if (OnDestroyed != null)
            OnDestroyed(this);
    }
    
    public void SetValues(SateliteValues values)
    {
        this.values = values;
    }

    public SateliteValues GetValues()
    {
        return this.values;
	}

	public void SetTeamId(int teamId)
	{
		this.values.SetTeamId(teamId);
	}

    protected void Start()
    {
        currentOrbitalVelocity = values.GetTrajectorySpeed();
        if (OnCreated != null)
        {
            OnCreated(this);
        }

        var coneComponent = transform.Find("Cone");
		if (coneComponent != null) {
			cone = coneComponent.gameObject;
		}
        updateLayer(IsPlayer);
	}
    private void Update()
    {
        rocket.SetActive((launchRoute.Count != 0));

        if (launchRoute.Count != 0) {
			TravelToInitialDestination();
			return;
		}
		if (pivot == null) {
			return;
		}

		// Accelerate to orbitalVelocity
		if (currentOrbitalVelocity < values.GetOrbitalVelocity()) {
			currentOrbitalVelocity += (Time.deltaTime / 4);
			if (currentOrbitalVelocity > values.GetOrbitalVelocity()) {
				currentOrbitalVelocity = values.GetOrbitalVelocity();
			}
		}

		// Show cone when satellite is deployed
		if (cone != null) {
			cone.GetComponent<Renderer>().enabled = true;
		}

		pivot.transform.Rotate(new Vector3(0, 0, currentOrbitalVelocity * -1 * turningDirection));
    }

    void DetermineDirection()
    {
		Vector3 lastRouteCoordinate = launchRoute[launchRoute.Count - 1];
		Vector3 secondLastRouteCoordinate = launchRoute[launchRoute.Count - 2];
		// Find the direction of the cross product to know which direction the vector is going (down for CW or up for CCW).
		Vector3 cross = Vector3.Cross(lastRouteCoordinate, secondLastRouteCoordinate);

		turningDirection = cross.z > 0 ? 1 : -1;
    }

    void TravelToInitialDestination()
    {
        float distanceTraveled = 0;
        float distanceForTurning = 0.001f;
        distanceTraveled += Vector3.Distance(transform.position, launchRoute[0]);
        if(distanceTraveled > values.GetTrajectorySpeed())
        {
            Vector3 newPosition1 = Vector3.MoveTowards(transform.position, launchRoute[0], (values.GetTrajectorySpeed()));
            if (Vector3.Distance(transform.position, newPosition1) > distanceForTurning)
            {
                transform.LookAt(newPosition1);
                transform.Rotate(new Vector3(-90, 0, 0));
                transform.Rotate(new Vector3(0, -90, 0));
            }
            transform.position = newPosition1;
            return;
        }
        while (distanceTraveled < values.GetTrajectorySpeed() && launchRoute.Count > 0)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, launchRoute[0], (values.GetTrajectorySpeed() - distanceTraveled));
            if(Vector3.Distance(transform.position,newPosition) > distanceForTurning)
            {
                transform.LookAt(newPosition);
                transform.Rotate(new Vector3(-90, 0, 0));
                transform.Rotate(new Vector3(0, -90, 0));
            }
            transform.position = newPosition;
            distanceTraveled += Vector3.Distance(transform.position, launchRoute[0]);


			if (transform.position == launchRoute[0]) {
                launchRoute.RemoveAt(0);
			}
        }
        if(launchRoute.Count == 0)
        {
            transform.LookAt(new Vector3(0, 0, transform.position.z));
            transform.Rotate(new Vector3(-90, 0, 0));
            transform.Rotate(new Vector3(0, -90, 0));
        }
		

		// Resize cone
		if (cone != null) {
			cone.transform.localScale = Vector3.one * transform.position.magnitude;
		}
    }
}
