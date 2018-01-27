using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Population : MonoBehaviour {


    [SerializeField]
    private List<Minion> allMinionsList;
    [SerializeField]
    private List<SignalTransmitter> signalTransmitters;


    private void Awake()
    {
        signalTransmitters = new List<SignalTransmitter>();
        allMinionsList = new List<Minion>();
        SignalTransmitter.OnCreated += AddSignalTransmitter;
        SignalTransmitter.OnDestroyed += RemoveSignalTransmitter;
        Minion.OnCreated += AddMinion;
        Minion.OnDestroyed += RemoveMinion;
    }


    private void Start()
    {
        
    }

    private void OnDestroy()
    {
        SignalTransmitter.OnCreated -= AddSignalTransmitter;
        SignalTransmitter.OnDestroyed -= RemoveSignalTransmitter;
    }

    public Minion[] GetAllMinions()
    {
        return allMinionsList.ToArray();
    }
    private void AddMinion(Minion minion)
    {
        minion.SetPopulation(this);
        allMinionsList.Add(minion);
    }
    private void RemoveMinion(Minion minion)
    {
        allMinionsList.Remove(minion);
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
