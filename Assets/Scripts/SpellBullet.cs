using UnityEngine;
using System.Collections;

public class SpellBullet : MonoBehaviour {

    public Attacks usedSpell;
    public Vector2 spellDir;
    public Vector2 startPosition;
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
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // check if hited object is a player
        if (other.gameObject.tag == "Player")
        {
            // check if player is a enemy
            if (other.gameObject != gameObject)
            {
                // check if enemy is registerd
                if (targetsHit != Util.IsGameObjectIncluded(targetsHit, other.gameObject))
                {
                    // register enemy
                    targetsHit = Util.IsGameObjectIncluded(targetsHit, other.gameObject);

                    // checks if the spell already hit its max of players
                    if (usedSpell.MaxTargetsReached())
                    {
                        // get player script
                        Player playerScript = other.gameObject.GetComponent<Player>();

                        // deal damage
                        playerScript.playerVitals.GetDamage(usedSpell.Damage);
                    }
                }
            }
        }
    }
}
