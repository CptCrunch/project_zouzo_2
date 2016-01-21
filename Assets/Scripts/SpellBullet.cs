using UnityEngine;
using System.Collections;

public class SpellBullet : MonoBehaviour {

    [HideInInspector] public Attacks usedSpell;
    [HideInInspector] public Vector2 spellDir;
    [HideInInspector] public Vector2 startPosition;
    [HideInInspector] public GameObject caster;

    private GameObject[] targetsHit = new GameObject[3];

	void Start ()
    {
        // save start position
        startPosition = transform.position;
	}
	
	void Update ()
    {
        // destroy gameObject if it reached its max range
        if (Vector2.Distance(transform.position, startPosition) > usedSpell.Range && usedSpell.Range > 0)
        {
            Destroy(gameObject);
        }

        if (usedSpell != null)
        {
            // move bullet
            if (usedSpell.Range > 0) { transform.Translate(spellDir * usedSpell.ToTravelTime / usedSpell.Range * Time.deltaTime); }
            else { transform.Translate(spellDir * usedSpell.ToTravelTime * Time.deltaTime); }
        }

        TargetCollision();
    }

    void TargetCollision()
    {
        // draw a DebugRay
        Debug.DrawRay(transform.position, spellDir * (GetComponent<BoxCollider2D>().bounds.size.x / 2 + 0.2f), Color.blue);
        
        // create a raycast
        foreach (RaycastHit2D other in Physics2D.RaycastAll(transform.position, spellDir, GetComponent<BoxCollider2D>().bounds.size.x / 2 + 0.2f))
        {
            // get hited gameObject
            GameObject objectHit = other.transform.gameObject;
            
            // check if hited object is a player
            if (objectHit.tag == "Player")
            {
                // check if player is a enemy
                if (objectHit != caster)
                {
                    // check if enemy is registerd
                    if (!Util.IsGameObjectIncluded(targetsHit, objectHit))
                    {
                        // register enemy
                        Util.IncludeGameObject(targetsHit, objectHit);

                        // checks if the spell already hit its max of players
                        if (!usedSpell.MaxTargetsReached())
                        {
                            // add player hit
                            usedSpell.PlayersHit++;

                            // get player script
                            Player playerScript = objectHit.GetComponent<Player>();

                            // deal damage
                            playerScript.playerVitals.GetDamage(usedSpell.Damage);

                            // after max enemys reached
                            if (usedSpell.MaxTargetsReached()) { }
                        }
                    }
                }
            }

            // check if hitedobject is a Obstacle
            if (objectHit.tag == "Ground")
            {
                //CustomDebuger.Log("hit Obstacle");
                // check if ability is the saggitarus spell
                if (usedSpell.ID == 9)
                {
                    // remove spell bullet script
                    Destroy(this);
                }
            }
        }
    }
}
