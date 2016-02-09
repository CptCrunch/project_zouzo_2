using UnityEngine;
using System.Collections;

public class Virgo : Attacks {

    private float dashStrength;
    private float dashTime;
    private float knockBackStrength;
    private float knockBackTime;
    private float knockBackDirection;
    private float stunTime;

    public Virgo(GameObject caster, float damage, float castTime, float delay, float duration, float cooldown, float range, int targets, uint spellDir, float dashStrength, float dashTime, float knockBackStrength, float knockBackTime, float stunTime) : base(
        caster, 6, "virgo", "utility ", targets, damage, castTime, delay, duration, cooldown, range, spellDir)
    {
        this.knockBackStrength = knockBackStrength;
        this.knockBackTime = knockBackTime;
        this.dashStrength = dashStrength;
        this.dashTime = dashTime;
        this.stunTime = stunTime;
    }

    public float StunTime { get { return stunTime; } }

    public override void Cast(GameObject _caster)
    {
        // get playerScript from caster
        Player playerScript = _caster.GetComponent<Player>();

        if (!IsDisabled)
        {
            // wait castTime

            // set animation
            CustomDebug.Log("<b>" + playerScript.playerVitals.Name + "</b> should play <b><color=white>" + Name + "</color></b> attack animation", "Animation");

            // cast spell
            playerScript.castedMeeleSpell = this;
            CustomDebug.Log("<b>" + playerScript.playerVitals.Name + "</b> casted<b><color=white> " + Name + "</color></b>", "Spells");

            // set spell as casted
            IsCasted = true;

            // player dash
            if (playerScript.Mirror) { playerScript.playerVitals.ApplyDash(-dashStrength, dashTime); knockBackDirection = 1; }
            else { playerScript.playerVitals.ApplyDash(dashStrength, dashTime); knockBackDirection = -1; }

            // reset TimeBetweenCasts
            TimeBeteewnCasts = 0;

            // set spell on cooldown
            SetCooldown();
        }

        // debug that spell is on cooldown
        else { CustomDebug.Log("<b><color=white>" + Name + "</color></b> is on <color=blue>cooldown</color> for: <color=blue>" + CurrCooldown + "</color> sec", "Cooldown"); }
    }

    public override void AfterCast()
    {
        IsCasted = false;
    }
    public override void Use(GameObject _target, GameObject _caster)
    {
        // get the vitals of the target
        LivingEntity targetVitals = _target.GetComponent<Player>().playerVitals;

        // knock target back
        targetVitals.ApplyKnockBack(knockBackStrength * knockBackDirection, knockBackTime, this);

        // deal damage
        targetVitals.GetDamage(Damage);
    }
}
