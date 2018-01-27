using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalTransmitter : MonoBehaviour
{
    public delegate void EntityEvent(SignalTransmitter transmitter);
    public static event EntityEvent OnCreated;
    public static event EntityEvent OnDestroyed;

    static Dictionary<SignalType, List<SignalTransmitter>> instances;
    public static Dictionary<SignalType, List<SignalTransmitter>> Instances
    {
        get
        {
            if (instances == null)
            {
                instances = new Dictionary<SignalType, List<SignalTransmitter>>();
                foreach (SignalType type in Enum.GetValues(typeof(SignalType)))
                    instances.Add(type, new List<SignalTransmitter>());
            }
            return instances;
        }
    }

    Population population;

    public HashSet<Minion> ConnectedMinions { get; private set; }

    [SerializeField]
    private double broadcastRadius = 20; //In degrees
    [SerializeField]
    private SignalType signalType;
    public SignalType SignalType { get { return signalType; } }
    public Transform Planet;


	// Use this for initialization
	private void Start () {
        if(OnCreated!=null)
        {
            OnCreated(this);
        }

        ConnectedMinions = new HashSet<Minion>();

        Instances[signalType].Add(this);
        if (Planet == null)
        {
            GameObject planetGameObject = GameObject.Find("Earth");
            if (planetGameObject != null)
                this.Planet = planetGameObject.transform;
        }
	}

    private void OnDestroy()
    {
        if (OnDestroyed != null)
            OnDestroyed(this);
        Instances[signalType].Remove(this);
    }

    public void SetPopuplation(Population pop)
    {
        this.population = pop;
    }
	
	// Update is called once per frame
	private void LateUpdate ()
    {
        ConnectedMinions = findConnectedReceivers();        
    }

    public bool SignalReaches(Minion receiver)
    {
        Vector2 planetDirection = (Vector2)Planet.position - (Vector2)transform.position;
        if (Vector2.Dot(planetDirection, (Vector2)Planet.position - (Vector2)receiver.transform.position) < 0)
            return false;
        
        Vector2 receiverDirection = (Vector2)receiver.transform.position - (Vector2)transform.position;

        float planetAngle = Mathf.Atan2(planetDirection.y, planetDirection.x)*MathUtils.RADIUS2DEGREE;
        float receiverAngle = Mathf.Atan2(receiverDirection.y, receiverDirection.x)*MathUtils.RADIUS2DEGREE;

        if (Mathf.Abs(MathUtils.AngleDifference(planetAngle, receiverAngle)) > broadcastRadius)
            return false;

        return true;
    }
    
    private HashSet<Minion> findConnectedReceivers()
    {
        HashSet<Minion> connectedReceivers = new HashSet<Minion>();
        foreach (var receiver in population.GetAllMinions())
            if (SignalReaches(receiver))
                connectedReceivers.Add(receiver);
        return connectedReceivers;
    }

    
}
