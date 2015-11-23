using UnityEngine;
using System.Collections;

public class Basic : Attacks {

    public Basic(float damage, float castTime, float duration, float cooldown, float range) : base(
        0, "basic", "meele", true, 0, damage, castTime, duration, cooldown, range) { }

    public override void Use(GameObject _target)
    {
        _target.GetComponent<Player>().playerVitals.GetDamage(Damage);
        OnCooldown = true;
    }
}
