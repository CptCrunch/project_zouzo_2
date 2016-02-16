using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour {

    public int damage;
    public float invulnTime;

    [Tooltip("accepted parameters: right, left, top, bottom")]
    public string colliderDirction;

    void Start() { if (colliderDirction != "right" && colliderDirction != "left" && colliderDirction != "top" && colliderDirction != "bottom") { Debug.LogError("Wrong colliderDirection parameter"); } }

    void Update()
    {
        Vector3 rayOrigin = new Vector3(0,0,0);
        Vector3 rayDirection = new Vector3(0, 0, 0);
        float rayRange = 0;

        // get object bounds
        Bounds objectBounds = gameObject.GetComponent<BoxCollider2D>().bounds;

        // set ray origin, direction, range
        switch (colliderDirction)
        {
            case "right":
                rayOrigin = objectBounds.center + new Vector3(objectBounds.size.x / 2, objectBounds.size.y / 2, 0);
                rayDirection = Vector2.down;
                rayRange = objectBounds.size.y;
                break;

            case "left":
                rayOrigin = objectBounds.center + new Vector3(-objectBounds.size.x / 2, objectBounds.size.y / 2, 0);
                rayDirection = Vector2.down;
                rayRange = objectBounds.size.y;
                break;

            case "top":
                rayOrigin = objectBounds.center + new Vector3(-objectBounds.size.x / 2, objectBounds.size.y / 2, 0);
                rayDirection = Vector2.right;
                rayRange = objectBounds.size.x;
                break;

            case "bottom":
                rayOrigin = objectBounds.center + new Vector3(-objectBounds.size.x / 2, -objectBounds.size.y / 2, 0);
                rayDirection = Vector2.right;
                rayRange = objectBounds.size.x;
                break;
        }

        // create DebugRay
        CustomDebug.DrawLine(rayOrigin, rayOrigin + rayDirection * rayRange, Color.cyan, "MapFeature");

        // create a raycast
        foreach (RaycastHit2D objectHit in Physics2D.RaycastAll(rayOrigin, rayDirection, rayRange))
        {
            // check if object is a Player
            if (objectHit.transform.gameObject.tag == "Player")
            {
                // get player vitals
                LivingEntity vitals = objectHit.transform.gameObject.GetComponent<Player>().playerVitals;

                // check if invulnerable
                if (!vitals.Invuln)
                {
                    // deal damage
                    vitals.GetDamage(damage);

                    // enable invulnerability
                    vitals.Invuln = true;

                    // call disableInvuln
                    StartCoroutine(disableInvuln(vitals));
                }
            }
        }
    }

    /// <summary>Disables invulnerability after parameter seconds</summary>
    /// <param name="_vitals"></param>
    /// <returns></returns>
    IEnumerator disableInvuln(LivingEntity _vitals)
    {
        // wait parameter seconds
        yield return new WaitForSeconds(invulnTime);

        // disable invulnerability
        _vitals.Invuln = false;
        
    }

}
