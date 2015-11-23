using UnityEngine;
using System.Collections;
using System;

public class Capricorn : Attacks {

    public Capricorn(float damage, float castTime, float duration, float cooldown, float range) : base(
        10, "capricorn", "CC", true, 0, damage, castTime, duration, cooldown, range) { }

    public override void Use(GameObject _target) {
    }
}