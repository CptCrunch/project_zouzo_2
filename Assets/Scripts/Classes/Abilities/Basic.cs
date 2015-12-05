using UnityEngine;
using System.Collections;

public class Basic : Attacks {

    private static int instanceCount = 0;

    public Basic(float damage, float castTime, float delay, float duration, float cooldown, float range, int targets) : base(
        0, "basic", "meele", true, targets, 0, damage, castTime, delay, duration, cooldown, range) { instanceCount++; }

    public override void Use(GameObject _target, GameObject _caster, bool _inAir)
    {
        // get the vitals of the target
        LivingEntity Vitals = _target.GetComponent<Player>().playerVitals;

        // deal damage
        Vitals.GetDamage(Damage);
    }

    public int InstanceCount { get { return instanceCount; } }
}
