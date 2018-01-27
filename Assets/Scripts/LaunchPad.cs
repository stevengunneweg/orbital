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
                if (trajectoryPositions.Count > 1)
                {
                    GameObject tempSat = SateliteFactory.FabricateDefaultSatelite();
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
        if (trajectoryPositions.Count == 0)
        {
            trajectoryPositions.Add(launchpadObject.transform.position);
        }
        if (Vector3.Distance(GetMouseWorldPosition() + new Vector3(0, 0, -1), Vector3.zero + new Vector3(0, 0, -1)) < Vector3.Distance(Vector3.zero + new Vector3(0, 0, -1), launchpadObject.transform.position))
        {
            trajectoryPositions = new List<Vector3>();
            lineRenderer.positionCount = trajectoryPositions.Count;
            lineRenderer.SetPositions(trajectoryPositions.ToArray());
            return;
        }
        float length = trajectoryPositions.Count == 0 ? 0 :
            Vector3.Distance(trajectoryPositions[0], GetMouseWorldPosition() + new Vector3(0, 0, -1));
        for (int i = 0; i < trajectoryPositions.Count - 1; i++)
        {
            length += Vector3.Distance(trajectoryPositions[i], trajectoryPositions[i + 1]);
        }
        if (length < 5)
        {
            if (trajectoryPositions[trajectoryPositions.Count - 1] != GetMouseWorldPosition() + new Vector3(0, 0, -1))
                trajectoryPositions.Add(GetMouseWorldPosition() + new Vector3(0, 0, -1));
        }

        lineRenderer.positionCount = trajectoryPositions.Count;
        lineRenderer.SetPositions(trajectoryPositions.ToArray());
    }

    public void UpdatePosition()
    {
        launchpadObject.transform.position = GetMouseWorldPosition().normalized * distanceToEarthsCrust + new Vector3(0,0,-1);
    }
}
