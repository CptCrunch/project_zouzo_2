using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrbSpawner : MonoBehaviour {
    protected OrbSpawner() { }
    private static OrbSpawner _instance = null;

    //Orb Spawn
    [Header("Orb Spawn")]
    public string orbTag = "Orb";
    public GameObject orbPrefab;

    public float spawnRate = 1.5f;
    private int random;

    private GameObject[] spawnPoints = new GameObject[4];
    public Spawnpoint[] spawnPointsCheck = new Spawnpoint[4];

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

        InvokeRepeating("SpawnOrbs", 0.5f , spawnRate);
    }
	
    private void SpawnOrbs()
    {
        random = Random.Range(0, spawnPoints.Length);
        if (!spawnPointsCheck[random].active)
        {
            spawnPointsCheck[random].orbPrefab = Instantiate(orbPrefab, spawnPointsCheck[random].spawnPoint.transform.position, Quaternion.identity) as GameObject;
            spawnPointsCheck[random].active = true;
        }
    }

    public static OrbSpawner Instance { get { return OrbSpawner._instance == null ? new OrbSpawner() : OrbSpawner._instance; } }
}

public struct Spawnpoint
{
    public GameObject spawnPoint;
    public GameObject orbPrefab;
    public int index;
    public bool active;
}
