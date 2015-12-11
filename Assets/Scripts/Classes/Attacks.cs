using UnityEngine;
using System.Collections;

public abstract class Attacks {

    private uint id;
    private string name;
    private string type;
    private float damage;
    private float castTime;
    private float toTravelTime;
    private float traveltTime;
    private bool shallTravel = true;
    private float duration;
    private float maxCooldown;
    private float currCooldown = 0;
    private bool isDisabled = false;
    private uint durability;
    private float range;
    private int maxTargets;
    private int playersHit;
    private bool isAbilityCasted = false;

    //Empty Constructor
    public Attacks() { }
    
    //Constructor with Heal
    public Attacks(uint id, string name, string type, int maxTargets, float damage, float castTime, float delay, float duration, float cooldown, float range)
    {
        this.id = id;
        this.name = name;
        this.type = type;
        this.maxTargets = maxTargets;
        this.damage = damage;
        this.castTime = castTime;
        this.toTravelTime = delay;
        this.castTime = castTime;
        this.maxCooldown = cooldown;
        this.range = range;
        if(Gamerules._instance.abilityLimit != 0) { durability = Gamerules._instance.abilityLimit; }
    }

    #region Get & Set 
    public uint ID { get { return id; } }
    public string Name { get { return name; } }
    public string Type { get { return type; } }
    public float Damage { get { return damage; } set { this.damage = value; } }
    public float CastTime { get { return castTime; } }
    public float ToTravelTime { get { return toTravelTime;  } }
    public bool ShallTravel { get { return shallTravel; } set { shallTravel = value; } }
    public float MaxCooldown { get { return maxCooldown; } }
    public float CurrCooldown { get { return currCooldown; } set { this.currCooldown = value; } }
    public bool IsDisabled { get { return isDisabled; } set { isDisabled = value; } }
    public uint Durability { get { return durability; } }
    public float Range { get { return range; } }
    public int PlayersHit { get { return playersHit; } set { playersHit = value; } }
    public bool IsAbilityCasted { get { return isAbilityCasted; } set { value = isAbilityCasted; } }
    #endregion

    /// <summary> Casts spell and checks if spell can be casted </summary>
    public abstract void Cast(GameObject _caster);

    /// <summary> Used at button releace </summary>
    public abstract void AfterCast();

    /// <summary> uses spell (deals damage, aplys conditions) </summary>
    public abstract void Use(GameObject _target, GameObject _caster);

    /// <summary> checks if spell hits his maximum targets </summary>
    public bool MaxTargetsReached()
    {
        playersHit++;
        if (maxTargets == 0) { return true; }
        if (playersHit > maxTargets) { return false; }
        return true;
    }

    public void ResetPlayersHit() { playersHit = 0; }

    public float TravelDistance()
    {
        // if 'toTravelTime' is 0 it's instand and shall automaticly return max range
        if (toTravelTime == 0) { traveltTime = toTravelTime; }

        traveltTime += Time.deltaTime;

        if (traveltTime >= toTravelTime)
        {
            traveltTime = 0;
            shallTravel = false;
            return range;
        }

        return range / toTravelTime * traveltTime;
    }

    public virtual void UpdateCooldowns()
    {
        currCooldown -= Time.deltaTime;
        if (currCooldown <= 0) { currCooldown = 0; IsDisabled = false; }
    }

    public virtual void SetCooldowne()
    {
        currCooldown = maxCooldown;
        isDisabled = true;
    }
}
