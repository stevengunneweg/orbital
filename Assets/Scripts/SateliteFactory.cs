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

    public static GameObject FabricateDefaultSatelite()
    {
        var instance = Instantiate(Factory.sateliteBasePrefab);
        instance.name = "Default Satelite";
        return instance;
    }


    [Header("Indestructable Satelite Values")]
    public int indestructableHealth = 9999;

    public static GameObject FabricateIndestructableSatelite()
    {
        var instance = FabricateDefaultSatelite();
        instance.name = "Indestructable Satelite";
        instance.GetComponent<Health>().IncreaseHealth(Factory.indestructableHealth);
        return instance;
    }

	
}
