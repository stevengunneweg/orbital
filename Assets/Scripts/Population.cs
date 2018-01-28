using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Population : MonoBehaviour {


    [SerializeField]
    private List<Minion> allMinionsList;
    [SerializeField]
    private List<SignalTransmitter> signalTransmitters;
    private List<Satelite> satelites;
    private void Awake()
    {
        signalTransmitters = new List<SignalTransmitter>();
        allMinionsList = new List<Minion>();
        satelites = new List<Satelite>();
        SignalTransmitter.OnCreated += AddSignalTransmitter;
        SignalTransmitter.OnDestroyed += RemoveSignalTransmitter;
        Satelite.OnCreated += AddSatelite;
        Satelite.OnDestroyed += RemoveSatelite;
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
        Satelite.OnCreated -= AddSatelite;
        Satelite.OnDestroyed -= RemoveSatelite;
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

    private void AddSatelite(Satelite satelite)
    {
        satelites.Add(satelite);
    }
    private void RemoveSatelite(Satelite satelite)
    {
        satelites.Remove(satelite);
    }

    public List<Satelite> ActivePlayerSatellites()
    {
        return satelites.Where(st => st.GetComponent<Satelite>().SatelliteActivated && st.IsPlayer).ToList();
    }

    public int NrOfSatellites()
    {
        return ActivePlayerSatellites().Count;
    }

    public List<Satelite> GetAllSatelites()
    {
        return satelites;
    }
}
