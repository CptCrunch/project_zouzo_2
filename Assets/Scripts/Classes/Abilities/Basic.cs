using UnityEngine;
using System.Collections;

public class Basic : Attacks {

    public Basic(float damage, float castTime, float duration, float cooldown, float range) : base(
        0, "basic", "meele", true, 0, damage, castTime, duration, cooldown, range) { }

    public override void Use(GameObject _target) {
        
        // get the vitals of the target
        LivingEntity Vitals = _target.GetComponent<Player>().playerVitals;

        // deal damage
        Vitals.GetDamage(Damage);

        // set spell onto cooldown
        OnCooldown = true;
    }
}
