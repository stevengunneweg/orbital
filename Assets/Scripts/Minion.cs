using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    static List<Minion> instances = new List<Minion>();
    public static List<Minion> Instances
    {
        get
        {
            if (instances == null)
                instances = new List<Minion>();
            return instances;
        }
    }
    
    void Start()
    {
        Instances.Add(this);
    }

    void OnDestroy()
    {
        Instances.Remove(this);
    }
    
    void FixedUpdate()
    {
        float connectionScore = 0;
        foreach (SignalType signalType in Enum.GetValues(typeof(SignalType)))
            connectionScore += calculateScore(signalType);
    }

    // todo [KG] make the score calculation do fancy things
    float calculateScore(SignalType signalType)
    {
        if (!isConnected(signalType))
            return 0;

        return (getConnections(signalType).Count + 1)*Time.deltaTime;
    }

    bool isConnected(SignalType signalType)
    {
        foreach (SignalTransmitter transmitter in SignalTransmitter.Instances[signalType])
            if (transmitter.ConnectedMinions.Contains(this))
                return true;
        return false;
    }

    HashSet<Minion> getConnections(SignalType signalType)
    {
        HashSet<Minion> connections = new HashSet<Minion>();

        foreach (SignalTransmitter signalTransmitter in SignalTransmitter.Instances[signalType])
            if (signalTransmitter.ConnectedMinions.Contains(this))
                foreach (Minion receiver in signalTransmitter.ConnectedMinions)
                    if (!connections.Contains(receiver))
                        connections.Add(receiver);

        if (connections.Contains(this))
            connections.Remove(this);

        return connections;
    }
}
