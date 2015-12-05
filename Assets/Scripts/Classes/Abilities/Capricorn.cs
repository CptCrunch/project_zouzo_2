using UnityEngine;
using System.Collections;
using System;

public class Capricorn : Attacks {

    private int time;
    private float height;
    private float maxKnockBackDistance;

    public Capricorn(float damage, float castTime, float travleTime, float duration, float cooldown, float range, int targets, float height, int time) : base(
        10, "capricorn", "CC", true, targets, 0, damage, castTime, travleTime, duration, cooldown, range)
    {
        this.time = time;
        this.height = height;
    }

    public float MaxKockBackDistance { get { return maxKnockBackDistance; } }

    public override void Use(GameObject _target, GameObject _caster) {
        
        // get the vitals of the target
        LivingEntity Vitals = _target.GetComponent<Player>().playerVitals;

        if (_caster.GetComponent<Controller2D>().collisions.below)
        {
            // kock the target up and deal damage
            try
            {
                Vitals.ApplyPlayerKnockUp(height, time);
            }
            catch (OverflowException)
            {
                Debug.LogError("Is Outside range of Int32 tyoe.");
            }

            Vitals.GetDamage(Damage);
        }

        else
        {
            float dirX = _caster.transform.position.x - _target.transform.position.x;
            float dirY = _target.transform.position.y - _caster.transform.position.y;
            Vector2 direction = new Vector2(dirX, dirY);
            _target.GetComponent<Player>().playerVitals.ApplyPlayerKnockBack(direction.x, direction.y, 20);
        }
    }
}
