using UnityEngine;
using System.Collections;
using System;

public class Capricorn : Attacks {

    private int time;
    private float height;
    private float maxKnockBackDistance;
    private bool usedInAir = false;

    public Capricorn(float damage, float castTime, float travleTime, float duration, float cooldown, float range, int targets, float height, int time) : base(
        10, "capricorn", "CC", targets, damage, castTime, travleTime, duration, cooldown, range)
    {
        this.time = time;
        this.height = height;
    }

    public float MaxKnockBackDistance { get { return maxKnockBackDistance; } }
    public bool UsedInAir { get { return usedInAir; } }

    public override void Cast(GameObject _caster)
    {
        // get playerScript from caster
        Player playerScript = _caster.GetComponent<Player>();

        if (_caster.GetComponent<Controller2D>().collisions.below)
        {
            if (!IsDisabled)
            {
                playerScript.castedSpell = this;
            }
        }

        else
        {
            if (playerScript.CanCapricorn2BeUsed() != null)
            {
                UseCapricorn2(_caster, playerScript.CanCapricorn2BeUsed());
            }
        }
    }

    public override void AfterCast()
    {
        IsAbilityCasted = false;
    }

    public override void Use(GameObject _target, GameObject _caster) {
        
        // get the vitals of the target
        LivingEntity Vitals = _target.GetComponent<Player>().playerVitals;

        // knock the target up and deal damage
        try
        {
            Vitals.ApplyPlayerKnockUp(height, time);
        }
        catch (OverflowException)
        {
            Debug.LogError("Is Outside range of Int32 tyoe.");
        }

        Vitals.GetDamage(Damage);
    }

    public void UseCapricorn2(GameObject _caster, GameObject _target)
    {
        // get the vitals of the target
        LivingEntity Vitals = _target.GetComponent<Player>().playerVitals;

        float dirX = _caster.transform.position.x - _target.transform.position.x;
        float dirY = _target.transform.position.y - _caster.transform.position.y;

        float distaceMultiplicator = 1 / (dirX + dirY);

        _target.GetComponent<Player>().playerVitals.ApplyPlayerKnockBack(dirX * distaceMultiplicator, dirY * distaceMultiplicator, 20);

        Vitals.GetDamage(Damage);

        usedInAir = true;
    }
}
