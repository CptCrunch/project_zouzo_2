using UnityEngine;
using System.Collections;
using System;

public class Capricorn : Attacks {

    public Capricorn(float damage, float castTime, float duration, float cooldown, float range) : base(
        10, "capricorn", "CC", true, 0, damage, castTime, duration, cooldown, range) { }

    public override void Use(GameObject _target) {
        Debug.Log("im in");
        // get the vitals of the target
        LivingEntity Vitals = _target.AddComponent<Player>().playerVitals;

        // kock the target up and deal damage
        Vitals.PlayerKnockUp(2f, 0.2f);
        Vitals.GetDamage(Damage);

        // set the spell onto cooldown
        OnCooldown = true;
    }
}