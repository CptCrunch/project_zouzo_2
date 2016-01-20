﻿using UnityEngine;
using System.Collections;

public class AbilityManager : MonoBehaviour {

    public static AbilityManager _instance;

    public Abilities[] abilities = new Abilities[0];
    public Abilities[] spells = new Abilities[13];

    void Awake()
    {
        if (_instance == null) { _instance = this; }

        for (int i = 0; i < abilities.Length; i++)
        {
            switch (abilities[i].name)
            {
                case "Basic": spells[0] = abilities[i]; break;
                case "Aries": spells[1] = abilities[i]; break;
                case "Taurus": spells[2] = abilities[i]; break;
                case "Gemini": spells[3] = abilities[i]; break;
                case "Cancer": spells[4] = abilities[i]; break;
                case "Leo": spells[5] = abilities[i]; break;
                case "Virgo": spells[6] = abilities[i]; break;
                case "Libra": spells[7] = abilities[i]; break;
                case "Scorpio": spells[8] = abilities[i]; break;
                case "Saggitarius": spells[9] = abilities[i]; break;
                case "Capricorn": spells[10] = abilities[i]; break;
                case "Aquarius": spells[11] = abilities[i]; break;
                case "Pisces": spells[12] = abilities[i]; break;
            }
        }
    }

    public Attacks CreateBasic()
    {
        return new Basic(spells[0].damage, spells[0].castTime, spells[0].travelTime, spells[0].duration, spells[0].cooldown, spells[0].range, spells[0].targets, spells[0].directions);
    }

    public Attacks CreateLeo()
    {
        return new Leo(spells[5].damage, spells[5].castTime, spells[5].travelTime, spells[5].duration, spells[5].cooldown, spells[5].range, spells[5].targets, spells[5].directions, spells[5].charges, spells[5].chargeCooldown);
    }

    public Attacks CreateSaggitarius()
    {
        return new Saggitarius(spells[9].damage, spells[9].castTime, spells[9].bulletSpeed, spells[9].duration, spells[9].cooldown, spells[9].range, spells[9].targets, spells[9].directions, spells[9].bullet);
    }

    public Attacks CreateCapricorn()
    {
        return new Capricorn(spells[10].damage, spells[10].castTime, spells[10].travelTime, spells[10].duration, spells[10].cooldown, spells[10].range, spells[10].targets, spells[10].directions, spells[10].knockUpHeght, spells[10].knockBackRange, spells[10].knockBackStrenght);
    }
}

[System.Serializable]
public struct Abilities
{
    public string name;
    public float damage;
    [Tooltip("the time it takes the spell to start [time in seconds]")]
    public float castTime;
    public float duration;
    [Tooltip("[time in seconds]")]
    public float cooldown;
    [Tooltip("as skillshot 0 means infinity")]
    public float range;
    [Tooltip("amount of players that the spell can hit (0 is infinity)")]
    public int targets;
    [Tooltip("the amount of directions the spell can be aimed (2, 4, 8)")]
    [Range(2,8)]
    public uint directions;
    [Header("Meeles")]
    [Tooltip("the time that the spell needs to reach max range (0 is instand) [time in seconds]")]
    public float travelTime;

    [Header("Skillshots")]
    public GameObject bullet;
    [Tooltip("the time that the spell needs to reach max range, if the range is 0 it's the distance the bullet will travel in 1 sec")]
    public float bulletSpeed;

    [Header("Class specific variables")]
    [Tooltip("nessesary for: capricorn(1)")]
    public float knockUpHeght;
    [Tooltip("nessesary for: capricorn(2)")]
    public float knockBackRange;
    [Tooltip("nessesary for: capricorn(2)\nthe distance the enemy gets kocked back")]
    public float knockBackStrenght;
    [Tooltip("nessesary for: leo")]
    public int charges;
    [Tooltip("nessesary for: leo")]
    public float chargeCooldown;
}
