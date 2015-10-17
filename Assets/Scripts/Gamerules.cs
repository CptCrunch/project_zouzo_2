using UnityEngine;
using System.Collections;

public class Gamerules : MonoBehaviour {

    public static Gamerules _instance;

    #region Gamerules Variable
    [Header("Game Rules")]
    [Range (1,4)]
    public int playerAmmount = 1;
    [Tooltip("Defines the maximal use of an ability")]
    public uint abilityLimit;
    public float itemSpawnrate;
    [Tooltip("Time between player death and spawn")]
    public float timeDeathSpawn;
    [Range(0,100)]
    public float damageModifier;
    public Gamemode[] gameModeList = new Gamemode[2];
    #endregion

    void Awake()
    {
        if (_instance == null) { _instance = this; }
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
