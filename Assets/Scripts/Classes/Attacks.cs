using UnityEngine;
using System.Collections;

public abstract class Attacks {

    private GameObject caster;
    private LivingEntity playerVitals;
    private PlayerAbilities playerAbilitiesScript;

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
    private uint spellDir;
    private bool isStarted = false;
    private bool isCast = false;
    private float timeBetweenCastes = 0;
    private float bulletSpeed;

    private GameObject[] hitTargets = new GameObject[4];
    private Vector3 spellDirection = new Vector3(0, 90, 0);

    //Empty Constructor
    public Attacks() { }
    
    //Constructor with Heal
    public Attacks(GameObject caster, uint id, string name, string type, int maxTargets, float damage, float castTime, float delay, float duration, float cooldown, float range, uint spellDir)
    {
        this.caster = caster;
        this.playerVitals = caster.GetComponent<Player>().playerVitals;
        this.playerAbilitiesScript = caster.GetComponent<PlayerAbilities>();
        this.id = id;
        this.name = name;
        this.type = type;
        this.maxTargets = maxTargets;
        this.damage = damage;
        this.castTime = castTime;
        this.toTravelTime = delay;
        this.maxCooldown = cooldown;
        this.range = range;
        this.spellDir = spellDir;
        if (Gamerules._instance.abilityLimit != 0) { durability = Gamerules._instance.abilityLimit; }
        if (range > 0) { bulletSpeed = toTravelTime / range; }
        else { bulletSpeed = toTravelTime; }
    }

    #region Get & Set 
    public GameObject Caster { get { return caster; } }
    public LivingEntity PlayerVitals { get { return playerVitals; } }
    public PlayerAbilities PlayerAbilitiesScript { get { return playerAbilitiesScript; } }
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
    public float Range { get { return range; } set { range = value; } }
    public int PlayersHit { get { return playersHit; } set { playersHit = value; } }
    public uint SpellDir { get { return spellDir; } }
    public bool IsCast { get { return isCast; } set { isCast = value; } }
    public bool IsStarted { get { return isStarted; } set { isStarted = value; } }
    public float TimeBeteewnCasts { get { return timeBetweenCastes; } set { timeBetweenCastes = value; } }
    public float BulletSpeed { get { return bulletSpeed; } set { bulletSpeed = value; } }
    public GameObject[] HitTargets { get { return hitTargets; } set { hitTargets = value; } }
    public Vector2 SpellDirection { get { return spellDirection; } set { spellDirection = value; } }
    #endregion

    public abstract void StartSpell();

    /// <summary> Casts spell and checks if spell can be casted </summary>
    public abstract void Cast();

    /// <summary> Used at button release </summary>
    public abstract void AfterCast();

    /// <summary> uses spell (deals damage, applys conditions) </summary>
    public abstract void Use(GameObject _target);

    /// <summary> checks if spell hits his maximum targets </summary>
    public bool MaxTargetsReached()
    {
        if (maxTargets == 0) { return false; }
        if (playersHit > maxTargets) { return true; }
        return false;
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
        if (currCooldown <= 0) { currCooldown = 0; IsDisabled = false; /*Debug.Log(IsDisabled);*/ }
    }

    public virtual void SetCooldown()
    {
        currCooldown = maxCooldown;
        isDisabled = true;
    }
}
