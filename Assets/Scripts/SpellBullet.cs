using UnityEngine;
using System.Collections;

public class SpellBullet : MonoBehaviour {

    [HideInInspector] public Attacks usedSpell;
    [HideInInspector] public Vector2 spellDir;
    [HideInInspector] public Vector2 startPosition;
    [HideInInspector] public GameObject caster;
    public Sprite[] bulletSprites = new Sprite[2];

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D collider;

    private float colliderSiteX;
    private float colliderSiteY;
    private float colliderBiggestSide;

    private GameObject[] targetsHit = new GameObject[3];

	void Start ()
    {
        // save start position
        startPosition = transform.position;

        // get spriteRenderer
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        // get collider
        collider = gameObject.GetComponent<BoxCollider2D>();

        // save collider sice
        colliderSiteX = collider.size.x;
        colliderSiteY = collider.size.y;

        if (colliderSiteX < colliderSiteY) { colliderBiggestSide = colliderSiteY; }
        else { colliderBiggestSide = colliderSiteX; }

        // set sprit angle
        switch (Util.Aim8Direction(new Vector2(spellDir.y, spellDir.x)))
        {
            case "up":
                collider.size = new Vector2(colliderSiteY, colliderSiteX);
                spriteRenderer.sprite = bulletSprites[1];
                break;
            case "upRight":
                collider.size = new Vector2(colliderBiggestSide, colliderBiggestSide);
                spriteRenderer.sprite = bulletSprites[2];
                spriteRenderer.flipX = true;
                break;
            case "right":
                spriteRenderer.flipX = true;
                break;
            case "downRight":
                collider.size = new Vector2(colliderBiggestSide, colliderBiggestSide);
                spriteRenderer.sprite = bulletSprites[2];
                spriteRenderer.flipX = true;
                spriteRenderer.flipY = true;
                break;
            case "down":
                collider.size = new Vector2(colliderSiteY, colliderSiteX);
                spriteRenderer.sprite = bulletSprites[1];
                spriteRenderer.flipY = true;
                break;
            case "downLeft":
                collider.size = new Vector2(colliderBiggestSide, colliderBiggestSide);
                spriteRenderer.sprite = bulletSprites[2];
                spriteRenderer.flipY = true;
                break;
            case "left":
                break;
            case "upLeft":
                collider.size = new Vector2(colliderBiggestSide, colliderBiggestSide);
                spriteRenderer.sprite = bulletSprites[2];
                break;
            case "noAim":
                break;
        }
    }
	
	void Update ()
    {
        // destroy gameObject if it reached its max range
        if (Vector2.Distance(transform.position, startPosition) > usedSpell.Range && usedSpell.Range > 0) { Destroy(gameObject); }

        // move bullet
        if (usedSpell != null) { transform.Translate(spellDir * usedSpell.BulletSpeed * Time.deltaTime); }

        TargetCollision();
    }

    void TargetCollision()
    {
        // draw a DebugRay
        Debug.DrawRay(transform.position, spellDir * (GetComponent<BoxCollider2D>().bounds.size.x + 0.2f), Color.blue);
        
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
                            direction = GetTipeOfHit();

                            // check if the arrow shoud flip vertical
                            if (direction == "vertical")
                            {
                                collider.size = new Vector2(colliderSiteY, colliderSiteX);
                                spriteRenderer.sprite = bulletSprites[1];
                                spriteRenderer.flipX = false;

                                if (Util.Aim8Direction(new Vector2(spellDir.y, spellDir.x)) == "upRight" || Util.Aim8Direction(new Vector2(spellDir.y, spellDir.x)) == "upLeft")
                                {
                                    spriteRenderer.flipY = false;
                                }

                                else
                                {
                                    spriteRenderer.flipY = true;
                                }
                            }

                            else
                            {
                                collider.size = new Vector2(colliderSiteX, colliderSiteY);
                                spriteRenderer.sprite = bulletSprites[0];
                                spriteRenderer.flipY = false;

                                if (Util.Aim8Direction(new Vector2(spellDir.y, spellDir.x)) == "upRight" || Util.Aim8Direction(new Vector2(spellDir.y, spellDir.x)) == "downRight")
                                {
                                    spriteRenderer.flipX = true;
                                }

                                else
                                {
                                    spriteRenderer.flipX = false;
                                }
                            }
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

    private string GetTipeOfHit()
    {
        Bounds colliderBounds = GetComponent<BoxCollider2D>().bounds;

        // get collider origen
        Vector2 origen = new Vector2(colliderBounds.center.x + (colliderBounds.size.x - 0.3f) / 2 * spellDir.x,
                                     colliderBounds.center.y + (colliderBounds.size.y - 0.3f) / 2 * spellDir.y);

        // make a debu ray for the horizontal axis
        CustomDebug.DrawLine(origen, origen + new Vector2(0, spellDir.y), Color.red, "Spells");

        // ceck if object hit a ceiling or floor
        foreach (RaycastHit2D hit in Physics2D.RaycastAll(origen, new Vector2(0, spellDir.y), 0.5f))
        {
            GameObject objectHit = hit.transform.gameObject;
            
            if (objectHit != null && objectHit.tag == "Ground") { Debug.Log(objectHit); return "vertical"; }
        }

        // make a debu ray for the vertical axis
        CustomDebug.DrawLine(origen, origen + new Vector2(spellDir.x, 0), Color.red, "Spells");

        // ceck if object hit a wall
        foreach (RaycastHit2D hit in Physics2D.RaycastAll(origen, new Vector2(spellDir.x, 0), 0.5f))
        {
            GameObject objectHit = hit.transform.gameObject;

            if (objectHit != null && objectHit.tag == "Ground") { return "horizontal"; }
        }

        return "no hit";
    }
}
