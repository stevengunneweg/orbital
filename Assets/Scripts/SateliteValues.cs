using UnityEngine;
[System.Serializable]
public class SateliteValues
{
    public float OrbitalVelocity { get; private set; }
    public float MaxTrajectoryLength { get; private set; }
    public float Cost { get; private set; }

    public SateliteValues(float cost, float orbitalVelocity, float maxTrajectoryLength)
    {
        this.Cost = cost;
        this.OrbitalVelocity = orbitalVelocity;
        this.MaxTrajectoryLength = maxTrajectoryLength;
    }
}