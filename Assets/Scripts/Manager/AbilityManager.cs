using UnityEngine;
using System.Collections;

public class AbilityManager : MonoBehaviour {

    public static AbilityManager _instance;

    public Abilities[] abilities = new Abilities[0];

    void Awake()
    {
        if (_instance == null) { _instance = this; }
    }

    public Attacks CreateLeo()
    {
        return new Leo(abilities[4].damage, abilities[4].castTime, abilities[4].travelTime, abilities[4].duration, abilities[4].cooldown, abilities[4].range, abilities[4].targets, abilities[4].charges);
    }

    public Attacks CreateCapricorn()
    {
        return new Capricorn(abilities[9].damage, abilities[9].castTime, abilities[9].travelTime, abilities[9].duration, abilities[9].cooldown, abilities[9].range, abilities[9].targets, abilities[9].knockUpHeght, abilities[9].time);
    }

    public Attacks CreateBasic()
    {
        return new Basic(abilities[12].damage, abilities[12].castTime, abilities[12].travelTime, abilities[12].duration, abilities[12].cooldown, abilities[12].range, abilities[12].targets);
    }
}

[System.Serializable]
public struct Abilities
{
    public string name;
    public float damage;
    [Tooltip("[time in seconds]")]
    public float castTime;
    [Tooltip("the time that the spell needs to reach max range (0 is instand) [time in seconds]")]
    public float travelTime;
    public float duration;
    [Tooltip("[time in seconds]")]
    public float cooldown;
    public float range;
    [Tooltip("amount of players that the spell can hit (0 is infinity)")]
    public int targets;

    [Header("Class specific variables")]
    [Tooltip("nessesary for: capricorn(1)")]
    public float knockUpHeght;
    [Tooltip("nessesary for: capricorn(2)")]
    public float knockBackDistance;
    [Tooltip("nessesary for: capricorn(2)")]
    public float maxKockBackDistance;
    [Tooltip("nessesary for: capricorn(1) \n[time in milliseconds (1:1000)]")]
    public int time;
    [Tooltip("nessesary for: leo")]
    public int charges;
}
