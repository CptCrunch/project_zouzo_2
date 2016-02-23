using UnityEngine;
using System.Collections;

public class Leo : Attacks {

    private static int instanceCount = 0;
    private int maxCharge;
    private int currCharge;
    private float maxChargeCooldown;
    private float currChargeCooldown;
    private bool castDisable = false;

    private int attackAnim = 0;
    private float animReset = 0.0f;

    public Leo(GameObject caster, int damage, float castTime, float delay, float duration, float cooldown, float range, int targets, uint spellDir, int maxCharge, float maxChargeCooldown) : base(
        caster, 5, "leo", "meele", targets, damage, castTime, delay, duration, cooldown, range, spellDir)
    {
        instanceCount++;
        this.maxCharge = maxCharge;
        this.currCharge = this.maxCharge;
        this.maxChargeCooldown = maxChargeCooldown;
    }

    public override void StartSpell()
    {
        if (!IsDisabled && !castDisable)
        {
            // set spell as started
            IsStarted = true;

            // set animation
            attackAnim++;
            Caster.GetComponent<Animator>().SetInteger("LeoAttack", attackAnim);
            Caster.GetComponent<Animator>().SetTrigger("LeoTrigger");
            animReset = 1.0f;
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
        Vitals.GetDamage(Damage + Damage * (attackAnim - 1) / 2);
    }

    public override void UpdateCooldowns()
    {
        // reduce current cooldown
        CurrCooldown -= Time.deltaTime;
        currChargeCooldown -= Time.deltaTime;
        animReset -= Time.deltaTime;

        //Animation Reset
        if(animReset <= 0)
        {
            animReset = 0;
            attackAnim = 0;
        }

        // enable casting if cooldown is over
        if (currChargeCooldown <= 0)
        {
            currChargeCooldown = 0;
            castDisable = false;
        }

        // check if cooldown is over 
        if (CurrCooldown <= 0)
        {
            // enable spell
            IsDisabled = false;

            // if charges are max stacked set cooldown to 0
            if (currCharge >= maxCharge) { CurrCooldown = 0; }
            // else add max cooldown to restart charg stacking
            else
            {
                CurrCooldown += MaxCooldown;
                
                // add one charge
                currCharge++;
            }
        }
    }

    public override void SetCooldown()
    {
        // set cooldown
        CurrCooldown = MaxCooldown;
        currChargeCooldown = maxChargeCooldown;

        // disable casting
        castDisable = true;
        
        // sub charge
        currCharge--;
        CustomDebug.Log("<b><color=white>" + Name + "</color></b>s currCharges: " + currCharge, "Spells");

        // disable spell if no charges are left
        if (currCharge <= 0) { IsDisabled = true; }
        else if (IsDisabled) { IsDisabled = false; }
    }

    public int InstanceCount { get { return instanceCount; } }
}
