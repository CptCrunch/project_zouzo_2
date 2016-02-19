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
        // set sprit angle
        switch (Util.Aim8Direction(spellDir))
        {
            case "up":
                break;
            case "upRight":
                break;
            case "right":
                break;
            case "downRight":
                break;
            case "down":
                break;
            case "downLeft":
                break;
            case "left":
                break;
            case "upLeft":
                break;
            case "noAim":
                break;
        }

        // destroy gameObject if it reached its max range
        if (Vector2.Distance(transform.position, startPosition) > usedSpell.Range && usedSpell.Range > 0) { Destroy(gameObject); }

        // move bullet
        if (usedSpell != null) { transform.Translate(spellDir * usedSpell.BulletSpeed * Time.deltaTime); }

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

                            CustomDebug.Log("<b><color=white>" + usedSpell.Name + "</color></b> hit <b>" + playerScript.playerVitals.Name + "</b>", "Spells");

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
                // check if ability is the saggitarus spell
                if (usedSpell.ID == 9)
                {
                    Saggitarius saggitariusspell = (Saggitarius)usedSpell;

                    if (saggitariusspell.IsSticky)
                    {
                        bool isDirectional = false;
                        string direction = "";

                        // checks if arrow  is diagonal
                        if (spellDir.x != 0 && spellDir.y != 0)
                        {
                            direction = GetTipeOfHit(spellDir);
                        }

                        else
                        {
                            // checks if arrow is horizontal
                            if (Mathf.Abs(spellDir.x) == 1) { direction = "horizontal"; }

                            // arrow is vertical                                                 
                            else { direction = "vertical"; }
                        }

                        // set platform
                        if (direction == "horizontal")
                        {
                            gameObject.layer = LayerMask.NameToLayer("Obstacle");
                            gameObject.tag = "Through";
                        }

                        else
                        {
                            gameObject.layer = LayerMask.NameToLayer("Obstacle");
                            gameObject.tag = "Ground";
                        }

                        // remove spell bullet script
                        Destroy(this);
                    }

                    else { Destroy(gameObject); }
                }
            }
        }
    }

    private string GetTipeOfHit(Vector2 _spellDir)
    {
        // get origen
        Vector2 origen = new Vector2(transform.position.x + GetComponent<BoxCollider2D>().bounds.size.x / 2 * _spellDir.x,
                                     transform.position.y + GetComponent<BoxCollider2D>().bounds.size.y / 2 * _spellDir.y);

        // ceck if object hit a ceiling or floor
        foreach (RaycastHit2D hit in Physics2D.RaycastAll(origen, new Vector2(0, _spellDir.y), 0.04f))
        {
            GameObject objectHit = hit.transform.gameObject;

            if (objectHit != null && objectHit.tag == "Ground") { return "vertical"; }
        }

        // ceck if object hit a wall
        foreach (RaycastHit2D hit in Physics2D.RaycastAll(origen, new Vector2(_spellDir.x, 0), 0.04f))
        {
            GameObject objectHit = hit.transform.gameObject;

            if (objectHit != null && objectHit.tag == "Ground") { return "horizontal"; }
        }
        return "horizontal";
    }
}
