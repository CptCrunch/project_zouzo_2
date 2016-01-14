using UnityEngine;
using System.Collections;
using System;

public class Capricorn : Attacks {

    private int time;
    private float height;
    private float knockBackDistance;
    private bool usedInAir = false;

    public Capricorn(float damage, float castTime, float travleTime, float duration, float cooldown, float range, int targets, uint spellDir, float height, int time, float knockBackDistance) : base(
        10, "capricorn", "CC", targets, damage, castTime, travleTime, duration, cooldown, range, spellDir)
    {
        this.time = time;
        this.height = height;
        this.knockBackDistance = knockBackDistance;
    }

    public float KnockBackDistance { get { return knockBackDistance; } }
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
            else { Debug.Log("capricorn is on cooldown for: " + CurrCooldown); }
        }

        else if(!usedInAir)
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
            else { Debug.Log("capricorn is on cooldown for: " + CurrCooldown); }
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
            Vitals.ApplyPlayerKnockUp(height, time);
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
        // get the vitals of the target
        LivingEntity Vitals = _target.GetComponent<Player>().playerVitals;

        // get distance between caster and target
        float dirX = _caster.transform.position.x - _target.transform.position.x;
        float dirY = _target.transform.position.y - _caster.transform.position.y;
        float distaceMultiplicator = 1 / (dirX + dirY) * knockBackDistance;

        // kock target away form caster
        _target.GetComponent<Player>().playerVitals.ApplyPlayerKnockBack(dirX * distaceMultiplicator, dirY * distaceMultiplicator, 20);

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
