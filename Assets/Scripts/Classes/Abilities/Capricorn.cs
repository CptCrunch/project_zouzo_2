using UnityEngine;
using System.Collections;
using System;

public class Capricorn : Attacks {

    private int time;
    private float height;

    public Capricorn(float damage, float castTime, float delay, float duration, float cooldown, float range, float height, int time) : base(
        10, "capricorn", "CC", true, true, 0, damage, castTime, delay, duration, cooldown, range)
    {
        this.time = time;
        this.height = height;
    }

    public override void Use(GameObject _target) {
        
        // get the vitals of the target
        LivingEntity Vitals = _target.GetComponent<Player>().playerVitals;

        // kock the target up and deal damage
        try {
            Vitals.ApplyPlayerKnockUp(height, time);
        }
        catch (OverflowException)
        {
            Debug.LogError("Is Outside range of Int32 tyoe.");
        }

        Vitals.GetDamage(Damage);

        // set the spell onto cooldown
        OnCooldown = true;
    }
}
