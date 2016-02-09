using UnityEngine;
using System.Collections;
using System;

public class Capricorn : Attacks {

    private float height;
    private float knockBackRange;
    private bool usedInAir = false;
    private float knockBackStrenght;

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

    public override void Cast(GameObject _caster)
    {
        // get playerScript from caster
        Player playerScript = _caster.GetComponent<Player>();

        if (_caster.GetComponent<Controller2D>().collisions.below)
        {
            if (!IsDisabled)
            {
                // wait castTime

                // set animation
                CustomDebug.Log("<b>" + _caster.GetComponent<Player>().playerVitals.Name + "</b> should play <b><color=white>" + Name + "</color></b> attack animation", "Animation");

                // cast spell
                playerScript.castedMeeleSpell = this;
                CustomDebug.Log("<b>" + _caster.GetComponent<Player>().playerVitals.Name + "</b> casted<b><color=white> " + Name + "</color></b>", "Spells");

                // set spell as casted
                IsCasted = true;

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
                if (playerScript.GetCapricorn2Targets() != null)
                {
                    // set animation
                    CustomDebug.Log("<b>" + _caster.GetComponent<Player>().playerVitals.Name + "</b> should play <b><color=white>" + Name + "2</color></b> attack animation", "Animation");

                    // use spell
                    UseCapricorn2(_caster, playerScript.GetCapricorn2Targets());
                    CustomDebug.Log("<b>" + _caster.GetComponent<Player>().playerVitals.Name + "</b> casted<b><color=white> " + Name + "2</color></b>", "Spells");

                    // set spell as casted
                    IsCasted = true;

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
    
    public override void AfterCast()
    {
        if (IsCasted) { IsCasted = false; }
    }

    public override void Use(GameObject _target, GameObject _caster)
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
