using UnityEngine;
using System.Collections;

public class Capricorn : Attacks
{
    public Capricorn(string name, float damage, float castTime, float cooldown): base(10, name, 0 ,damage, castTime, cooldown) { }

    public override void Use() {}
}
