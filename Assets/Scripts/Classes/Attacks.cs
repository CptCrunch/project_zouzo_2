using UnityEngine;
using System.Collections;

public abstract class Attacks {

    private uint id;
    private string name;
    private float heal;
    private float damage;
    private float castTime;
    private float cooldown;
    private uint durability;

    //Empty Constructor
    public Attacks() { }
    
    //Constructor with Heal
    public Attacks(uint id, string name, float heal, float damage, float castTime, float cooldown)
    {
        this.id = id;
        this.name = name;
        this.heal = heal;
        this.damage = damage;
        this.castTime = castTime;
        this.cooldown = cooldown;
        if(Gamerules._instance.abilityLimit != 0) { durability = Gamerules._instance.abilityLimit; }
    }

    #region Get & Set 
    public uint ID { get { return id; } set { this.id = value; } }
    public string Name { get { return name; } set { this.name = value; } }
    public float Heal { get { return heal; } set { this.heal = value; } }
    public float Damage { get { return damage; } set { this.damage = value; } }
    public float CastTime { get { return castTime; } set { this.castTime = value; } }
    public float Cooldown { get { return cooldown; } set { this.cooldown = value; } }
    public uint Durability { get { return durability; } }
    #endregion

    public abstract void Use();
}
