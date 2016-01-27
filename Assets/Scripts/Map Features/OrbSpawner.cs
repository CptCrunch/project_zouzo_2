using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrbSpawner : MonoBehaviour {

    //Orb Spawn
    [Header("Orb Spawn")]
    public string orbTag = "Orb";
    public GameObject orbPrefab;

    private GameObject[] spawnPoints = new GameObject[4];
    private Spawnpoint[] spawnPointsCheck = new Spawnpoint[4];

    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag(orbTag);

        int count = 0;
        foreach(GameObject item in spawnPoints)
        {
            spawnPointsCheck[count].spawnPoint = item;
            spawnPointsCheck[count].index = 0;
            spawnPointsCheck[count].active = false;
            count++;
        }
    }
	
    private IEnumerator SpawnOrbs(float time)
    {
        while(Gamerules._instance.Running)
        {
            int random = Random.Range(0, spawnPoints.Length);
            if(!spawnPointsCheck[random].active)
            {
                GameObject go = Instantiate(orbPrefab, spawnPointsCheck[random].spawnPoint.transform.position, Quaternion.identity) as GameObject;
                spawnPointsCheck[random].active = true;
            }
        }

        return null;
    }

}

public struct Spawnpoint
{
    public GameObject spawnPoint;
    public int index;
    public bool active;
}
