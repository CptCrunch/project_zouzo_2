using UnityEngine;
using System.Collections;

public class AbilityOrb : MonoBehaviour {

    public AbilityOrbValues point;
    //private string[] attacks = new string[12] { "Aries", "Taurus", "Gemini", "Cancer", "Leo", "Virgo", "Libra", "Scorpio", "Saggitarius", "Capricorn", "Aquarius", "Pisces" };
    private string[] attacks = new string[4] { "Leo", "Virgo","Saggitarius", "Capricorn"};
    public string chosenAttackName;

    /*private Attacks GetRandomAttack(GameObject caster)
    {
        Attacks attack = null;

        switch (chosenAttackName)
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

        return attack;
    }*/

    private Attacks GetRandomAttack(GameObject caster)
    {
        Attacks attack = null;

        switch (chosenAttackName)
        {
            case "Leo": attack = AbilityManager.Instance.CreateLeo(caster); break;
            case "Virgo": attack = AbilityManager.Instance.CreateVirgo(caster); break;
            case "Saggitarius": attack = AbilityManager.Instance.CreateSaggitarius(caster); break;
            case "Capricorn": attack = AbilityManager.Instance.CreateCapricorn(caster); break;
        }

        return attack;
    }

    void Start()
    {
        chosenAttackName = attacks[Random.Range(0, attacks.Length)];

        if (OrbSpawner.Instance.orbSprites.Length > 0) {
            /*switch(chosenAttackName)
            {
                case "Aries": SetSprite(OrbSpawner.Instance.orbSprites[0]); break;
                case "Taurus": SetSprite(OrbSpawner.Instance.orbSprites[1]); break;
                case "Gemini": SetSprite(OrbSpawner.Instance.orbSprites[2]); break;
                case "Cancer": SetSprite(OrbSpawner.Instance.orbSprites[3]); break;
                case "Leo": SetSprite(OrbSpawner.Instance.orbSprites[3]); break;
                case "Virgo": SetSprite(OrbSpawner.Instance.orbSprites[4]); break;
                case "Libra": SetSprite(OrbSpawner.Instance.orbSprites[5]); break;
                case "Scorpio": SetSprite(OrbSpawner.Instance.orbSprites[6]); break;
                case "Saggitarius": SetSprite(OrbSpawner.Instance.orbSprites[7]); break;
                case "Capricorn": SetSprite(OrbSpawner.Instance.orbSprites[8]); break;
                case "Aquarius": SetSprite(OrbSpawner.Instance.orbSprites[9]); break;
                case "Pisces": SetSprite(OrbSpawner.Instance.orbSprites[10]); break;
            }*/

            switch (chosenAttackName) {
                case "Leo": SetSprite(OrbSpawner.Instance.orbSprites[0]); break;
                case "Virgo": SetSprite(OrbSpawner.Instance.orbSprites[1]); break;
                case "Saggitarius": SetSprite(OrbSpawner.Instance.orbSprites[2]); break;
                case "Capricorn": SetSprite(OrbSpawner.Instance.orbSprites[3]); break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().PickupOrb(GetRandomAttack(other.gameObject));
            
            DestroyPrefab();
        }
    }

    private void SetSprite(Sprite sprite)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }


    private void DestroyPrefab()
    {
        point.Active = false;
        Destroy(gameObject);
    }
}
