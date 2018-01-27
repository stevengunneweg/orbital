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

        return (getConnections(signalType).Count + 1) * Time.deltaTime;
    }

    bool isConnected(SignalType signalType)
    {
        return getConnectedTransmitters(signalType).Count != 0;
    }

    HashSet<SignalTransmitter> getConnectedTransmitters(SignalType signalType)
    {
        HashSet<SignalTransmitter> connectedTransmitters = new HashSet<SignalTransmitter>();
        foreach (SignalTransmitter signalTransmitter in SignalTransmitter.Instances[signalType])
        {
            if (signalTransmitter.Receivers.Contains(this))
                if (!connectedTransmitters.Contains(signalTransmitter))
                    connectedTransmitters.Add(signalTransmitter);
            var connector = signalTransmitter.gameObject.GetComponent<TransmitterConnector>();
            if (connector != null)
                foreach (var connectedTransmitter in connector.Connections)
                    if (connectedTransmitter.Transmitter.Receivers.Contains(this))
                        if (!connectedTransmitters.Contains(connectedTransmitter.Transmitter))
                            connectedTransmitters.Add(connectedTransmitter.Transmitter);
        }
        return connectedTransmitters;
    }

    HashSet<SignalReceiver> getConnections(SignalType signalType)
    {
        HashSet<SignalTransmitter> connectedTransmitters = getConnectedTransmitters(signalType);
        HashSet<SignalReceiver> connections = new HashSet<SignalReceiver>();
        foreach (SignalTransmitter signalTransmitter in connectedTransmitters)
            foreach (SignalReceiver receiver in signalTransmitter.Receivers)
                if (!connections.Contains(receiver))
                    connections.Add(receiver);

        if (connections.Contains(this))
            connections.Remove(this);

        return connections;
    }
}
