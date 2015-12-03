using UnityEngine;
using System.Collections;

public abstract class Attacks {

    private uint id;
    private string name;
    private string type;
    private float heal;
    private float damage;
    private float castTime;
    private float delay;
    private float duration;
    private float currDuration = 0;
    private float cooldown;
    private bool onCooldown = false;
    private uint durability;
    private float range;
    private bool isMeele;
    private bool isAOE;
    private int playersHit;

    //Empty Constructor
    public Attacks() { }
    
    //Constructor with Heal
    public Attacks(uint id, string name, string type, bool isMeele, bool isAOE, float heal, float damage, float castTime, float delay, float duration, float cooldown, float range)
    {
        this.id = id;
        this.name = name;
        this.type = type;
        this.isMeele = isMeele;
        this.isAOE = isAOE;
        this.heal = heal;
        this.damage = damage;
        this.castTime = castTime;
        this.delay = delay;
        this.castTime = castTime;
        this.cooldown = cooldown;
        this.range = range;
        if(Gamerules._instance.abilityLimit != 0) { durability = Gamerules._instance.abilityLimit; }

    }

    #region Get & Set 
    public uint ID { get { return id; } }
    public string Name { get { return name; } }
    public string Type { get { return type; } }
    public bool IsMeele { get { return isMeele; } }
    public bool IsAOE { get { return isAOE; } }
    public float Heal { get { return heal; } set { this.heal = value; } }
    public float Damage { get { return damage; } set { this.damage = value; } }
    public float CastTime { get { return castTime; } set { this.castTime = value; } }
    public float Delay { get { return delay;  } }
    public float Cooldown { get { return cooldown; } set { this.cooldown = value; } }
    public bool OnCooldown { get { return onCooldown; } set { onCooldown = value; } }
    public uint Durability { get { return durability; } }
    public float Range { get { return range; } }
    public int PlayersHit { get { return playersHit; } set { playersHit = value; } }
    public float CurrDuration { get { return currDuration; } set { currDuration = value; } }
    #endregion

    public abstract void Use(GameObject _target);

    public bool IsCastable()
    {
        playersHit++;
        if (!isAOE)
        {
            if (playersHit > 1)
            {
                return false;
            }
        }
        return true;
    }

    public void ResetPlayersHit() { playersHit = 0; }

    public bool AbilityTime()
    {
        if (currDuration > delay)
        {
            currDuration = 0;
            return false;
        }

        currDuration += Time.deltaTime;

        return true;
    }
}
