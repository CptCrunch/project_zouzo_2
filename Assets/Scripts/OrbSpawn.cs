using UnityEngine;
using System.Collections;

public class OrbSpawn : MonoBehaviour {

    public float maxCooldown = 0f;
    private float currCooldown = 0;

    public float despawnTime = 0f;
    public float nextDespawn = 0;

    private bool alowedToSpawn = false;

    [HideInInspector] public GameObject orb = null;
	
	// Update is called once per frame
	void Update ()
    {
        if (GameManager._instance.Running)
        {
            if (orb == null)
            {
                if (currCooldown <= 0) { if (OrbSpawner.Instance.enableSelfSpawning && alowedToSpawn) { if (OrbSpawner.Instance.SpawnOrb()) { alowedToSpawn = false; } } }
                currCooldown -= Time.deltaTime;
            }

            else
            {
                if (nextDespawn > 0) { nextDespawn -= Time.deltaTime; }
                else { DeliteOrb(); }
            }
        }
    }

    public void DeliteOrb()
    {
        Destroy(orb);
        orb = null;
        currCooldown = maxCooldown;
        OrbSpawner.Instance.orbCount--;
        alowedToSpawn = true;
    }

    public void SetNewOrb(GameObject _orb) { orb = _orb; nextDespawn = despawnTime; }
    public float Cooldown { get { return currCooldown; } }
}
