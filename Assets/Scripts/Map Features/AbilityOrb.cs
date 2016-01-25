using UnityEngine;
using System.Collections;

public class AbilityOrb : MonoBehaviour {

    private Attacks attack;

    public void GetRandomAttack()
    {
        switch (Random.Range(1, AbilityManager._instance.spells.Length))
        {
            case 1:
                attack = AbilityManager._instance.CreateAries();
                break;
            case 2:
                attack = AbilityManager._instance.CreateTaurus();
                break;
            case 3:
                attack = AbilityManager._instance.CreateGemini();
                break;
            case 4:
                attack = AbilityManager._instance.CreateCancer();
                break;
            case 5:
                attack = AbilityManager._instance.CreateLeo();
                break;
            case 6:
                attack = AbilityManager._instance.CreateVirgo();
                break;
            case 7:
                attack = AbilityManager._instance.CreateVirgo();
                break;
            case 8:
                attack = AbilityManager._instance.CreateScorpio();
                break;
            case 9:
                attack = AbilityManager._instance.CreateSaggitarius();
                break;
            case 10:
                attack = AbilityManager._instance.CreateCapricorn();
                break;
            case 11:
                attack = AbilityManager._instance.CreateAquarius();
                break;
            case 12:
                attack = AbilityManager._instance.CreatePisces();
                break;
        }
    }

    
    public void PickUp()
    {

    }

    public void DestroyPrefab()
    {

    }

    public Attacks Attack { get { return attack; } set { attack = value; } }
}
