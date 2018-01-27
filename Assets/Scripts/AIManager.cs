using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {

    float difficultyTimer_max = 200;
    float difficultyTimer_cur = 0;
    float stageTimer_max = 5;
    float stageTimer_cur = 0;
    int stage = 1;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (difficultyTimer_cur <= difficultyTimer_max)
            difficultyTimer_cur += Time.deltaTime;
        if (stageTimer_cur <= stageTimer_max)
            stageTimer_cur += Time.deltaTime;
        else
        {
            stage++;
            if(stage<=15)
                stageTimer_cur = 0;
        }
        if (CanISpawn())
            SpawnSatallite();

    }

    void SpawnSatallite()
    {
        GameObject go = SateliteFactory.From(SateliteFactory.SatelliteType.Railgun, 2);
        Vector3 startPosition = Quaternion.Euler(0,0,(Random.value*360))* (Vector3.up * 2.7f);
        Vector3 endPosition = startPosition+ (startPosition.normalized*(0.3f+Random.value*4));
        List<Vector3> trajectoryList = new List<Vector3>();
        trajectoryList.Add(startPosition);
        trajectoryList.Add(endPosition);
        go.GetComponent<Satelite>().Spawn(trajectoryList);
    }

    bool CanISpawn()
    {
        //Debug.Log("Random.value: " + Random.value +"<"+ SpawnChance()+" stage: "+stage+" true: "+(Random.value < SpawnChance()));
        return Random.value < SpawnChance();
    }
    float SpawnChance()
    {
        return (0.001f * (float)(stage));
    }
}
