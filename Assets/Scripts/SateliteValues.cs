using UnityEngine;
[System.Serializable]
public class SateliteValues
{

    [SerializeField]
    private float orbitalVelocity;
    [SerializeField]
    private float maxTrajectoryLength;
    [SerializeField]
    private float cost;

    public SateliteValues(float cost, float orbitalVelocity, float maxTrajectoryLength)
    {
        this.cost = cost;
        this.orbitalVelocity = orbitalVelocity;
        this.maxTrajectoryLength = maxTrajectoryLength;
    }

    public float GetOrbitalVelocity()
    {
        return orbitalVelocity;
    }

    public float GetMaxTrajectoryLength()
    {
        return maxTrajectoryLength;
    }

    public float GetCost()
    {
        return cost;
    }

}