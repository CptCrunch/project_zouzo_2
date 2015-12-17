using UnityEngine;
using System.Collections;

public class Saggitarius : Attacks {

    private static int instanceCount = 0;
    private GameObject bullet;


    public Saggitarius(float damage, float castTime, float delay, float duration, float cooldown, float range, int targets, uint spellDir, GameObject bullet) : base(
        9, "saggitarius", "skillshot", targets, damage, castTime, delay, duration, cooldown, range, spellDir)
    {
        instanceCount++;
        this.bullet = bullet;
    }

    public override void Cast(GameObject _caster)
    {
        // get playerScript from caster
        Player playerScript = _caster.GetComponent<Player>();

        if (!IsDisabled)
        {
            // wait castTime

            // set animation

            // cast spell
            playerScript.FireSkillShot(this, bullet);

            // set spell on cooldown
            SetCooldown();
        }

        // debug that spell is on cooldown
        else { Debug.Log("saggitarus is on cooldown for: " + CurrCooldown); }
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
}
