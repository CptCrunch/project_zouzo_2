using UnityEngine;
using System.Collections;

public class Leo : Attacks {

    private static int instanceCount = 0;
    private int maxCharge;
    private int currCharge;

    public Leo(float damage, float castTime, float delay, float duration, float cooldown, float range, int targets, int maxCharge) : base(
        5, "leo", "meele", targets, damage, castTime, delay, duration, cooldown, range)
    {
        instanceCount++;
        this.maxCharge = maxCharge;
        this.currCharge = this.maxCharge;
    }

    public override void Cast(GameObject _caster)
    {
        // get playerScript from caster
        Player playerScript = _caster.GetComponent<Player>();

        if (!IsDisabled)
        {
            playerScript.castedSpell = this;
            SetCooldowne();
        }
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
        CurrCooldown -= Time.deltaTime;

        if (CurrCooldown <= 0)
        {
            currCharge++;
            IsDisabled = false;

            if (currCharge >= maxCharge) { CurrCooldown = 0; }
            else { CurrCooldown += MaxCooldown; }
        }
    }

    public override void SetCooldowne()
    {
        // set cooldown
        CurrCooldown = MaxCooldown;
        
        // sub charge
        currCharge--;
        Debug.Log("currCharges: " + currCharge);

        // setunable spell if no charges are left
        if (currCharge <= 0) { IsDisabled = true; }
        else if (IsDisabled) { IsDisabled = false; }
    }

    public int InstanceCount { get { return instanceCount; } }
}
