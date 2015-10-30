using UnityEngine;
using System.Collections;

public class Gamerules : MonoBehaviour {

    public static Gamerules _instance;

    #region Gamerules Variable
    [Header("Game Rules")]
    [Range (0,4)]
    public int playerAmmount = 0;
    [Tooltip("Defines the maximal use of an ability")]
    public uint abilityLimit;
    public float itemSpawnrate;
    [Tooltip("Time between player death and spawn")]
    public float timeDeathSpawn;
    [Range(0,100)]
    public float damageModifier;
    public Gamemode[] gameModeList = new Gamemode[2];
    public GameObject playerPrefab;
    public Transform[] playerSpawn = new Transform[4];
    #endregion

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (_instance == null) { _instance = this; }
    }
    
    //Create All Player
    void OnLevelWasLoaded()
    {
        if (playerAmmount > 0)
        {
            for (int i = 0; i < playerAmmount; i++)
            {
                GameObject go = Instantiate(playerPrefab, playerSpawn[i].position, Quaternion.identity) as GameObject;
                go.GetComponent<Player>().playerAxis = "P" + (i + 1);
                go.name = "P" + (i + 1) + "_Player";
            }
        }
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
