using UnityEngine;
using System.Collections;

public class AbilityManager : MonoBehaviour {

    public static AbilityManager _instance;

    public Abilities[] abilities = new Abilities[0];
    void Awake()
    {
        if (_instance == null) { _instance = this; }
    }

    public Attacks UseCapricorn() {
        return new Capricorn(abilities[9].damage, abilities[9].castTime, abilities[9].duration, abilities[9].cooldown, abilities[9].range);
    }

    public Attacks Basic()
    {
        return new Basic(abilities[12].damage, abilities[12].castTime, abilities[12].duration, abilities[12].cooldown, abilities[12].range);
    }
}

[System.Serializable]
public class Abilities {
    public string name;
    public float damage;
    public float castTime;
    public float duration;
    public float cooldown;
    public float range;
}
