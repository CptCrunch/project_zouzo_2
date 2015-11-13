using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class LivingEntity
{
    private Player instance;

    private string name;
    private float moveSpeed;
    private float slowedSpeed;
    private float currSpeed;
    private float maxHealth;
    private float currHealth;
    private float basicAttackDamage;

    private bool stunned;
    private int stunIndex;
    private bool slowed;
    private bool slowedOverTime;
    private int slowIndex;
    private bool knockUped;
    private int knockUpIndex;
    private bool knockBacked;
    private int knockBackIndex;

    public LivingEntity(GameObject playerObject, string name, float moveSpeed, float slowedSpeed, float maxHealth, float basicAttackDamage)
    {
        // set maxHealth ( will use maxHealth from Gamerulses )
        if (Gamerules._instance.playerMaxHealth == 0) { this.maxHealth = maxHealth; } 
        else { this.maxHealth = Gamerules._instance.playerMaxHealth; }

        instance = playerObject.GetComponent<Player>();

        this.name = name;
        this.moveSpeed = moveSpeed;
        currSpeed = moveSpeed;
        this.slowedSpeed = slowedSpeed;
        this.currHealth = maxHealth;
        this.basicAttackDamage = basicAttackDamage;
    }

    #region Get & Set
    public float MoveSpeed { get { return currSpeed; } }
    public string Name { get { return name; } }
    public float CurrHealth { get { return currHealth; } }
    public float BasicAttackDamage { get { return basicAttackDamage; } }

    public bool Stunned { get { return stunned; } }
    public bool Slowed {
        get {
            if (!slowed && !slowedOverTime) { return false; }
            else { return true;  }
        }
    }
    public bool KnockUped { get { return knockUped; } }
    public bool KnockBacked { get { return knockBacked; } }
    #endregion

    #region Functions

    // heal Player
    public void Heal(float _ammount) {
        float newHealth = currHealth + _ammount;

        if(newHealth > maxHealth) { currHealth = maxHealth; }
        else { currHealth = newHealth; }
    }

    // damage Player
    public void GetDamage(float _ammount) {
        currHealth -= _ammount;

        if(_ammount >= maxHealth || currHealth <= 0) { currHealth = 0; }
    }

    // stun Player
    public IEnumerator Stun(float _time) {
        stunIndex++;
        int currIndex = stunIndex;

        stunned = true;
        /*Debug.Log("Stun start");*/
        instance.velocity.x = 0;

        yield return new WaitForSeconds(_time);
        if (currIndex == stunIndex) { stunned = false; /*Debug.Log("Stun stop");*/ }
    }

    // slow Plyer
    public void Slow(bool _toggle) {
        if (_toggle) { currSpeed = slowedSpeed; slowed = true; /*Debug.Log("Slow start");*/ }
        else {
            slowed = false;
            /*Debug.Log("Slow stop");*/
            if (!Slowed) { currSpeed = moveSpeed; }
        }
             
            
    }

    // slow Plyer over Time
    public IEnumerator SlowOverTime(float _time) {
        slowIndex++;
        int currIndex = slowIndex;
        
        slowedOverTime = true;
        /*Debug.Log("SlowOverTime start");*/
        currSpeed = slowedSpeed;

        yield return new WaitForSeconds(_time);
        if (currIndex == slowIndex) {
            slowedOverTime = false;
            /*Debug.Log("SlowOverTime stop");*/
            if (!Slowed) { currSpeed = moveSpeed; }
        }
    }

    // knock up Player
    public IEnumerator PlayerKnockUp(float _yHeight, float _time) {
        knockUpIndex++;
        int currIndex = knockUpIndex;

        knockUped = true;
        /*Debug.Log("KnockUp start");*/
        instance.velocity.x = 0;
        instance.velocity.y += _yHeight / _time;

        yield return new WaitForSeconds(_time);
        if (currIndex == knockUpIndex) { knockUped = false; /*Debug.Log("KnockUp stop");*/ }
    }

    // knock back Player
    public IEnumerator PlayerKnockBack(float _xDistance, float _yDistance, float _time) {
        knockBackIndex++;
        int currIndex = knockBackIndex;

        knockBacked = true;
        /*Debug.Log("KockBack start");*/
        instance.velocity.x = _xDistance / _time;
        instance.velocity.y += _yDistance / _time;

        yield return new WaitForSeconds(_time);
        if (currIndex == knockBackIndex) { knockBacked = false; /*Debug.Log("KockBack stop");*/ }
    }
    #endregion
}