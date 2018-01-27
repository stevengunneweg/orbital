using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    private Population population;
    public void SetPopulation(Population pop)
    {
        this.population = pop;
    }

    float connectionScore = 0;
   

    void Update()
    {
        connectionScore = 0;
        foreach (SignalType signalType in Enum.GetValues(typeof(SignalType)))
        {
            connectionScore += calculateScore(signalType) * Time.deltaTime * 0.03f;
        }
    }

    // todo [KG] make the score calculation do fancy things
    float calculateScore(SignalType signalType)
    {
        if (!isConnected(signalType))
            return 0;

        return (getConnections(signalType).Count + 1);
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
            if (signalTransmitter.ConnectedMinions.Contains(this))
                if (!connectedTransmitters.Contains(signalTransmitter))
                    connectedTransmitters.Add(signalTransmitter);
            var connector = signalTransmitter.gameObject.GetComponent<TransmitterConnector>();
            if (connector != null)
                foreach (var connectedTransmitter in connector.Connections)
                    if (connectedTransmitter.Transmitter.ConnectedMinions.Contains(this))
                        if (!connectedTransmitters.Contains(connectedTransmitter.Transmitter))
                            connectedTransmitters.Add(connectedTransmitter.Transmitter);
        }
        return connectedTransmitters;
    }

    HashSet<Minion> getConnections(SignalType signalType)
    {
        HashSet<SignalTransmitter> connectedTransmitters = getConnectedTransmitters(signalType);
        HashSet<Minion> connectedMinions = new HashSet<Minion>();
        foreach (SignalTransmitter signalTransmitter in connectedTransmitters)
            foreach (Minion minion in signalTransmitter.ConnectedMinions)
                if (!connectedMinions.Contains(minion))
                    connectedMinions.Add(minion);

        if (connectedMinions.Contains(this))
            connectedMinions.Remove(this);

        return connectedMinions;
    }

    public float GetCurrentScore()
    {
        return connectionScore;
    }
}
