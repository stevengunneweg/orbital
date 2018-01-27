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
            connectionScore += calculateScore(signalType) * Time.deltaTime * 2.0f;
        }
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
        {
            if (transmitter.gameObject.GetComponent<Satelite>().SatelliteActivated && 
                transmitter.ConnectedMinions.Contains(this))
            {

                return true;
            }
        }
           
        return false;
    }

    HashSet<Minion> getConnections(SignalType signalType)
    {
        HashSet<Minion> connections = new HashSet<Minion>();

        foreach (SignalTransmitter signalTransmitter in SignalTransmitter.Instances[signalType])
        {
            if (signalTransmitter.ConnectedMinions.Contains(this))
            {
                foreach (Minion receiver in signalTransmitter.ConnectedMinions)
                {
                    if (!connections.Contains(receiver))
                        connections.Add(receiver);
                }                    
            }                
        }        

        if (connections.Contains(this))
        {
            connections.Remove(this);
        }

        return connections;
    }

    public float GetCurrentScore()
    {
        return connectionScore;
    }
}
