using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gamerules : MonoBehaviour {

    public static Gamerules _instance;

    [HideInInspector] public bool Running;

    [Tooltip("all scenes the Gamerules script will detect as a stage")]
    public string[] registerdStages = new string[1];

    #region Gamerules Variable
    [Header("Game Rules")]

    [Range (0,4)]
    public int playerAmmount = 0;

    public float playerMaxHealth = 0;

    [Tooltip("Defines the maximal use of an ability")]
    public uint abilityLimit;

    [Tooltip("Time between player death and spawn")]
    public float timeDeathSpawn;

    [Range(0,200), Tooltip("In Percent (100% = normal Damage")]
    public float damageModifier = 100.0f;

    public int lifeLimit;

    [Tooltip("Time in Minutes")]
    public float timeLimit = 10;
    public float currentTime;

    // Player/Controller Selection
    [Header("Player Selection")]
    public GameObject[] playerPrefabs = new GameObject[4] { null, null, null, null };

    private GameObject[] playerSpawn;
    private int[] randomSpawnOrder = new int[4] { 0, 1, 2, 3 };
    [Tooltip("Spawnpoints must have this given tag")]
    public string spawnTag;

    [Header("Debug")]
    public DebugValues Tags;
    #endregion

    #region character variables
    [HideInInspector] public CharacterPicture[] charPics = new CharacterPicture[4];
     public GameObject[] playerOnStage = new GameObject[4] { null, null, null, null };
    #endregion

    void Awake()
    {
        // Dont destroy this object on loading a new level
        DontDestroyOnLoad(gameObject);
        // set singelton instance
        if (_instance == null) { _instance = this; }
    }

    void Start()
    {
        #region Debug
        Tags.ResetValues();
        CustomDebug.Active = Tags.Active;
        CustomDebug.EnableTag("Main", Tags.Main);
        CustomDebug.EnableTag("Testing", Tags.Testing);
        CustomDebug.EnableTag("Player", Tags.Player);
        CustomDebug.EnableTag("Damage", Tags.Damage);
        CustomDebug.EnableTag("Spells", Tags.Spells);
        CustomDebug.EnableTag("Cooldowns", Tags.Cooldown);
        CustomDebug.EnableTag("UI", Tags.UI);
        CustomDebug.EnableTag("Animation", Tags.Animation);
        CustomDebug.EnableTag("Condition", Tags.Condition);
        CustomDebug.EnableTag("Contrioles", Tags.Controles);
        CustomDebug.EnableTag("MapFeature", Tags.Controles);
        #endregion

    }

    void Update()
    {
        if(Running)
        {
            currentTime = timeLimit - Time.deltaTime;
            if (currentTime <= 0) EndGame();
        }
    }

    // Create All Player when the level loads
    void OnLevelWasLoaded()
    {
        // --- [ stage chack ] ---
        bool isStage = false;
        // check if scene is a registered stage
        foreach (string stage in registerdStages) { if (stage == Application.loadedLevelName) { isStage = true; } }
        // continue if scene is registered
        if (isStage)
        {
            OnStage();
        }

         StartCoroutine(WaitCoroutine());
    }

    public void OnStage()
    {
        // find all spawns
        playerSpawn = GameObject.FindGameObjectsWithTag(spawnTag);

        // --- [ set random spawn point order ] ---
        Util.MixArray(randomSpawnOrder);

        // --- [ spawn player ] ---
        int momSpawn = 0;
        foreach (CharacterPicture player in charPics)
        {
            // check if object isn't null
            if (player != null)
            {
                momSpawn++;
                // get a random spornpoint

                // check player type
                switch (player.Character)
                {
                    case "Earth":
                        Util.IncludeGameObject(playerOnStage, Instantiate(playerPrefabs[0], playerSpawn[randomSpawnOrder[momSpawn]].transform.position, Quaternion.identity) as GameObject);
                        break;
                    case "Sun":
                        Util.IncludeGameObject(playerOnStage, Instantiate(playerPrefabs[1], playerSpawn[randomSpawnOrder[momSpawn]].transform.position, Quaternion.identity) as GameObject);
                        break;
                    case "Saturn":
                        Util.IncludeGameObject(playerOnStage, Instantiate(playerPrefabs[2], playerSpawn[randomSpawnOrder[momSpawn]].transform.position, Quaternion.identity) as GameObject);
                        break;
                    case "Jupiter":
                        Util.IncludeGameObject(playerOnStage, Instantiate(playerPrefabs[3], playerSpawn[randomSpawnOrder[momSpawn]].transform.position, Quaternion.identity) as GameObject);
                        break;
                }
            }
        }
    }

    private IEnumerator WaitCoroutine()
    {
        foreach(GameObject go in playerOnStage)
        {
            if (go != null)
            {
                go.GetComponent<Player>().gamerulesDisabled = true;
                go.GetComponent<Player>().flipEnable = false;
                CustomDebug.Log(go.name + ", " +go.GetComponent<Player>().gamerulesDisabled, "Testing");
            }
        }

        //TODO: Implement UI Count
        CustomDebug.Log(3, "Testing");
        yield return new WaitForSeconds(1);
        CustomDebug.Log(2, "Testing");
        yield return new WaitForSeconds(1);
        CustomDebug.Log(1, "Testing");
        yield return new WaitForSeconds(1);
        CustomDebug.Log("Go", "Testing");
        yield return new WaitForSeconds(1);

        foreach (GameObject go in playerOnStage)
        {            
            if (go != null)
            {
                go.GetComponent<Player>().gamerulesDisabled = false;
                go.GetComponent<Player>().flipEnable = true;
                CustomDebug.Log(go.name + ", " + go.GetComponent<Player>().gamerulesDisabled, "Testing");
            }
        }

        Running = true;
        yield return null;
    }
    
    private void ChangeAxis(string axis, GameObject obj) {
        // Change the player axis and name
        obj.GetComponent<Player>().playerAxis = axis;
        obj.name = axis + "_Player";
    }

    public void PlayerDeath()
    {
        EndGame();
    }

    public void EndGame()
    {
        Running = false;
    }
}
