using System;
using UnityEngine;
[System.Serializable]
public class SateliteValues
{

    [SerializeField]
    private float orbitalVelocity;
    [SerializeField]
    private float trajectorySpeed;
    [SerializeField]
	private float cost;
	[SerializeField]
	private int teamId;
    public int TeamId { get { return teamId; } }

    public SateliteValues(float cost, float orbitalVelocity, float trajectorySpeed)
    {
        this.cost = cost;
        this.orbitalVelocity = orbitalVelocity;
        this.trajectorySpeed = trajectorySpeed;
    }

    public float GetOrbitalVelocity()
    {
        return orbitalVelocity;
    }

    public float GetCost()
    {
        return cost;
	}

	public float GetTeamId()
	{
		return teamId;
	}

    public void SetCost(float cost)
    {
        this.cost = cost;
    }

	public void SetTeamId(int value)
	{
		teamId = value;
	}

    internal float GetTrajectorySpeed()
    {
        return trajectorySpeed;
    }
}