using UnityEngine;
using System.Collections;

public class Capricorn : Attacks
{
    public Capricorn(float damage, float castTime, float duration, float cooldown, GameObject capricornPrefab) : base(10, "capricorn", 0, damage, castTime, duration, cooldown, capricornPrefab) { }

    public override void Use() {}
}
