using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SateliteFactory : MonoBehaviour
{

    public enum SatelliteType
    {
		Transmit,
        Attack,
        Armored,
        SelfRepairing
    }

    private static SateliteFactory _factory;
    public static SateliteFactory Factory
    {
        get
        {
            if (_factory == null)
                _factory = Hierarchy.GetComponentWithTag<SateliteFactory>();
            return _factory;
        }
    }
	public const int defaultTeamId = 1;

    [Header("Base")]
    public GameObject sateliteBasePrefab;

    public static Satelite From(SatelliteType type, int teamId = defaultTeamId)
    {
        var satelite = InstantiateSaleliteBase(teamId);

        switch (type)
        {
            case SatelliteType.Attack:
                MakeAttackSatellite(satelite);
                SetColor(teamId, AddChild(satelite, Factory.graphics_satellite_att));
                satelite.GetValues().SetCost(175);
                break;
            case SatelliteType.SelfRepairing:
                MakeTransmitSatelite(satelite);
                MakeSelfRepairingTransmitSatelite(satelite);
                SetColor(teamId, AddChild(satelite, Factory.graphics_satellite_repair));
                satelite.GetValues().SetCost(300);
                break;
            case SatelliteType.Armored:
                MakeTransmitSatelite(satelite);
                MakeArmoredSatelite(satelite);
                SetColor(teamId, AddChild(satelite, Factory.graphics_satellite_def));
                satelite.GetValues().SetCost(250);
                break;
            case SatelliteType.Transmit:
            default:
                MakeTransmitSatelite(satelite);
                SetColor(teamId, AddChild(satelite, Factory.graphics_satellite_trans));
                satelite.GetValues().SetCost(125);
                break;
        }

        return satelite;
    }

    private static GameObject AddChild(Satelite satelite, GameObject child)
    {
        var parent = satelite.transform;
		return Instantiate(child, parent, false);
    }
    private static GameObject SetColor(int teamid, GameObject _graphics)
    {
        Color color = ((Color.white * 0.2f) + ((teamid == 1) ? Color.white : Color.red) * 0.8f);
        _graphics.transform.GetChild(0).GetComponent<Renderer>().material.color = color;

        return _graphics;
    }

    private static Satelite InstantiateSaleliteBase(int teamId)
    {
        var instance = Instantiate(Factory.sateliteBasePrefab);
        instance.name = Factory.sateliteBasePrefab.name;
		instance.transform.position = new Vector3(1, 1, 1) * 10000;

        var satellite = instance.GetComponent<Satelite>();
        satellite.SetValues(new SateliteValues(100, 0.5f, 0.05f));
        satellite.SetTeamId(teamId);

        return satellite;
    }

	[Header("Transmit Satelite Values")]
    public GameObject graphics_satellite_trans;
    public Material coneMaterial;
	public float broadcastRadius = 15;

	private static void MakeTransmitSatelite(Satelite satelite)
    {
        satelite.name = "Transmit Satelite";

		// Add Transmit script
		SignalTransmitter transmitter = satelite.gameObject.AddComponent<SignalTransmitter>();
		transmitter.broadcastRadius = Factory.broadcastRadius;
		transmitter.signalType = SignalType.Internet;
		satelite.gameObject.AddComponent<TransmitterConnector>();

		// Add cone of shame
		ConeFactory coneFactory = new ConeFactory();
		coneFactory.numVertices = 20;
		coneFactory.radiusTop = 0f;
		coneFactory.radiusBottom = Mathf.Tan(Factory.broadcastRadius) / 2;
		coneFactory.length = 1f;
		GameObject cone = coneFactory.ManufactureCone();
		// Set parent, position and rotation
		cone.transform.parent = satelite.transform;
		cone.transform.localPosition = Vector3.zero;
		cone.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
		// Set Material
		cone.GetComponent<MeshRenderer>().sharedMaterial = Factory.coneMaterial;
		// Hide cone initially
		cone.GetComponent<Renderer>().enabled = false;
    }

    [Header("Attack Satelite Values")]
    public GameObject graphics_satellite_att;
    public GameObject railgunBulletPrefab;
    public float railgunRotationSpeed = 1f;
    public float railgunReloadDuration = 1f;

    private static void MakeAttackSatellite(Satelite satelite)
    {
        satelite.name = "Attack Satelite";

        var railgun = satelite.gameObject.AddComponent<Attack>();
        railgun.rotationSpeed = Factory.railgunRotationSpeed;
        railgun.reloadDuration = Factory.railgunReloadDuration;
        railgun.bulletPrefab = Factory.railgunBulletPrefab;
    }

    [Header("Armored Satelite Values")]
    public GameObject graphics_satellite_def;
    public int armoredExtraShieldCount = 3;

    private static void MakeArmoredSatelite(Satelite satelite)
    {
        satelite.name = "Armored Satelite";
        for (int i = 0; i < Factory.armoredExtraShieldCount; i++)
        {
            satelite.gameObject.AddComponent<Armored>();
        }
    }

    [Header("Repair Satelite Values")]
    public GameObject graphics_satellite_repair;

    private static void MakeSelfRepairingTransmitSatelite(Satelite satelite)
    {
        satelite.name = "Self Repairing Transmit";
        satelite.gameObject.AddComponent<Repairing>();
    }
}
