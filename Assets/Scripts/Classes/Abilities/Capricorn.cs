using UnityEngine;
using System.Collections;
using System;

public class Capricorn : Attacks {

    private float height;
    private float knockBackRange;
    private bool usedInAir = false;
    private float knockBackStrenght;

    public Capricorn(float damage, float castTime, float travleTime, float duration, float cooldown, float range, int targets, uint spellDir, float height, float knockBackRange, float knockBackStrenght) : base(
        10, "capricorn", "CC", targets, damage, castTime, travleTime, duration, cooldown, range, spellDir)
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

                // cast spell
                playerScript.castedMeeleSpell = this;

                // set spell on cooldown
                SetCooldown();
            }

            // debug that spell is on cooldown
            else { Debug.Log("<b><color=white>capricorn</color></b> is on <color=blue>cooldown</color> for: <color=blue>" + CurrCooldown + "</color> sec"); }
        }

        else
        {
            if (!usedInAir)
            {
                if (playerScript.GetCapricorn2Targets() != null)
                {
                    // set animation

                    // use spell
                    UseCapricorn2(_caster, playerScript.GetCapricorn2Targets());

                    // reset cooldown
                    SetCooldown();
                }

                // debug that spell is on cooldown
                else { Debug.Log("no target in range for <b><color=white>Capricorn 2</color></b>"); }
            }
            else { Debug.Log("<b><color=white>capricorn</color></b> is on <color=blue>cooldown</color> for: <color=blue>" + CurrCooldown + "</color> sec"); }
        }
    }
    
    public override void AfterCast()
    {
        IsAbilityCasted = false;
    }

    public override void Use(GameObject _target, GameObject _caster) {
        
        // get the vitals of the target
        LivingEntity Vitals = _target.GetComponent<Player>().playerVitals;

        // knock the target up and deal damage
        try
        {
            Vitals.ApplyPlayerKnockUp(height);
        }
        catch (OverflowException)
        {
            Debug.LogError("Is Outside range of Int32 tyoe.");
        }

        // deal damage
        Vitals.GetDamage(Damage);
    }

    public void UseCapricorn2(GameObject _caster, GameObject _target)
    {
        Debug.Log("<b>" + _caster.GetComponent<Player>().playerVitals.Name + "</b> used <b><color=white>Capricorn2</color></b>");

        // get the vitals of the target
        LivingEntity Vitals = _target.GetComponent<Player>().playerVitals;

        // get distance between caster and target
        float dirX = _target.transform.position.x - _caster.transform.position.x;
        float dirY = _target.transform.position.y - _caster.transform.position.y;

        // get mutiplicator
        float multi = 1 / Mathf.Sqrt(dirX * dirX + dirY * dirY);
        
        // kock target away form caster
        _target.GetComponent<Player>().playerVitals.ApplyPlayerKnockBack(dirX * multi * knockBackStrenght, dirY * multi * knockBackStrenght);

        // deal damage
        Vitals.GetDamage(Damage);

        // disable Capricorn2
        usedInAir = true;
    }

    public override void UpdateCooldowns()
    {
        CurrCooldown -= Time.deltaTime;
        if (CurrCooldown <= 0) { CurrCooldown = 0; IsDisabled = false; usedInAir = false; }
    }
}
