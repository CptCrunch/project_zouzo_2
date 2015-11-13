using UnityEngine;
using System.Collections;
using System;

public class Ability : Attacks {

    public Ability(uint id, string name, float heal, float damage, float castTime, float duration, float cooldown, GameObject abilityPrefab) : base(0, name, heal, damage, castTime, duration, cooldown, abilityPrefab)
    {

    }

    public override void Use()
    {
        throw new NotImplementedException();
    }

}
