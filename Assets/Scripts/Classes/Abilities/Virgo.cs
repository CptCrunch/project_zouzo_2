using UnityEngine;
using System.Collections;

public class Virgo : Attacks {

    private float dashStrength;
    private float dashTime;
    private float knockBackStrength;
    private float knockBackTime;
    private float knockBackDirection;
    private float stunTime;

    public Virgo(GameObject caster, Sprite[] icons, int damage, float castTime, float delay, float duration, float cooldown, float range, int targets, uint spellDir, float dashStrength, float dashTime, float knockBackStrength, float knockBackTime, float stunTime) : base(
        caster, 6, icons, "Virgo", "utility ", targets, damage, castTime, delay, duration, cooldown, range, spellDir)
    {
        this.knockBackStrength = knockBackStrength;
        this.knockBackTime = knockBackTime;
        this.dashStrength = dashStrength;
        this.dashTime = dashTime;
        this.stunTime = stunTime;
    }

    public float StunTime { get { return stunTime; } }

    public override void StartSpell()
    {
        if (!IsDisabled)
        {
            // set spell as started
            IsStarted = true;

            // set animation
            Caster.GetComponent<Animator>().SetBool("Virgo", true);
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

        // player dash
        if (Caster.GetComponent<Player>().Mirror) { PlayerVitals.ApplyDash(-dashStrength, dashTime); knockBackDirection = 1; }
        else { Caster.GetComponent<Player>().playerVitals.ApplyDash(dashStrength, dashTime); knockBackDirection = -1; }

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
        LivingEntity targetVitals = _target.GetComponent<Player>().playerVitals;

        // knock target back
        targetVitals.ApplyKnockBack(knockBackStrength * knockBackDirection, knockBackTime, this);

        // deal damage
        targetVitals.GetDamage(Damage);
    }
}
