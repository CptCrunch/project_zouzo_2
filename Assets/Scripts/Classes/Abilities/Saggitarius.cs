using UnityEngine;
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


    public Saggitarius(float damage, float castTime, float delay, float duration, float cooldown, float range, int targets, uint spellDir, GameObject bullet, float timeToGetMaxRange, float minRange, float stickArrowPerCent, float castSlow) : base(
        9, "saggitarius", "skillshot", targets, damage, castTime, delay, duration, cooldown, 0, spellDir)
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

    public override void Cast(GameObject _caster)
    {
        if (!IsDisabled)
        {
            // set caster
            caster = _caster;

            // set spell as casted
            IsCasted = true;

            // slow player
            caster.GetComponent<Player>().playerVitals.ApplyLaunch(true, castSlow);

            // reset TimeBetweenCasts
            TimeBeteewnCasts = 0;

            // set animation
            CustomDebug.Log("<b>" + _caster.GetComponent<Player>().playerVitals.Name + "</b> should play <b><color=white>" + Name + "</color></b> attack animation", "Animation");

            // save cater
            caster = _caster;
        }

        // debug that spell is on cooldown
        else { CustomDebug.Log("<b><color=white>" + Name + "</color></b> is on <color=blue>cooldown</color> for: <color=blue>" + CurrCooldown + "</color> sec", "Spells"); }
    }

    public override void AfterCast()
    {
        if (IsCasted)
        {
            IsCasted = false;

            // get range
            if (TimeBeteewnCasts >= timeToGetMaxRange) { Range = 0; }
            else { Range = minRange + (maxRange - minRange) * TimeBeteewnCasts / timeToGetMaxRange; }

            // check if arrow is sticky
            if (TimeBeteewnCasts >= timeToGetMaxRange * stickArrowPerCent) { isSticky = true; }

            // get playerScript from caster
            Player playerScript = caster.GetComponent<Player>();

            // wait castTime

            // cast spell
            playerScript.FireSkillShot(this, bullet);
            CustomDebug.Log("<b>" + caster.GetComponent<Player>().playerVitals.Name + "</b> casted<b><color=white> " + Name + "</color></b>", "Spells");

            // set norm speed
            caster.GetComponent<Player>().playerVitals.ApplyLaunch(false, castSlow);

            // set spell on cooldown
            SetCooldown();
        }
    }

    public override void Use(GameObject _target, GameObject _caster)
    {
        // get the vitals of the target
        LivingEntity Vitals = _target.GetComponent<Player>().playerVitals;

        // deal damage
        Vitals.GetDamage(Damage);
    }
}
