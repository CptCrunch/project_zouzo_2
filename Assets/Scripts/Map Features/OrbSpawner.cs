using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrbSpawner : MonoBehaviour
{
    //Singleton
    private static OrbSpawner _instance = null;
    public static OrbSpawner Instance { get { return _instance; } }
    void Awake() { if(_instance == null) { _instance = this; } else { Destroy(gameObject); } }

    //Orb Spawn
    [Header("Orbs")]
    public string orbTag = "Orb";
    public GameObject orbPrefab;
    [Tooltip("maximum amount of orbs, at once, on the stage")]
    public int orbMax = 4;
    [HideInInspector] public int orbCount = 0;

    [Header("Spawns")]
    [Tooltip("time between collecting the orb/desapearing, and respawning a neu one")]
    public float spawnCooldown = 1.5f;
    [Tooltip("time it takes the orb to disapeare again")]
    public float spawnActiveTime = 1.5f;
    [Tooltip("the time when the second wave of arbs spawn (to get to the max amount of orbs)")]
    public float timeToStartSelfSpawn = 2f;
    [Tooltip("the spawns at with possition the first orbs will spawn")]
    public GameObject[] standardSpawns = new GameObject[4];
    [HideInInspector] public bool enableSelfSpawning = false;
    private GameObject[] spawnPoints;
    private string[] spawnableOrbs = new string[0];

    public Sprite[] orbSprites;

    public bool showDebugCooldowns = false;
    /*private int random;*/



    /*private AbilityOrbValues[] spawnPointsCheck = new AbilityOrbValues[4]
    { new AbilityOrbValues(null, null),new AbilityOrbValues(null, null), new AbilityOrbValues(null, null), new AbilityOrbValues(null, null) };*/

    /*void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag(orbTag);

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
    } */

    void Start()
    {
        // save spawnpoints
        spawnPoints = GameObject.FindGameObjectsWithTag(orbTag);

        // --- [ get all available spells ] ---
        for (int i = 0; i < AbilityManager.Instance.spells.Length; i++)
        {
            // check if spell is available
            if (AbilityManager.Instance.spells[i].active)
            {
                // check if spell isn't a basic
                if (AbilityManager.Instance.spells[i].name != "Basic")
                {
                    string[] saveOldOrb = spawnableOrbs;
                    spawnableOrbs = new string[spawnableOrbs.Length + 1];

                    for (int o = 0; o < spawnableOrbs.Length - 1; o++)
                    {
                        spawnableOrbs[o] = saveOldOrb[o];
                    }

                    spawnableOrbs[spawnableOrbs.Length - 1] = AbilityManager.Instance.spells[i].name;
                }
            }
        }

        foreach (GameObject spawn in spawnPoints)
        {
            spawn.GetComponent<OrbSpawn>().despawnTime = spawnActiveTime;
            spawn.GetComponent<OrbSpawn>().maxCooldown = spawnCooldown;
        }

        // --- [ create first orbs ] ---
        for (int i = 0; i < GameManager._instance.PlayerOnStageAmount; i++)
        {
            CreateOrbs(standardSpawns[i]);
        }
    }

    void Update()
    {
        // check if game is running
        if (GameManager._instance.Running)
        {
            // sub from timer
            if (timeToStartSelfSpawn > 0) { timeToStartSelfSpawn -= Time.deltaTime; }

            // exicute if timer is over
            else if (!enableSelfSpawning)
            {
                // enable self spawning
                enableSelfSpawning = true;

                // create more orbs
                while (orbCount < orbMax) { SpawnOrb(); }
            }
        }
    }

    public bool SpawnOrb()
    {
        int[] availableSpawns = new int[0];

        if (orbCount < orbMax)
        {
            int randomSpawn = 0;

            // get all spawns
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                // check if spawn is active
                if (spawnPoints[i].GetComponent<OrbSpawn>().Cooldown <= 0 && spawnPoints[i].GetComponent<OrbSpawn>().orb == null)
                {
                    // --- [ save avtive spawns ] ---
                    int[] saveOldSpawn = availableSpawns;
                    availableSpawns = new int[availableSpawns.Length + 1];
                    for (int o = 0; o < availableSpawns.Length - 1; o++) { availableSpawns[o] = saveOldSpawn[o]; }

                    availableSpawns[availableSpawns.Length - 1] = i;
                }
            }

            // random availableSpawn
            if (availableSpawns.Length > 0)
            {
                randomSpawn = availableSpawns[Random.Range(0, availableSpawns.Length - 1)];
                CreateOrbs(spawnPoints[randomSpawn]);
            }
        }
        return availableSpawns.Length > 0;
    }

    private void CreateOrbs(GameObject _spawn)
    {
        orbCount++;

        // create orb
        GameObject orb = Instantiate(orbPrefab, _spawn.transform.position, Quaternion.identity) as GameObject;

        // get random spell
        string randomSpell = spawnableOrbs[Random.Range(0, spawnableOrbs.Length)];
        // transfer spell type
        orb.GetComponent<AbilityOrb>().spellType = randomSpell;
        // set animation
        orb.GetComponent<Animator>().SetInteger("animationID", AbilityManager.Instance.GetSpellID(randomSpell));

        // transfer spawn
        orb.GetComponent<AbilityOrb>().spawn = _spawn.GetComponent<OrbSpawn>();

        // add orb
        _spawn.GetComponent<OrbSpawn>().SetNewOrb(orb);
    }
}

    