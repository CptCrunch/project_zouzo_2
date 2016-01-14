using UnityEngine;
using System.Collections;

public class Leo : Attacks {

    private static int instanceCount = 0;
    private int maxCharge;
    private int currCharge;
    private float maxChargeCooldown;
    private float currChargeCooldown;
    private bool castDisable = false;

    public Leo(float damage, float castTime, float delay, float duration, float cooldown, float range, int targets, uint spellDir, int maxCharge, float maxChargeCooldown) : base(
        5, "leo", "meele", targets, damage, castTime, delay, duration, cooldown, range, spellDir)
    {
        instanceCount++;
        this.maxCharge = maxCharge;
        this.currCharge = this.maxCharge;
        this.maxChargeCooldown = maxChargeCooldown;
        this.currChargeCooldown = currChargeCooldown;
    }

    public override void Cast(GameObject _caster)
    {
        // get playerScript from caster
        Player playerScript = _caster.GetComponent<Player>();

        if (!IsDisabled && !castDisable)
        {
            // wait castTime

            // set animation
            _caster.GetComponent<Animator>().SetInteger("LeoAttack", currCharge);
            _caster.GetComponent<Animator>().SetTrigger("LeoTrigger");
            
            // cast spell
            playerScript.castedMeeleSpell = this;

            // set spell on cooldown
            SetCooldown();
        }

        // debug that spell is on cooldown
        else { CustomDebuger.Log("leo is on cooldown for: " + CurrCooldown); }
    }

    public override void AfterCast()
    {
        IsAbilityCasted = false;
    }

    public override void Use(GameObject _target, GameObject _caster)
    {
        // get the vitals of the target
        LivingEntity Vitals = _target.GetComponent<Player>().playerVitals;

        // deal damage
        Vitals.GetDamage(Damage);
    }

    public override void UpdateCooldowns()
    {
        // reduce current cooldown
        CurrCooldown -= Time.deltaTime;
        currChargeCooldown -= Time.deltaTime;

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
        CustomDebuger.Log("currCharges: " + currCharge);

        // disable spell if no charges are left
        if (currCharge <= 0) { IsDisabled = true; }
        else if (IsDisabled) { IsDisabled = false; }
    }

    public int InstanceCount { get { return instanceCount; } }
}
