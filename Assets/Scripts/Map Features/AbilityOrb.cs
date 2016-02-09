using UnityEngine;
using System.Collections;

public class AbilityOrb : MonoBehaviour {

    private Attacks attack;

    public void GetRandomAttack(GameObject caster)
    {
        switch (Random.Range(1, AbilityManager.Instance.spells.Length))
        {
            case 1: attack  = AbilityManager.Instance.CreateAries(caster); break;
            case 2: attack  = AbilityManager.Instance.CreateTaurus(caster); break;
            case 3: attack  = AbilityManager.Instance.CreateGemini(caster); break;
            case 4: attack  = AbilityManager.Instance.CreateCancer(caster); break;
            case 5: attack  = AbilityManager.Instance.CreateLeo(caster); break;
            case 6: attack  = AbilityManager.Instance.CreateVirgo(caster); break;
            case 7: attack  = AbilityManager.Instance.CreateLibra(caster); break;
            case 8: attack  = AbilityManager.Instance.CreateScorpio(caster); break;
            case 9: attack  = AbilityManager.Instance.CreateSaggitarius(caster); break;
            case 10: attack = AbilityManager.Instance.CreateCapricorn(caster); break;
            case 11: attack = AbilityManager.Instance.CreateAquarius(caster); break;
            case 12: attack = AbilityManager.Instance.CreatePisces(caster); break;
        }
    }

    void Start()
    {
        /*GetRandomAttack();*/
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (attack != null)
            {
                other.gameObject.GetComponent<Player>().PickupOrb(attack);
                DestroyPrefab();
            }
        }
    }

    public void DestroyPrefab()
    {

    }

    public Attacks Attack { get { return attack; } set { attack = value; } }
}
