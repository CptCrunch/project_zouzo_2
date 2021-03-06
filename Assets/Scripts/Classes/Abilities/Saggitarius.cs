﻿using UnityEngine;
using System.Collections;

public class Saggitarius : Attacks {

    private static int instanceCount = 0;
    private GameObject bullet;
    private float timeToGetMaxRange;
    private bool isSticky = false;
    private float stickArrowPerCent;
    private float minRange;
    private float maxRange;
    private float castSlow;
    private float normSpeed;
    private GameObject caster;


    public Saggitarius(GameObject caster, Sprite[] icons, int damage, float castTime, float delay, float duration, float cooldown, float range, int targets, uint spellDir, GameObject bullet, float timeToGetMaxRange, float minRange, float stickArrowPerCent, float castSlow) : base(
        caster, 9, icons, "Sagittarius", "skillshot", targets, damage, castTime, delay, duration, cooldown, 0, spellDir)
    {
        instanceCount++;
        this.bullet = bullet;
        this.timeToGetMaxRange = timeToGetMaxRange;
        this.minRange = minRange;
        this.maxRange = range;
        this.stickArrowPerCent = stickArrowPerCent;
        this.castSlow = castSlow;
    }

    public bool IsSticky { get { return isSticky; } }

    public override void StartSpell()
    {
        if (!IsDisabled)
        {   
            // set spell as started
            IsStarted = true;

            // slow player
            PlayerVitals.ApplyLaunch(true, castSlow);

            // reset TimeBetweenCasts
            TimeBeteewnCasts = 0;

            // set animation
            Caster.GetComponent<Animator>().SetBool("SagittariusActive", true);
            CustomDebug.Log("<b>" + PlayerVitals.Name + "</b> should play <b><color=white>" + Name + "</color></b> attack animation", "Animation");

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
    }

    public override void AfterCast()
    {
        if (IsCast)
        {
            // get range
            if (TimeBeteewnCasts >= timeToGetMaxRange) { Range = 0; }
            else { Range = minRange + (maxRange - minRange) * TimeBeteewnCasts / timeToGetMaxRange; }

            // check if arrow is sticky
            if (TimeBeteewnCasts >= timeToGetMaxRange * stickArrowPerCent) { isSticky = true; }

            // cast spell
            CustomDebug.Log("Found bullet: " + bullet != null, "Testing");
            PlayerAbilitiesScript.FireSkillShot(this, bullet);
            CustomDebug.Log("<b>" + PlayerVitals.Name + "</b> casted<b><color=white> " + Name + "</color></b>", "Spells");

            // set spell on cooldown
            SetCooldown();
        }

        else
        {
            // set spell on a low cooldown
            CurrCooldown = MaxCooldown / 4;
            IsDisabled = true;
        }
        // set norm speed
        PlayerVitals.ApplyLaunch(false, castSlow);

        // animation set false
        Caster.GetComponent<Animator>().SetBool("SagittariusActive", false);

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
}
