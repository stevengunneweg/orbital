using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalReceiver : MonoBehaviour
{
    static List<SignalReceiver> instances = new List<SignalReceiver>();
    public static List<SignalReceiver> Instances
    {
        get
        {
            if (instances == null)
                instances = new List<SignalReceiver>();
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
        Score.Value += connectionScore;
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
            if (transmitter.Receivers.Contains(this))
                return true;
        return false;
    }

    HashSet<SignalReceiver> getConnections(SignalType signalType)
    {
        HashSet<SignalReceiver> connections = new HashSet<SignalReceiver>();

        foreach (SignalTransmitter signalTransmitter in SignalTransmitter.Instances[signalType])
            if (signalTransmitter.Receivers.Contains(this))
                foreach (SignalReceiver receiver in signalTransmitter.Receivers)
                    if (!connections.Contains(receiver))
                        connections.Add(receiver);

        if (connections.Contains(this))
            connections.Remove(this);

        return connections;
    }
}
