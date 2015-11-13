using UnityEngine;
using System.Collections;

public class Capricorn : Attacks
{
    public Capricorn(string name, float damage, float castTime, float duration, float cooldown, GameObject capricornPrefab) : base(10, name, 0, damage, castTime, duration, cooldown, capricornPrefab) { }

    public override void Use() {}
}
