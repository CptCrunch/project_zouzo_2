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
            CustomDebug.Log("<b>" + _caster.GetComponent<Player>().playerVitals.Name + "</b> should play <b><color=white>" + Name + "</color></b> attack animation", "Animation");

            // cast spell
            playerScript.castedMeeleSpell = this;
            CustomDebug.Log("<b>" + _caster.GetComponent<Player>().playerVitals.Name + "</b> casted<b><color=white> " + Name + "</color></b>", "Spells");

            // set spell as casted
            IsCasted = true;

            // reset TimeBetweenCasts
            TimeBeteewnCasts = 0;

            // set spell on cooldown
            SetCooldown();
        }

        // debug that spell is on cooldown
        else { CustomDebug.Log("<b><color=white>" + Name + "</color></b> is on <color=blue>cooldown</color> for: <color=blue>" + CurrCooldown + "</color> sec", "Spells"); }
    }

    public override void AfterCast()
    {
        IsCasted = false;
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
