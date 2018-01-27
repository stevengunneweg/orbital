using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {

    int secondsTillNextEnemy = 30;

    private void Start()
    {
        StartCoroutine(WaitForEnemy());
    }


    IEnumerator WaitForEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(secondsTillNextEnemy);
            secondsTillNextEnemy -= 2;
            if (secondsTillNextEnemy < 3) secondsTillNextEnemy = 3;
            SpawnSatallite();
        }
        

    }

    void SpawnSatallite()
    {
        GameObject go = SateliteFactory.From(SateliteFactory.SatelliteType.Attack, 2);
        Vector3 startPosition = Quaternion.Euler(0,0,(Random.value*360))* (Vector3.up * 2.7f);
        Vector3 endPosition = startPosition+ (startPosition.normalized*(0.3f+Random.value*4));
        List<Vector3> trajectoryList = new List<Vector3>();
        trajectoryList.Add(startPosition);
        trajectoryList.Add(endPosition);
        go.GetComponent<Satelite>().Spawn(trajectoryList);
    }
}
