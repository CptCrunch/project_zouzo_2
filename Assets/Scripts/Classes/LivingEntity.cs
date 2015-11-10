using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class LivingEntity
{
    public float maxHealth;
    private float currHealth;
    private string name;
    private float basicAttackDamage;

    public LivingEntity(float maxHealth, string name, float basicAttackDamage) {
        
        // set maxHealth ( will use maxHealth from Gamerulses )
        if (Gamerules._instance.playerMaxHealth == 0) { this.maxHealth = maxHealth; } 
        else { this.maxHealth = Gamerules._instance.playerMaxHealth; }

        this.currHealth = maxHealth;
        this.name = name;
        this.basicAttackDamage = basicAttackDamage;
    }

    #region Get & Set
    public string Name { get { return name; } }
    public float CurrHealth { get { return currHealth; } }
    public float BasicAttackDamage { get { return basicAttackDamage; } }
    #endregion

    #region Functions

    // heal Player
    public void Heal(float ammount) {
        float newHealth = currHealth + ammount;

        if(newHealth > maxHealth) { currHealth = maxHealth; }
        else { currHealth = newHealth; }
    }

    // damage Player
    public void GetDamage(float ammount) {
        currHealth -= ammount;

        if(ammount >= maxHealth || currHealth <= 0) { currHealth = 0; }
    }
    #endregion
}