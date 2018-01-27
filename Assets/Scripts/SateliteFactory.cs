using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SateliteFactory : MonoBehaviour
{

    public enum SatelliteType
    {
		Default,
		Transmit,
        Railgun,
        Shield,
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

    public static GameObject From(SatelliteType type, int teamId = defaultTeamId)
    {
        var _base = Base(teamId);

        switch (type)
        {
            case SatelliteType.Railgun:
                MakeRailgun(_base);
                AddAttGraphics(_base);
                break;
            case SatelliteType.SelfRepairing:
                SelfRepairingTransmitSatelite(_base);
                break;
            case SatelliteType.Shield:
                MakeDefenseSatelite(_base);
                break;
            case SatelliteType.Default:
			case SatelliteType.Transmit:
            default:
                MakeTransmitSatelite(_base);
                AddTransmissionGraphics(_base);
                break;
        }

        return _base;
    }

    [Header("Graphics")]
    public GameObject graphics_satellite_att;
    public GameObject graphics_satellite_def;
    public GameObject graphics_satellite_trans;

    public static void AddAttGraphics(GameObject _base)
    {
        AddGraphics(_base, Factory.graphics_satellite_att);
    }

    public static void AddDefGraphics(GameObject _base)
    {
        AddGraphics(_base, Factory.graphics_satellite_def);
    }

    public static void AddTransmissionGraphics(GameObject _base)
    {
        AddGraphics(_base, Factory.graphics_satellite_trans);
    }

    private static void AddGraphics(GameObject _base, GameObject graphics)
    {
        var parent = _base.transform;
        var instance = Instantiate(graphics, parent, false);
    }

	private static GameObject Base(int teamId = defaultTeamId)
    {
        var instance = Instantiate(Factory.sateliteBasePrefab);
        instance.name = "Default Satelite";
		instance.transform.position = new Vector3(1, 1, 1) * 10000;
		instance.GetComponent<Satelite>().SetTeamId(teamId);
        return instance;
    }

	[Header("Transmit Satelite Values")]
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
        _base.GetComponent<Satelite>().SetValues(new SateliteValues(100, 0.5f, 0.05f));
    }

	[Header("Indestructable Satelite Values")]
	public int indestructableHealth = 9999;

	public static void MakeIndestructableSatelite(GameObject _base)
	{
		_base.name = "Indestructable Satelite";
		_base.GetComponent<Health>().IncreaseHealth(Factory.indestructableHealth);
	}

    [Header("Railgun Satelite Values")]
    public GameObject railgunBulletPrefab;
    public float railgunRotationSpeed = 1f;
    public float railgunReloadDuration = 1f;

	public static void MakeRailgun(GameObject _base)
    {
        _base.name = "Railgun Satelite";

        // Check if it already has a railgun
        if (_base.GetComponent<RailgunModule>() != null)
            return;

        var railgun = _base.AddComponent<RailgunModule>();
        railgun.rotationSpeed = Factory.railgunRotationSpeed;
        railgun.reloadDuration = Factory.railgunReloadDuration;
        railgun.bulletPrefab = Factory.railgunBulletPrefab;
        _base.GetComponent<Satelite>().SetValues(new SateliteValues(150, 0.5f, 0.05f));
    }

    public static void MakeDefenseSatelite(GameObject _base)
    {
        _base.name = "Defense Satelite";

        // Check if it already has a railgun
        if (_base.GetComponent<RailgunModule>() != null)
            return;
        //Add 50 health 3 times
        for(int i=0; i<3; i++)
        {
            _base.AddComponent<Shielding>();
        }
        _base.GetComponent<Satelite>().SetValues(new SateliteValues(150, 0.5f, 0.05f));
    }

    public static void SelfRepairingTransmitSatelite(GameObject _base)
    {
        _base.name = "Self Repairing Transmit";
        MakeTransmitSatelite(_base);
        _base.AddComponent<Repairing>();
        _base.GetComponent<Satelite>().SetValues(new SateliteValues(200, 0.5f, 0.05f));
    }
}
