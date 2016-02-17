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

    [Range(0, 4)]
    public int playerAmmount = 0;

    public float playerMaxHealth = 0;

    [Tooltip("Defines the maximal use of an ability")]
    public uint abilityLimit;

    [Tooltip("Time between player death and spawn")]
    public float timeDeathSpawn;

    [Range(0, 200), Tooltip("In Percent (100% = normal Damage")]
    public float damageModifier = 100.0f;

    public int lifeLimit;

    [Tooltip("Time in Minutes")]
    public float timeLimit = 10;
    public float currentTime;

    // Player/Controller Selection
    [Header("Player Selection")]
    private GameObject[] playerSpawn;
    private int[] randomSpawnOrder = new int[4] { 0, 1, 2, 3 };
    [Tooltip("Spawnpoints must have this given tag")]
    public string spawnTag;

    [Header("Debug")]
    public DebugValues Tags;
    #endregion

    #region character variables
    public PlayerInfo[] playerInfo = new PlayerInfo[0];
    private string[] playerNames;

    [HideInInspector] public CharacterPicture[] charPics = new CharacterPicture[4];
    private GameObject[] playerOnStage = new GameObject[4] { null, null, null, null };
    #endregion

    void Awake()
    {
        // Dont destroy this object on loading a new level
        DontDestroyOnLoad(gameObject);
        // set singelton instance
        if (_instance == null) { _instance = this; }
        
        // set playerNames length
        playerNames = new string[playerInfo.Length];
        // set playerNames
        for (int i = 0; i < playerInfo.Length; i++) { playerNames[i] = playerInfo[i].name; }
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
        CustomDebug.EnableTag("MapFeature", Tags.MapFeature);
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

                // create player
                Util.IncludeGameObject(playerOnStage, Instantiate(GetPlayerPrefabByName(player.Character), playerSpawn[randomSpawnOrder[momSpawn]].transform.position, Quaternion.identity) as GameObject);
            }
        }
    }

    public GameObject[] PlayerOnStage { get { return playerOnStage; } }

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

    private PlayerInfo GetPlayerInfoByName(string _name)
    {
        foreach (PlayerInfo info in playerInfo)
        {
            if (info.name == _name)
            {
                return info;
            }
        }

        return new PlayerInfo();
    }

    #region Info Getter
    public string[] PlayerNames { get { return playerNames; } }
    public int GetPlayerInfoLength() { return playerInfo.Length; }
    #region info by name
    public GameObject GetPlayerPrefabByName(string _name)
    {
        // get info
        PlayerInfo info = GetPlayerInfoByName(_name);
        // return prafab
        if (info.prefab != null) { return info.prefab; }
        // if there is no prefab print an error and return null
        else { Debug.LogError("<b>" + _name + "</b> is not an accepted entry for <b>GetPlayerPrefab</b> or the prefab is not declarated"); }
        return null;
    }

    public Sprite GetPlayerStandardSplashartByName(string _name)
    {
        // get info
        PlayerInfo info = GetPlayerInfoByName(_name);
        // return prafab
        if (info.standardSplashart != null) { return info.standardSplashart; }
        // if there is no prefab print an error and return null
        else { Debug.LogError("<b>" + _name + "</b> is not an accepted entry for <b>GetPlayerPrefab</b> or the standard splashart is not declarated"); }
        return null;
    }

    public Sprite GetPlayerLockedSplashartByName(string _name)
    {
        // get info
        PlayerInfo info = GetPlayerInfoByName(_name);
        // return prafab
        if (info.lockedSplashart != null) { return info.lockedSplashart; }
        // if there is no prefab print an error and return null
        else { Debug.LogError("<b>" + _name + "</b> is not an accepted entry for <b>GetPlayerPrefab</b> or the locked splashart is not declarated"); }
        return null;
    }

    public Sprite GetPlayerBlockedSplashartByName(string _name)
    {
        // get info
        PlayerInfo info = GetPlayerInfoByName(_name);
        // return prafab
        if (info.blockedSplashart != null) { return info.blockedSplashart; }
        // if there is no prefab print an error and return null
        else { Debug.LogError("<b>" + _name + "</b> is not an accepted entry for <b>GetPlayerPrefab</b> or the blocked splashart is not declarated"); }
        return null;
    }

    public Sprite GetPlayerStandardIconByName(string _name)
    {
        // get info
        PlayerInfo info = GetPlayerInfoByName(_name);
        // return prafab
        if (info.standardIcon != null) { return info.standardIcon; }
        // if there is no prefab print an error and return null
        else { Debug.LogError("<b>" + _name + "</b> is not an accepted entry for <b>GetPlayerPrefab</b> or the standard icon is not declarated"); }
        return null;
    }

    public Sprite GetPlayerDeathIconByName(string _name)
    {
        // get info
        PlayerInfo info = GetPlayerInfoByName(_name);
        // return prafab
        if (info.deathIcon != null) { return info.deathIcon; }
        // if there is no prefab print an error and return null
        else { Debug.LogError("<b>" + _name + "</b> is not an accepted entry for <b>GetPlayerPrefab</b> or the standart icon is not declarated"); }
        return null;
    }

    public int GetPlayerIndexByName(string _name)
    {
        // get info
        PlayerInfo info = GetPlayerInfoByName(_name);
        // return index
        if (info.deathIcon != null) { return System.Array.IndexOf(playerInfo, info); }
        // if there is no prefab print an error and return null
        else { Debug.LogError("<b>" + _name + "</b> is not an accepted entry for <b>GetPlayerPrefab</b> or the standart icon is not declarated"); }
        return 0;
    }
    #endregion

    #region name by info
    public string GetPlayerNameByPrefab(GameObject _prefab)
    {
        foreach (PlayerInfo info in playerInfo)
        {
            if (info.prefab == _prefab)
            {
                return info.name;
            }
        }

        Debug.LogError("<b>" + _prefab.name + "</b> is not a declarated as a prefab");
        return null;
    }

    public string GetPlayerNameByStandardSplashart(Sprite _standardSplashart)
    {
        foreach (PlayerInfo info in playerInfo)
        {
            if (info.standardSplashart == _standardSplashart)
            {
                return info.name;
            }
        }

        Debug.LogError("<b>" + _standardSplashart.name + "</b> is not a declarated stadard splashart");
        return null;
    }

    public string GetPlayerNameByLockedSplashart(Sprite _lockedSplashart)
    {
        foreach (PlayerInfo info in playerInfo)
        {
            if (info.lockedSplashart == _lockedSplashart)
            {
                return info.name;
            }
        }

        Debug.LogError("<b>" + _lockedSplashart.name + "</b> is not a declarated as a locked splashart");
        return null;
    }

    public string GetPlayerNameByBlockedSplashart(Sprite _blockedSplashart)
    {
        foreach (PlayerInfo info in playerInfo)
        {
            if (info.blockedSplashart == _blockedSplashart)
            {
                return info.name;
            }
        }

        Debug.LogError("<b>" + _blockedSplashart.name + "</b> is not a declarated as a blocked splashart");
        return null;
    }

    public string GetPlayerNameByStandardIcon(Sprite _standaerdIcon)
    {
        foreach (PlayerInfo info in playerInfo)
        {
            if (info.standardIcon == _standaerdIcon)
            {
                return info.name;
            }
        }

        Debug.LogError("<b>" + _standaerdIcon.name + "</b> is not a declarated as a standard icon");
        return null;
    }

    public string GetPlayerNameByDeathIcon(Sprite _deathIcon)
    {
        foreach (PlayerInfo info in playerInfo)
        {
            if (info.standardIcon == _deathIcon)
            {
                return info.name;
            }
        }

        Debug.LogError("<b>" + _deathIcon.name + "</b> is not a declarated as a death icon");
        return null;
    }

    public string GetPlayerNameByIndex(int _deathIcon)
    {
        if (_deathIcon < playerInfo.Length)
        {
            return playerInfo[_deathIcon].name;
        }

        Debug.LogError("<b>" + _deathIcon + "</b> is aout of range of playerInfo");
        return null;
    }
    #endregion
    #endregion
}

[System.Serializable]
public struct PlayerInfo
{
    public string name;
    public GameObject prefab;

    [Header("Splasharts")]
    public Sprite standardSplashart;
    public Sprite lockedSplashart;
    public Sprite blockedSplashart;

    [Header("Icons")]
    public Sprite standardIcon;
    public Sprite deathIcon;
}
