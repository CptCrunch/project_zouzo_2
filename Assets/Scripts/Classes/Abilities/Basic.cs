using UnityEngine;
using System.Collections;
using System;
using System.Threading;

public class Basic : Attacks {

    private static int instanceCount = 0;

    public Basic(float damage, float castTime, float delay, float duration, float cooldown, float range, int targets, uint spellDir) : base(
        0, "basic", "meele", targets, damage, castTime, delay, duration, cooldown, range, spellDir) { instanceCount++; }

    public override void Cast(GameObject _caster)
    {
        // get playerScript from caster
        Player playerScript = _caster.GetComponent<Player>();

        if (!IsDisabled)
        {
            // wait castTime

            // set animation
            _caster.GetComponent<Animator>().SetInteger("AttackRan", UnityEngine.Random.Range(1, 4));
            _caster.GetComponent<Animator>().SetTrigger("Attack");
            Debug.Log("Should Play");

            // cast spell
            playerScript.castedMeeleSpell = this;

            // set spell on cooldown
            SetCooldown();
        }

        // debug that spell is on cooldown
        else { Debug.Log("basic is on cooldown for: " + CurrCooldown); }

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
