using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SateliteFactory : MonoBehaviour {
    
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

    [Header("Base")]
    public GameObject sateliteBasePrefab;

    private static GameObject Base()
    {
        var instance = Instantiate(Factory.sateliteBasePrefab);
        instance.name = "Default Satelite";
        instance.transform.position = new Vector3(1, 1, 1) * 10000;
        return instance;
    }

    public static GameObject FabricateDefaultSatelite()
    {
        var instance = Base();
        return instance;
    }


    [Header("Indestructable Satelite Values")]
    public int indestructableHealth = 9999;

    public static GameObject FabricateIndestructableSatelite()
    {
        var instance = Base();
        instance.GetComponent<Health>().IncreaseHealth(Factory.indestructableHealth);
        return instance;
    }

    [Header("Railgun Satelite Values")]
    public GameObject railgunBulletPrefab;
    public float railgunRotationSpeed = 1f;
    public float railgunReloadDuration = 1f;

    public static GameObject FabricateRailgunSatelite()
    {
        var instance = Base();

        var railgun = instance.AddComponent<RailgunModule>();
        railgun.rotationSpeed = Factory.railgunRotationSpeed;
        railgun.reloadDuration = Factory.railgunReloadDuration;
        railgun.bulletPrefab = Factory.railgunBulletPrefab;

        return instance;
    }
}
