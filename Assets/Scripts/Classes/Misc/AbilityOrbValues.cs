using UnityEngine;
using System.Collections;

public class AbilityOrbValues {

    private GameObject spawnPoint;
    private GameObject orbPrefab;
    private bool active;

    public AbilityOrbValues(GameObject spawnPoint, GameObject orbPrefab)
    {
        this.spawnPoint = spawnPoint;
        this.orbPrefab = orbPrefab;
        active = false;
    }

    public GameObject SpawnPoint { get { return spawnPoint; } set { this.spawnPoint = value; } }
    public GameObject OrbPrefab { get { return orbPrefab; } set { orbPrefab = value; } }
    public bool Active { get { return active; } set { active = value; } }
}
