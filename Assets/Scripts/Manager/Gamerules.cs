using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gamerules : MonoBehaviour {

    public static Gamerules _instance;

    #region Gamerules Variable
    [Header("Game Rules")]

    [Range (0,4)]
    public int playerAmmount = 0;

    public float playerMaxHealth = 0;

    [Tooltip("Defines the maximal use of an ability")]
    public uint abilityLimit;

    public float itemSpawnrate;

    [Tooltip("Time between player death and spawn")]
    public float timeDeathSpawn;

    [Range(0,200), Tooltip("In Percent (100% = normal Damage")]
    public float damageModifier;

    public Gamemode[] gameModeList = new Gamemode[2];

    // Player/Controller Selection
    [Header("Player Selection")]
    public GameObject[] playerPrefab = new GameObject[4];
    private GameObject[] playerSpawn = new GameObject[4];
    [Tooltip("Spawnpoints must have this given tag")]
    public string spawnTag;

    [HideInInspector]
    public Dictionary<string, bool> chosenPics = new Dictionary<string, bool>();
    [HideInInspector]
    public Dictionary<string, string> controllerToPlayer = new Dictionary<string, string>();
    [HideInInspector]
    public GameObject[] spawnedPlayer = new GameObject[4];

    [Header("Debug")]
    public bool LogingToConsole = false;

    public GameObject obj;
    #endregion

    void Awake() {
        // Dont destroy this object on loading a new level
        DontDestroyOnLoad(gameObject);

        CustomDebuger.Active = LogingToConsole;

        if (_instance == null) { _instance = this; }
    }

    // Create All Player when the level loads
    void OnLevelWasLoaded() {
        playerSpawn = GameObject.FindGameObjectsWithTag(spawnTag);

        foreach(var item in chosenPics)
        {
            if(item.Key == "Earth" && item.Value == true)
            {
                spawnedPlayer[0] = Instantiate(playerPrefab[0], playerSpawn[Random.Range(0, playerSpawn.Length)].transform.position, Quaternion.identity) as GameObject;
            }

            if (item.Key == "Saturn" && item.Value == true)
            {
                spawnedPlayer[1] = Instantiate(playerPrefab[1], playerSpawn[Random.Range(0, playerSpawn.Length)].transform.position, Quaternion.identity) as GameObject;
            }

            if (item.Key == "Jupiter" && item.Value == true)
            {
                spawnedPlayer[2] = Instantiate(playerPrefab[2], playerSpawn[Random.Range(0, playerSpawn.Length)].transform.position, Quaternion.identity) as GameObject;
            }

            if (item.Key == "Sun" && item.Value == true)
            {
                spawnedPlayer[3] = Instantiate(playerPrefab[3], playerSpawn[Random.Range(0, playerSpawn.Length)].transform.position, Quaternion.identity) as GameObject;
            }
        }

        foreach(var i in controllerToPlayer)
        {
            switch(i.Key)
            {
                case "Earth":
                    AdjustAxis(i.Value, 0);
                    break;
                case "Jupiter":
                    AdjustAxis(i.Value, 1);
                    break;
                case "Saturn":
                    AdjustAxis(i.Value, 2);
                    break;
                case "Sun":
                    AdjustAxis(i.Value, 3);
                    break;
            }


        }
    }

    private void AdjustAxis(string value, int index)
    {
        switch (value)
        {
            case "KB":
                ChangeAxis("KB", spawnedPlayer[index]);
                break;
            case "P1":
                ChangeAxis("P1", spawnedPlayer[index]);
                break;
            case "P2":
                ChangeAxis("P2", spawnedPlayer[index]);
                break;
            case "P3":
                ChangeAxis("P3", spawnedPlayer[index]);
                break;
        }
    }

    private void ChangeAxis(string axis, GameObject obj) {

        // Change the player axis and name
        obj.GetComponent<Player>().playerAxis = axis;
        obj.name = axis + "_Player";
    }

    [System.Serializable]
    public class Gamemode
    {
        public string name;
        public int lifeLimit;
        [Range(0,100)]
        public float timeLimit;
    }
}
