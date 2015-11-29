using UnityEngine;
using System.Collections;

public abstract class Attacks {

    private uint id;
    private string name;
    private string type;
    private float heal;
    private float damage;
    private float castTime;
    private float duration;
    private float cooldown;
    private bool onCooldown = false;
    private uint durability;
    private float range;
    private bool isMeele;

    //Empty Constructor
    public Attacks() { }
    
    //Constructor with Heal
    public Attacks(uint id, string name, string type, bool isMeele, float heal, float damage, float castTime, float duration, float cooldown, float range)
    {
        this.id = id;
        this.name = name;
        this.type = type;
        this.isMeele = isMeele;
        this.heal = heal;
        this.damage = damage;
        this.castTime = castTime;
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
    public float Heal { get { return heal; } set { this.heal = value; } }
    public float Damage { get { return damage; } set { this.damage = value; } }
    public float CastTime { get { return castTime; } set { this.castTime = value; } }
    public float Cooldown { get { return cooldown; } set { this.cooldown = value; } }
    public bool OnCooldown { get { return onCooldown; } set { onCooldown = value; } }
    public uint Durability { get { return durability; } }
    public float Range { get { return range; } }
    #endregion

    public abstract void Use(GameObject _target);
}
