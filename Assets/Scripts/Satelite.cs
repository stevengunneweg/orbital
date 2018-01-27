using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satelite : MonoBehaviour {

    private SateliteValues values = new SateliteValues(10, 0.5f, 0.005f);
    List<Vector3> launchRoute;
    GameObject pivot;

    public void Spawn(List<Vector3> launchRoute)
    {
        this.launchRoute = launchRoute;
        pivot = new GameObject();
        transform.parent = pivot.transform;

        transform.position = launchRoute[0];
    }

    public SateliteValues GetValues()
    {
        return this.values;
    }

    private void Update()
    {
        if(launchRoute.Count != 0)
        {
            TravelToInitialDestination();
            return;
        }
        pivot.transform.Rotate(new Vector3(0, 0, this.values.OrbitalVelocity));
    }
    
    void TravelToInitialDestination()
    {
        float distanceTraveled = 0;
        distanceTraveled += Vector3.Distance(transform.position, launchRoute[0]);
        if(distanceTraveled > values.MaxTrajectoryLength)
        {
            Vector3 newPosition1 = Vector3.MoveTowards(transform.position, launchRoute[0], (values.MaxTrajectoryLength));
            transform.position = newPosition1;
            return;
        }
        while (distanceTraveled < values.MaxTrajectoryLength && launchRoute.Count > 0)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, launchRoute[0], (values.MaxTrajectoryLength - distanceTraveled));
            transform.position = newPosition;
            distanceTraveled += Vector3.Distance(transform.position, launchRoute[0]);


            if (transform.position == launchRoute[0])
                launchRoute.RemoveAt(0);
        }


    }

}
