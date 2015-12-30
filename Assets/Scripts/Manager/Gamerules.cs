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
    [HideInInspector]
    public string[] chosenCharacters = new string[4];
    #endregion

    void Awake() {
        // Dont destroy this object on loading a new level
        DontDestroyOnLoad(gameObject);

        if (_instance == null) { _instance = this; }
    }
    
    // Create All Player when the level loads
    /*void OnLevelWasLoaded() {

        int character = 0;
        int counter = 0;

        //Get all objecticts with the given tag
        playerSpawn = GameObject.FindGameObjectsWithTag(spawnTag);

        // instantiate Player-Prefabs for each listed Player on a Spawnpoint
        if (playerAmmount > 0)
        {
            foreach (string z in chosenCharacters)
            {
                if (z == "Earth")
                {
                    character = counter;
                    print(character);
                }
                else if (z == "Jupiter")
                {
                    character = counter;
                    print(character);
                }
                else if (z == "Saturn")
                {
                    character = counter;
                    print(character);
                }
                else if (z == "Sun")
                {
                    character = counter;
                    print(character);
                }

                GameObject go = Instantiate(playerPrefab[character], playerSpawn[Random.Range(0, playerSpawn.Length)].transform.position, Quaternion.identity) as GameObject;

                // set the axis of the Prefab
                if (PlayerSelection._instance.controller[counter] == "KB")
                {
                    ChangeAxis("KB", go);
                }

                if (PlayerSelection._instance.controller[counter] == "P1")
                {
                    ChangeAxis("P1", go);
                }

                if (PlayerSelection._instance.controller[counter] == "P2")
                {
                    ChangeAxis("P2", go);
                }

                if (PlayerSelection._instance.controller[counter] == "P3")
                {
                    ChangeAxis("P3", go);
                }

                if (PlayerSelection._instance.controller[counter] == "P4")
                {
                    ChangeAxis("P4", go);
                }

                counter++;
            }
        }
        
    }*/

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


    [System.Serializable]
    public class ButtonLayout
    {
        public string jump;
        public string basicAttack;
        public string ability_1;
        public string ability_2;
        public string ability_3;
    }
}
