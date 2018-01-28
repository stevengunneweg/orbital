using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SateliteFactory : MonoBehaviour
{

    public enum SatelliteType
    {
		Transmit,
        Attack,
        Defense,
        SelfRepairing
    }

    public float TypeToCost(SatelliteType type)
    {
        switch (type)
        {
            case SatelliteType.SelfRepairing:
                return 300f;
            case SatelliteType.Attack:
                return 125f;
            case SatelliteType.Defense:
                return 200f;
            case SatelliteType.Transmit:
            default:
                return 100f;
        }
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

    public static GameObject From(SatelliteType type, int teamId = defaultTeamId)
    {
        var _base = Base(teamId, Factory.TypeToCost(type));

        switch (type)
        {
            case SatelliteType.Attack:
                MakeAttackSatellite(_base);
                break;
            case SatelliteType.SelfRepairing:
                SelfRepairingTransmitSatelite(_base);
                break;
            case SatelliteType.Defense:
                MakeDefenseSatelite(_base);
                break;
			case SatelliteType.Transmit:
            default:
                MakeTransmitSatelite(_base);
                break;
        }

        return _base;
    }

    private static GameObject AddChild(GameObject _base, GameObject child)
    {
        var parent = _base.transform;
        var instance = Instantiate(child, parent, false);
        return instance;
    }

	private static GameObject Base(int teamId, float cost)
    {
        var instance = Instantiate(Factory.sateliteBasePrefab);
        instance.name = Factory.sateliteBasePrefab.name;
		instance.transform.position = new Vector3(1, 1, 1) * 10000;
        var satellite = instance.GetComponent<Satelite>();
        satellite.SetValues(new SateliteValues(cost, 0.5f, 0.05f));
        satellite.SetTeamId(teamId);
        return instance;
    }
    private static GameObject SetColor(GameObject _base, GameObject _graphics)
    {
        Color color = ((Color.white * 0.2f) + ((_base.GetComponent<Satelite>().GetValues().TeamId == 1) ? Color.white : Color.red) * 0.8f);
        _graphics.transform.GetChild(0).GetComponent<Renderer>().material.color = color;

        return _graphics;
    }

	[Header("Transmit Satelite Values")]
    public GameObject graphics_satellite_trans;
    public Material coneMaterial;
	public float broadcastRadius = 15;

	public static void MakeTransmitSatelite(GameObject _base)
    {
        _base.name = "Transmit Satelite";

		// Add Transmit script
		SignalTransmitter transmitter = _base.AddComponent<SignalTransmitter>();
		transmitter.broadcastRadius = Factory.broadcastRadius;
		transmitter.signalType = SignalType.Internet;
		_base.AddComponent<TransmitterConnector>();

		// Add cone of shame
		ConeFactory coneFactory = new ConeFactory();
		coneFactory.numVertices = 20;
		coneFactory.radiusTop = 0f;
		coneFactory.radiusBottom = Mathf.Tan(Factory.broadcastRadius) / 2;
		coneFactory.length = 1f;
		GameObject cone = coneFactory.ManufactureCone();
		// Set parent, position and rotation
		cone.transform.parent = _base.transform;
		cone.transform.localPosition = Vector3.zero;
		cone.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
		// Set Material
		cone.GetComponent<MeshRenderer>().sharedMaterial = Factory.coneMaterial;
		// Hide cone initially
		cone.GetComponent<Renderer>().enabled = false;
        SetColor(_base, AddChild(_base, Factory.graphics_satellite_trans));
    }

    [Header("Attack Satelite Values")]
    public GameObject graphics_satellite_att;
    public GameObject railgunBulletPrefab;
    public float railgunRotationSpeed = 1f;
    public float railgunReloadDuration = 1f;

	public static void MakeAttackSatellite(GameObject _base)
    {
        _base.name = "Attack Satelite";

        var railgun = _base.AddComponent<Attack>();
        railgun.rotationSpeed = Factory.railgunRotationSpeed;
        railgun.reloadDuration = Factory.railgunReloadDuration;
        railgun.bulletPrefab = Factory.railgunBulletPrefab;

        SetColor(_base, AddChild(_base, Factory.graphics_satellite_att));
    }

    [Header("Defensive Satelite Values")]
    public GameObject graphics_satellite_def;
    public int defensiveExtraShieldCount = 3;

    public static void MakeDefenseSatelite(GameObject _base)
    {
        _base.name = "Defense Satelite";

        //Add 50 health 'defensiveExtraShieldCount' times
        for (int i = 0; i < Factory.defensiveExtraShieldCount; i++)
        {
            _base.AddComponent<Shielding>();
        }

        SetColor(_base, AddChild(_base, Factory.graphics_satellite_def));
    }

    [Header("Repair Satelite Values")]
    public GameObject graphics_satellite_repair;

    public static void SelfRepairingTransmitSatelite(GameObject _base)
    {
        _base.name = "Self Repairing Transmit";
        MakeTransmitSatelite(_base);
        _base.AddComponent<Repairing>();
        SetColor(_base, AddChild(_base, Factory.graphics_satellite_att));
    }
}
