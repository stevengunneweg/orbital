using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(SignalTransmitter))]
public class TransmitterConnector : MonoBehaviour
{
    static Dictionary<SignalType, List<TransmitterConnector>> instances;
    public static Dictionary<SignalType, List<TransmitterConnector>> Instances
    {
        get
        {
            if (instances == null)
            {
                instances = new Dictionary<SignalType, List<TransmitterConnector>>();
                foreach (SignalType type in Enum.GetValues(typeof(SignalType)))
                    instances.Add(type, new List<TransmitterConnector>());
            }
            return instances;
        }
    }

    public SignalTransmitter Transmitter { get; private set; }
    public HashSet<TransmitterConnector> Connections { get; private set; }

    SignalType signalType { get { return Transmitter.SignalType; } }
    
    void Awake()
    {
        Transmitter = GetComponent<SignalTransmitter>();
        Connections = new HashSet<TransmitterConnector>();

        Instances[signalType].Add(this);
    }

    void OnDestroy()
    {
        Instances[signalType].Remove(this);
    }

    static bool isFirstObjectToUpdate = true;
    void Update()
    {
        isFirstObjectToUpdate = true;
    }

    void LateUpdate()
    {
        if (isFirstObjectToUpdate)
        {
            isFirstObjectToUpdate = false;

            foreach (var signalType in Instances.Keys)
            {
                var connections = groupConnectedTransmitters(signalType);
                foreach (var group in connections)
                    foreach (var transmitter in group.ToList())
                    {
                        transmitter.Connections = new HashSet<TransmitterConnector>(group);
                        transmitter.Connections.Remove(transmitter);
                    }
            }
        }
    }

    bool isInSight(HashSet<TransmitterConnector> others)
    {
        foreach (var transmitter in others)
            if (isInSight(transmitter))
                return true;

        return false;
    }

    bool isInSight(TransmitterConnector other)
    {
        Vector2 startPos = (Vector2)transform.position;
        Vector2 endPos = (Vector2)other.transform.position;
        Vector2 direction = endPos - startPos;
        var hits = Physics2D.RaycastAll(startPos, direction.normalized, direction.magnitude);
        return !hits.Any(hit => hit.transform != transform && hit.transform != other.transform);
    }

    List<HashSet<TransmitterConnector>> groupConnectedTransmitters(SignalType signals)
    {
        List<HashSet<TransmitterConnector>> groups = new List<HashSet<TransmitterConnector>>();
        foreach (var transmitter in Instances[signalType])
        {
            if (!transmitter.Transmitter.Activated)
                continue;

            List<int> connections = new List<int>();
            for (int i = 0; i < groups.Count; ++i)
                if (isInSight(groups[i]))
                    connections.Add(i);
            if (connections.Count == 0)
                groups.Add(new HashSet<TransmitterConnector> { transmitter });
            else
            {
                groups[connections[0]].Add(transmitter);
                for (int i = connections.Count - 1; i > 0; --i)
                {
                    foreach (var obj in groups[connections[i]])
                        groups[connections[0]].Add(obj);
                    groups.RemoveAt(connections[i]);
                }
            }
        }
        return groups;
    }
}
