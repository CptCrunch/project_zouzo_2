using UnityEngine;
using System.Collections;
using System;

public class Capricorn : Attacks {

    private float height;
    private float knockBackRange;
    private bool usedInAir = false;
    private float knockBackStrenght;
    private bool airCast = false;

    public Capricorn(GameObject caster, float damage, float castTime, float travleTime, float duration, float cooldown, float range, int targets, uint spellDir, float height, float knockBackRange, float knockBackStrenght) : base(
        caster, 10, "capricorn", "CC", targets, damage, castTime, travleTime, duration, cooldown, range, spellDir)
    {
        this.height = height;
        this.knockBackRange = knockBackRange;
        this.knockBackStrenght = knockBackStrenght;
    }

    public float KnockBackDistance { get { return knockBackRange; } }
    public float KnockBackStrenght { get { return knockBackStrenght; } }
    public bool UsedInAir { get { return usedInAir; } }

    public override void StartSpell()
    {
        //check if knockUp should be used
        if (Caster.GetComponent<Controller2D>().collisions.below)
        {
            if (!IsDisabled)
            {
                // set spell as started
                IsStarted = true;

                // set animation
                Caster.GetComponent<Animator>().SetTrigger("CapricornKnockBack");
                CustomDebug.Log("<b>" + Caster.GetComponent<Player>().playerVitals.Name + "</b> should play <b><color=white>" + Name + "</color></b> attack animation", "Animation");

                // set air cast to false
                airCast = false;

                // set spell on cooldown
                SetCooldown();
            }

            // debug that spell is on cooldown
            else { CustomDebug.Log("<b><color=white>" + Name + "</color></b> is on <color=blue>cooldown</color> for: <color=blue>" + CurrCooldown + "</color> sec", "Cooldown"); }
        }

        else
        {
            if (!usedInAir)
            {
                if (PlayerAbilitiesScript.GetCapricorn2Targets() != null)
                {
                    // set spell as started
                    IsStarted = true;

                    // set animation
                    Caster.GetComponent<Animator>().SetTrigger("CapricornKnockUp");
                    CustomDebug.Log("<b>" + Caster.GetComponent<Player>().playerVitals.Name + "</b> should play <b><color=white>" + Name + "2</color></b> attack animation", "Animation");

                    // set air cast to false
                    airCast = true;

                    // reset TimeBetweenCasts
                    TimeBeteewnCasts = 0;

                    // reset cooldown
                    SetCooldown();
                }

                // debug that spell is on cooldown
                else { CustomDebug.Log("no target in range for <b><color=white>" + Name + "2</color></b>", "Spells"); }
            }
            else { CustomDebug.Log("<b><color=white>" + Name + "</color></b> is on <color=blue>cooldown</color> for: <color=blue>" + CurrCooldown + "</color> sec", "Cooldown"); }
        }
    }

    public override void Cast()
    {
        // set spell as not started
        IsStarted = false;

        // set spell as cast
        IsCast = true;

        if (!airCast)
        {
            // cast spell
            PlayerAbilitiesScript.castedMeeleSpell = this;
            CustomDebug.Log("<b>" + Caster.GetComponent<Player>().playerVitals.Name + "</b> casted<b><color=white> " + Name + "</color></b>", "Spells");
        }

        else
        {
            if (PlayerAbilitiesScript.GetCapricorn2Targets() != null)
            {
                // use spell
                UseCapricorn2(Caster, PlayerAbilitiesScript.GetCapricorn2Targets());
                CustomDebug.Log("<b>" + Caster.GetComponent<Player>().playerVitals.Name + "</b> casted<b><color=white> " + Name + "2</color></b>", "Spells");
            }
        }
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

        // knock the target up and deal damage
        try { Vitals.ApplyKnockUp(height, this); }
        catch (OverflowException) { Debug.LogError("Is Outside range of Int32 tyoe."); }

        // deal damage
        Vitals.GetDamage(Damage);
    }

    public void UseCapricorn2(GameObject _caster, GameObject _target)
    {
        // get the vitals of the target
        LivingEntity targetVitals = _target.GetComponent<Player>().playerVitals;

        // get distance between caster and target
        float dirX = _target.transform.position.x - _caster.transform.position.x;
        float dirY = _target.transform.position.y - _caster.transform.position.y;

        // get mutiplicator
        float multi = Mathf.Sqrt(1 / (dirX * dirX + dirY * dirY));

        // kock target away form caster
        float time = targetVitals.ApplyKnockUp(dirY * multi * knockBackStrenght, this);
        Debug.Log("KnockUpTime: " + time);
        targetVitals.ApplyKnockBack(dirX * multi * knockBackStrenght, time, this);

        // deal damage
        targetVitals.GetDamage(Damage);

        // disable Capricorn2
        usedInAir = true;
    }

    public override void UpdateCooldowns()
    {
        CurrCooldown -= Time.deltaTime;
        if (CurrCooldown <= 0) { CurrCooldown = 0; IsDisabled = false; usedInAir = false; }
    }
}
