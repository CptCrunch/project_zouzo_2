using UnityEngine;
using System.Collections;
using System;

public class Ability : Attacks {

    public Ability(uint id, string name, float heal, float damage, float castTime, float cooldown) : base(id, name, heal, damage, castTime, cooldown)
    {

    }

    public override void Use()
    {
        throw new NotImplementedException();
    }

}
