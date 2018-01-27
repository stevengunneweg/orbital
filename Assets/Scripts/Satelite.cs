using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satelite : MonoBehaviour {

    [SerializeField]
    SateliteValues values;
    int turningDirection = 1; // 0 == Counter Clockwise -> 1 == Clockwise
    List<Vector3> launchRoute = new List<Vector3>();
	GameObject pivot;
	float currentOrbitalVelocity = 0.05f;
	public bool SatelliteActivated { get; private set; }
    [SerializeField]
    GameObject rocket;

	public void Spawn(List<Vector3> launchRoute)
    {
        this.launchRoute = launchRoute;
        pivot = new GameObject("Satelite Pivot");
        transform.parent = pivot.transform;

        transform.position = launchRoute[0];
        DetermineDirection();
        SatelliteActivated = true;
    }

    public SateliteValues GetValues()
    {
        return this.values;
	}

	public void SetTeamId(int teamId)
	{
		this.values.SetTeamId(teamId);
	}

    protected void Awake()
    {
		values = new SateliteValues(10, 0.5f, 0.005f);
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
        if(distanceTraveled > values.GetMaxTrajectoryLength())
        {
            Vector3 newPosition1 = Vector3.MoveTowards(transform.position, launchRoute[0], (values.GetMaxTrajectoryLength()));
            if (Vector3.Distance(transform.position, newPosition1) > distanceForTurning)
            {
                transform.LookAt(newPosition1);
                transform.Rotate(new Vector3(-90, 0, 0));
                transform.Rotate(new Vector3(0, -90, 0));
            }
            transform.position = newPosition1;
            return;
        }
        while (distanceTraveled < values.GetMaxTrajectoryLength() && launchRoute.Count > 0)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, launchRoute[0], (values.GetMaxTrajectoryLength() - distanceTraveled));
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
		
    }
}
