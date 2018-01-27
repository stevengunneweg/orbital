using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Population : MonoBehaviour {
    [SerializeField]
    private Minion[] allMinions;
    [SerializeField]
    private List<SignalTransmitter> signalTransmitters;

    private void Start()
    {
        signalTransmitters = new List<SignalTransmitter>();
        SignalTransmitter.OnCreated += AddSignalTransmitter;
        SignalTransmitter.OnDestroyed += RemoveSignalTransmitter;
        foreach (var m in allMinions)
        {
            m.SetPopulation(this);
        }
    }

    private void OnDestroy()
    {
        SignalTransmitter.OnCreated -= AddSignalTransmitter;
        SignalTransmitter.OnDestroyed -= RemoveSignalTransmitter;
    }

    public Minion[] GetAllMinions()
    {
        return this.allMinions;
    }

    private void AddSignalTransmitter(SignalTransmitter st)
    {
        signalTransmitters.Add(st);
        st.SetPopuplation(this);
    }
    private void RemoveSignalTransmitter(SignalTransmitter st)
    {
        signalTransmitters.Remove(st);
    }

    public List<SignalTransmitter> ActiveSatellites()
    {
        return signalTransmitters.Where(st => st.GetComponent<Satelite>().SatelliteActivated).ToList();
    }

    public int NrOfSatellites()
    {
        return ActiveSatellites().Count;
    }
}
