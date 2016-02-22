﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public static GameManager _instance;

    [HideInInspector] public bool Running;

    [Tooltip("all scenes the GameManager script will detect as a stage")]
    public string[] registerdStages = new string[1];

    #region GameManager Variable
    public int playerAmmount = 0;
    public int abilityLimit;
    public float damageModifier = 100.0f;
    public int lifeLimit;
    public float timeLimit = 10;
    private float currentTime;

    // Player/Controller Selection
    private GameObject[] playerSpawn;
    private int[] randomSpawnOrder = new int[4] { 0, 1, 2, 3 };
    public string spawnTag;
    #endregion

    #region character variables
    public float playerMaxHealth = 0;
    public float timeDeathSpawn;
    public PlayerInfo[] playerInfo = new PlayerInfo[0];
    private string[] playerNames;

    [HideInInspector] public CharacterPicture[] charPics = new CharacterPicture[4];
    private GameObject[] playerOnStage = new GameObject[4] { null, null, null, null };
    #endregion

    [Header("Debug")]
    public DebugValues Tags;

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

    public GameObject[] PlayerOnStage { get { return playerOnStage; } }

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

        // check if scene is a stage and start OnStage metod if so
        if (IsStage()) { OnStage(); }
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
        // check if scene is a stage and start OnStage metod if so
        if (IsStage()) { OnStage(); }

        StartCoroutine(WaitCoroutine());
    }

    public void OnStage()
    {
        // find all spawns
        playerSpawn = GameObject.FindGameObjectsWithTag(spawnTag);

        // --- [ set random spawn point order ] ---
        Util.MixArray(randomSpawnOrder);

        /*/ --- [ register player on stage ] ---
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            Util.IncludeGameObject(playerOnStage, player);
        }*/

        /*/ --- [ spawn player ] ---
        int momSpawn = 0;
        foreach (CharacterPicture player in charPics)
        {
            Debug.Log(player.Character);
            // check if object isn't null
            if (player != null)
            {
                momSpawn++;

                // create player
                Util.IncludeGameObject(playerOnStage, Instantiate(GetPlayerPrefabByName(player.Character), playerSpawn[randomSpawnOrder[momSpawn]].transform.position, Quaternion.identity) as GameObject);
            }
        }*/
    }

    private IEnumerator WaitCoroutine()
    {
        foreach(GameObject go in playerOnStage)
        {
            if (go != null)
            {
                go.GetComponent<Player>().gamerulesDisabled = true;
                go.GetComponent<Player>().flipEnable = false;
                go.GetComponent<Player>()._animator.SetTrigger("Spawn");
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

    //TODO: Implement To spawn new player
    public void PlayerDeath(LivingEntity playerVitals)
    {
        
    }

    public void EndGame()
    {
        Running = false;
    }

    public bool IsStage()
    {
        // --- [ stage chack ] ---
        // check if scene is a registered stage
        foreach (string stage in registerdStages) { if (stage == Application.loadedLevelName) { return true; } }
        // continue if scene is registered
        return false;
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
        else { Debug.LogError("<b>" + _name + "</b> is not an accepted entry for <b>GetPlayerPrefabByName</b> or the prefab is not declarated"); }
        return null;
    }

    public Sprite GetPlayerStandardSplashartByName(string _name)
    {
        // get info
        PlayerInfo info = GetPlayerInfoByName(_name);
        // return prafab
        if (info.standardSplashart != null) { return info.standardSplashart; }
        // if there is no prefab print an error and return null
        else { Debug.LogError("<b>" + _name + "</b> is not an accepted entry for <b>GetPlayerInfoByName</b> or the standard splashart is not declarated"); }
        return null;
    }

    public Sprite GetPlayerLockedSplashartByName(string _name)
    {
        // get info
        PlayerInfo info = GetPlayerInfoByName(_name);
        // return prafab
        if (info.lockedSplashart != null) { return info.lockedSplashart; }
        // if there is no prefab print an error and return null
        else { Debug.LogError("<b>" + _name + "</b> is not an accepted entry for <b>GetPlayerLockedSplashartByName</b> or the locked splashart is not declarated"); }
        return null;
    }

    public Sprite GetPlayerBlockedSplashartByName(string _name)
    {
        // get info
        PlayerInfo info = GetPlayerInfoByName(_name);
        // return prafab
        if (info.blockedSplashart != null) { return info.blockedSplashart; }
        // if there is no prefab print an error and return null
        else { Debug.LogError("<b>" + _name + "</b> is not an accepted entry for <b>GetPlayerBlockedSplashartByName</b> or the blocked splashart is not declarated"); }
        return null;
    }

    public Sprite GetPlayerStandardIconByName(string _name)
    {
        // get info
        PlayerInfo info = GetPlayerInfoByName(_name);
        // return prafab
        if (info.standardIcon != null) { return info.standardIcon; }
        // if there is no prefab print an error and return null
        else { Debug.LogError("<b>" + _name + "</b> is not an accepted entry for <b>GetPlayerStandardIconByName</b> or the standard icon is not declarated"); }
        return null;
    }

    public Sprite GetPlayerDeathIconByName(string _name)
    {
        // get info
        PlayerInfo info = GetPlayerInfoByName(_name);
        // return prafab
        if (info.deathIcon != null) { return info.deathIcon; }
        // if there is no prefab print an error and return null
        else { Debug.LogError("<b>" + _name + "</b> is not an accepted entry for <b>GetPlayerDeathIconByName</b> or the standart icon is not declarated"); }
        return null;
    }

    public int GetPlayerIndexByName(string _name)
    {
        // return index
        for (int i = 0; i < playerInfo.Length; i++)
        {
            if (playerInfo[i].name == _name) { return i; }
        }
        // if there is no prefab print an error and return null
        Debug.LogError("<b>" + _name + "</b> is not an accepted entry for <b>GetPlayerIndexByName</b> or the standart icon is not declarated");
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