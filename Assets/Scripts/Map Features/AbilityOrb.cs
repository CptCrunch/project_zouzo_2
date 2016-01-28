using UnityEngine;
using System.Collections;

public class AbilityOrb : MonoBehaviour {

    private Attacks attack;

    public void GetRandomAttack()
    {
        switch (Random.Range(1, AbilityManager.Instance.spells.Length))
        {
            case 1: attack  = AbilityManager.Instance.CreateAries(); break;
            case 2: attack  = AbilityManager.Instance.CreateTaurus(); break;
            case 3: attack  = AbilityManager.Instance.CreateGemini(); break;
            case 4: attack  = AbilityManager.Instance.CreateCancer(); break;
            case 5: attack  = AbilityManager.Instance.CreateLeo(); break;
            case 6: attack  = AbilityManager.Instance.CreateVirgo(); break;
            case 7: attack  = AbilityManager.Instance.CreateVirgo(); break;
            case 8: attack  = AbilityManager.Instance.CreateScorpio(); break;
            case 9: attack  = AbilityManager.Instance.CreateSaggitarius(); break;
            case 10: attack = AbilityManager.Instance.CreateCapricorn(); break;
            case 11: attack = AbilityManager.Instance.CreateAquarius(); break;
            case 12: attack = AbilityManager.Instance.CreatePisces(); break;
        }
    }

    void Start()
    {
        GetRandomAttack();
    }

    public void PickUp()
    {

    }

    public void DestroyPrefab()
    {

    }

    public Attacks Attack { get { return attack; } set { attack = value; } }
}
