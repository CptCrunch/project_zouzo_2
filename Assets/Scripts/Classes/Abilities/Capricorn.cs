using UnityEngine;
using System.Collections;
using System;

public class Capricorn : Attacks {

    public Capricorn(float damage, float castTime, float duration, float cooldown, GameObject capricornPrefab) : base(
        10, "capricorn", 0, damage, castTime, duration, cooldown, 2f) { }

    public override void Use() {

    }
}