using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SateliteFactory : MonoBehaviour
{

    public enum SatelliteType
    {
        Default,
        Railgun,
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

    [Header("Graphics")]
    public GameObject graphics_satellite_att;
    public GameObject graphics_satellite_def;
    public GameObject graphics_satellite_gun;

    public static GameObject From(SatelliteType type, int teamId = defaultTeamId)
    {
        var _base = Base(teamId);

        switch (type)
        {
            case SatelliteType.Railgun:
                MakeRailgun(_base);
                break;
            case SatelliteType.Default:
            default:
                break;
        }

        return _base;
    }

	private static GameObject Base(int teamId = defaultTeamId)
    {
        var instance = Instantiate(Factory.sateliteBasePrefab);
        instance.name = "Default Satelite";
		instance.transform.position = new Vector3(1, 1, 1) * 10000;
		instance.GetComponent<Satelite>().SetTeamId(teamId);
        return instance;
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
    }
}
