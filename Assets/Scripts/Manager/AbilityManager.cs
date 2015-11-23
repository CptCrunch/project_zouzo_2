using UnityEngine;
using System.Collections;

public class AbilityManager : MonoBehaviour {

    public static AbilityManager _instance;
    public GameObject[] abilityCollider = new GameObject[13];

    public Abilities[] abilities = new Abilities[0];
    void Awake()
    {
        if (_instance == null) { _instance = this; }
    }

    public Attacks UseCapricorn() {
        return new Capricorn(abilities[9].damage, abilities[9].castTime, abilities[9].duration, abilities[9].cooldown, abilities[9].prefab);
    }
}

[System.Serializable]
public class Abilities {
    public string name;
    public GameObject prefab;
    public float damage;
    public float castTime;
    public float duration;
    public float cooldown;
}
