using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LaunchPad : MonoBehaviour {

    [SerializeField]
    private Renderer launchpadRenderer;

    [SerializeField]
    private GameObject launchpadObject;

    private float distanceToEarthsCrust = 1.6f;

    private bool isActive = true;

    private bool isMoving = true;

    private bool isShooting = false;

    [SerializeField]
    private LineRenderer lineRenderer;

    private Vector3 GetMouseWorldPosition()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = 10.0f;
        var worldPos = Camera.main.ScreenToWorldPoint(mousePos);

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
            trajectoryPositions.Add(launchpadObject.transform.position);
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
                GameObject tempSat = SateliteFactory.FabricateRailgunSatelite();
                tempSat.GetComponent<Satelite>().Spawn(trajectoryPositions);
				tempSat.GetComponent<Satelite>().transform.Rotate(launchpadObject.transform.rotation.eulerAngles);
                trajectoryPositions = new List<Vector3>();
                isMoving = true;
                isShooting = false;
            }
        }
           
    }

    List<Vector3> trajectoryPositions = new List<Vector3>();
    public void DrawTrajectory()
    {
        float length = trajectoryPositions.Count == 0 ? 0 :
            Vector3.Distance(trajectoryPositions[0], GetMouseWorldPosition() + new Vector3(0, 0, -1));
        
        for (int i=0;i<trajectoryPositions.Count -1; i++)
        {
            length += Vector3.Distance(trajectoryPositions[i], trajectoryPositions[i + 1]);
        }
        if(length < 5)
        {
            trajectoryPositions.Add(GetMouseWorldPosition() + new Vector3(0, 0, -1));
        }

        lineRenderer.positionCount = trajectoryPositions.Count;
        lineRenderer.SetPositions(trajectoryPositions.ToArray());
    }

    public void ShootSatellite()
    {

    }


    public void UpdatePosition()
    {
        launchpadObject.transform.position = GetMouseWorldPosition().normalized * distanceToEarthsCrust + new Vector3(0,0,-1);
    }
}
