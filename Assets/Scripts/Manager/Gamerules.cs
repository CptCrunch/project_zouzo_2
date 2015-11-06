﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    //Player/Controller Selection
    [Header("Player Selection")]
    public GameObject[] playerPrefab = new GameObject[4];
    private GameObject[] playerSpawn = new GameObject[4];
    [Tooltip("Spawnpoints must have this given tag")]
    public string spawnTag;

    //Testing 
    [HideInInspector]
    public List<string> connectedControllers = new List<string>();
    public TestPlayer[] testPlayers = new TestPlayer[5];
    #endregion

    void Awake()
    {
        //Dont destroy the object on load of a new level
        DontDestroyOnLoad(gameObject);

        if (_instance == null) { _instance = this; }
    }
    
    //Create All Player when the level loads
    void OnLevelWasLoaded()
    {
        //Get all objecticts with the given tag
        playerSpawn = GameObject.FindGameObjectsWithTag(spawnTag);

        if (playerAmmount > 0)
        {
            for (int i = 0; i < playerAmmount; i++)
            {
                GameObject go = Instantiate(playerPrefab[i], playerSpawn[Random.Range(0, playerSpawn.Length)].transform.position, Quaternion.identity) as GameObject;

                if (PlayerSelection._instance.controller[i] == "KB") 
                {
                    ChangeAxis("KB", go);
                } 
                if(PlayerSelection._instance.controller[i] == "P1")
                {
                    ChangeAxis("P1", go);
                }

                if(PlayerSelection._instance.controller[i] == "P2")
                {
                    ChangeAxis("P2", go);
                }

                if(PlayerSelection._instance.controller[i] == "P3")
                {
                    ChangeAxis("P3", go);
                }

                if(PlayerSelection._instance.controller[i] == "P4")
                {
                    ChangeAxis("P4", go);
                }
            }
        }
    }

    private void ChangeAxis(string axis, GameObject obj)
    {
        obj.GetComponent<Player>().playerAxis = axis;
        obj.name = axis + "_Player";

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
