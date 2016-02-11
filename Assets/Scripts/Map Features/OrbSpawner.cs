using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrbSpawner : MonoBehaviour {
    //Singleton
    private static OrbSpawner _instance = null;
    public static OrbSpawner Instance { get { return _instance; } }
    void Awake() { if(_instance == null) { _instance = this; } else { _instance = new OrbSpawner(); } }

    //Orb Spawn
    [Header("Orb Spawn")]
    public string orbTag = "Orb";
    public GameObject orbPrefab;
    public Sprite[] orbSprites;

    public float spawnRate = 1.5f;
    private int random;

    private GameObject[] spawnPoints = new GameObject[4];
    private AbilityOrbValues[] spawnPointsCheck = new AbilityOrbValues[4] { new AbilityOrbValues(null, null), new AbilityOrbValues(null, null), new AbilityOrbValues(null, null), new AbilityOrbValues(null, null)};

    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag(orbTag);

        CustomDebug.LogArray(orbSprites);

        int count = 0;
        foreach(GameObject item in spawnPoints)
        {
            spawnPointsCheck[count].SpawnPoint = item;
            spawnPointsCheck[count].Active = false;
            count++;
        }

        InvokeRepeating("SpawnOrbs", 0.5f , spawnRate);
    }
	
    private void SpawnOrbs()
    {
        random = Random.Range(0, spawnPoints.Length);
        if (!spawnPointsCheck[random].Active)
        {
            spawnPointsCheck[random].OrbPrefab = Instantiate(orbPrefab, spawnPointsCheck[random].SpawnPoint.transform.position, Quaternion.identity) as GameObject;
            spawnPointsCheck[random].OrbPrefab.GetComponent<AbilityOrb>().point = spawnPointsCheck[random];

            spawnPointsCheck[random].Active = true;
        }
    }  
}

    