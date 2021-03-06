﻿using UnityEngine;
using System.Collections;
using System;
using System.Threading;

public class Basic : Attacks {

    private static int instanceCount = 0;

    public Basic(GameObject caster, Sprite[] icons, int damage, float castTime, float delay, float duration, float cooldown, float range, int targets, uint spellDir) : base(
        caster, 0, icons, "basic", "meele", targets, damage, castTime, delay, duration, cooldown, range, spellDir) { instanceCount++; }

    public override void StartSpell()
    {
        if (!IsDisabled)
        {
            // set spell as started
            IsStarted = true;

            // set animation
            Caster.GetComponent<Animator>().SetInteger("AttackRan", UnityEngine.Random.Range(1, 4));
            Caster.GetComponent<Animator>().SetTrigger("Attack");
            CustomDebug.Log("<b>" + PlayerVitals.Name + "</b> should play <b><color=white>" + Name + "</color></b> attack animation", "Animation");

            // reset TimeBetweenCasts
            TimeBeteewnCasts = 0;

            // set spell on cooldown
            SetCooldown();
        }

        // debug that spell is on cooldown
        else { CustomDebug.Log("<b><color=white>" + Name + "</color></b> is on <color=blue>cooldown</color> for: <color=blue>" + CurrCooldown + "</color> sec", "Cooldown"); }
    }

    public override void Cast()
    {
        // set spell as not started
        IsStarted = false;

        // set spell as cast
        IsCast = true;

        // cast spell
        PlayerAbilitiesScript.castedMeeleSpell = this;
        CustomDebug.Log("<b>" + PlayerVitals.Name + "</b> casted<b><color=white> " + Name + "</color></b>", "Spells");
    }

    public override void AfterCast()
    {
        // set spell as not cast
        IsCast = false;
    }

    public override void Use(GameObject _target)
    {
        // get the vitals of the target
        LivingEntity Vitals = _target.GetComponent<Player>().playerVitals;

        // deal damage
        Vitals.GetDamage(Damage);
    }

    public int InstanceCount { get { return instanceCount; } }
}
