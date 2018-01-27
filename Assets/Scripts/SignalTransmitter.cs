using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalTransmitter : MonoBehaviour
{
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
    
    public HashSet<SignalReceiver> Receivers { get; private set; }

    [SerializeField]
    double broadcastRadius = 20; //In degree
    [SerializeField]
    SignalType signalType;
    public SignalType SignalType { get { return signalType; } }

    public Transform Planet;

	// Use this for initialization
	void Start () {
        Receivers = new HashSet<SignalReceiver>();

        Instances[signalType].Add(this);
        if (Planet == null)
        {
            GameObject planetGameObject = GameObject.Find("Earth");
            if (planetGameObject != null)
                this.Planet = planetGameObject.transform;
        }
	}

    void OnDestroy()
    {
        Instances[signalType].Remove(this);
    }
	
	// Update is called once per frame
	void LateUpdate ()
    {
        Receivers = findConnectedReceivers();        
    }

    public bool ReceivesSignal(SignalReceiver receiver)
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
    
    HashSet<SignalReceiver> findConnectedReceivers()
    {
        HashSet<SignalReceiver> connectedReceivers = new HashSet<SignalReceiver>();
        foreach (var receiver in SignalReceiver.Instances)
            if (ReceivesSignal(receiver))
                connectedReceivers.Add(receiver);
        return connectedReceivers;
    }

    
}
