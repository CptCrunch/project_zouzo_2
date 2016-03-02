using UnityEngine;
using System.Collections;

public class OrbSpawn : MonoBehaviour {

    [HideInInspector] public float maxCooldown = 0f;
    private float currCooldown = 0;

    [HideInInspector] public float despawnTime = 0f;
    [HideInInspector] public float nextDespawn = 0;

    private bool alowedToSpawn = false;

    [HideInInspector] public GameObject orb = null;

	// Update is called once per frame
	void Update ()
    {
        if (!OrbSpawner.Instance.showDebugCooldowns) { if (transform.GetChild(0).gameObject.active) { transform.GetChild(0).gameObject.active = false; } }
        else { if(!transform.GetChild(0).gameObject.active) transform.GetChild(0).gameObject.active = true; }

        if (GameManager._instance.Running)
        {
            if (OrbSpawner.Instance.enableSelfSpawning && alowedToSpawn && currCooldown <= 0) { if (OrbSpawner.Instance.SpawnOrb()) { alowedToSpawn = false; } }
            if (orb == null)
            {
                currCooldown -= Time.deltaTime;

                // --- [ over head de bug ] ---
                if (OrbSpawner.Instance.showDebugCooldowns)
                {
                    transform.GetChild(0).GetComponent<TextMesh>().text = gameObject.name + "\n" + currCooldown.ToString();
                    if (currCooldown > 0) { transform.GetChild(0).GetComponent<TextMesh>().color = Color.red; }
                    else { transform.GetChild(0).GetComponent<TextMesh>().color = Color.green; }
                }
            }

            else
            {
                if (nextDespawn > 0) { nextDespawn -= Time.deltaTime; }
                else { DeliteOrb(); }

                // --- [ over head de bug ] ---
                if (OrbSpawner.Instance.showDebugCooldowns)
                {
                    transform.GetChild(0).GetComponent<TextMesh>().text = gameObject.name + "\n" + nextDespawn.ToString();
                    transform.GetChild(0).GetComponent<TextMesh>().color = Color.blue;
                }
            }
        }

        // --- [ over head de bug ] ---
        if (OrbSpawner.Instance.showDebugCooldowns)
        {
            if (alowedToSpawn) { transform.GetChild(0).GetComponent<TextMesh>().fontSize = 35; }
            else { transform.GetChild(0).GetComponent<TextMesh>().fontSize = 30; }
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
