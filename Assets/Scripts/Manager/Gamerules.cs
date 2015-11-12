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

    // Testing 
    [HideInInspector]
    public List<string> connectedControllers = new List<string>();
    public TestPlayer[] testPlayers = new TestPlayer[5];
    #endregion

    void Awake() {
        // Dont destroy this object on loading a new level
        DontDestroyOnLoad(gameObject);

        if (_instance == null) { _instance = this; }
    }
    
    // Create All Player when the level loads
    void OnLevelWasLoaded() {

        //Get all objecticts with the given tag
        playerSpawn = GameObject.FindGameObjectsWithTag(spawnTag);

        // instantiate Player-Prefabs for each listed Player on a Spawnpoint
        if (playerAmmount > 0) {
            for (int i = 0; i < playerAmmount; i++) {
                GameObject go = Instantiate(playerPrefab[i], playerSpawn[Random.Range(0, playerSpawn.Length)].transform.position, Quaternion.identity) as GameObject;

                // set the axis of the Prefab
                if (PlayerSelection._instance.controller[i] == "KB") {
                    ChangeAxis("KB", go); }

                if(PlayerSelection._instance.controller[i] == "P1") {
                    ChangeAxis("P1", go); }

                if(PlayerSelection._instance.controller[i] == "P2") {
                    ChangeAxis("P2", go); }

                if(PlayerSelection._instance.controller[i] == "P3") {
                    ChangeAxis("P3", go); }

                if(PlayerSelection._instance.controller[i] == "P4") {
                    ChangeAxis("P4", go); }
            }
        }
    }

    private void ChangeAxis(string axis, GameObject obj) {

        // Change the player axis and name
        obj.GetComponent<Player>().playerAxis = axis;
        obj.name = axis + "_Player";

        // Set prefab clour and Text
        switch(axis)
        {
            case "P1":
                obj.GetComponent<Renderer>().material.color = testPlayers[0].color;
                obj.GetComponentInChildren<TextMesh>().text = testPlayers[0].name;
                break;

            case "P2":
                obj.GetComponent<Renderer>().material.color = testPlayers[1].color;
                obj.GetComponentInChildren<TextMesh>().text = testPlayers[1].name;
                break;

            case "P3":
                obj.GetComponent<Renderer>().material.color = testPlayers[2].color;
                obj.GetComponentInChildren<TextMesh>().text = testPlayers[2].name;
                break;

            case "P4":
                obj.GetComponent<Renderer>().material.color = testPlayers[3].color;
                obj.GetComponentInChildren<TextMesh>().text = testPlayers[3].name;
                break;

            case "KB":
                obj.GetComponent<Renderer>().material.color = testPlayers[4].color;
                obj.GetComponentInChildren<TextMesh>().text = testPlayers[4].name;
                break;
        }       
    }
    
    [System.Serializable]
    public class TestPlayer
    {
        public string name;
        public Color color;
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
