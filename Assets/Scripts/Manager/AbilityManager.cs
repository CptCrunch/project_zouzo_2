using UnityEngine;
using System.Collections;

public class AbilityManager : MonoBehaviour {

    private Attacks capricornAbility;

    [Header("Capricorn")]
    public GameObject capricornPrefab;

	// Use this for initialization
	void Start () {
        capricornAbility = new Capricorn( 5f, 0f, 0f, 1f, capricornPrefab);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
