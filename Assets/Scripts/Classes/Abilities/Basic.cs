using UnityEngine;
using System.Collections;
using System;

public class Basic : Attacks {

    private static int instanceCount = 0;

    public Basic(float damage, float castTime, float delay, float duration, float cooldown, float range, int targets) : base(
        0, "basic", "meele", targets, damage, castTime, delay, duration, cooldown, range) { instanceCount++; }

    public override void Cast(GameObject _caster)
    {
        // get playerScript from caster
        Player playerScript = _caster.GetComponent<Player>();

        if (!IsDisabled)
        {
            _caster.GetComponent<Animator>().SetInteger("AttackRan", UnityEngine.Random.Range(1, 4));
            _caster.GetComponent<Animator>().SetTrigger("Attack");

            playerScript.castedSpell = this;
            SetCooldowne();
        }

        IsAbilityCasted = true;
    }

    public override void AfterCast()
    {
        IsAbilityCasted = false;
    }

    public override void Use(GameObject _target, GameObject _caster)
    {
        // get the vitals of the target
        LivingEntity Vitals = _target.GetComponent<Player>().playerVitals;

        // deal damage
        Vitals.GetDamage(Damage);
        _target.GetComponent<Animator>().SetTrigger("Damage");
    }

    public int InstanceCount { get { return instanceCount; } }
}
