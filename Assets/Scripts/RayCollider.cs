using UnityEngine;
using System.Collections;

public class RayCollider : MonoBehaviour
{
    public struct directions
    {
        public directionBool value;
        public directionGameObject gameObject;
    }

    public struct directionBool
    {
        public bool any;
        public bool top;
        public bool bottom;
        public bool right;
        public bool left;
    }

    public struct directionFloat
    {
        public float top;
        public float bottom;
        public float right;
        public float left;
    }

    public struct directionGameObject
    {
        public GameObject any;
        public GameObject top;
        public GameObject bottom;
        public GameObject right;
        public GameObject left;
    }

    private BoxCollider2D boxCollider;

    public directionBool enable;
    public directionFloat offset;
    [HideInInspector] public directions collision;

    [Header("Tag")]
    public string[] checkTag = new string[0];

    [Header("Enable Collision")]
    public bool enableTop = true;
    public bool enableBottom = true;
    public bool enableRight = true;
    public bool enableLeft = true;

    [Header("Offset")]
    public float offsetTop = 0.0f;
    public float offsetBottom = 0.0f;
    public float offsetRight = 0.0f;
    public float offsetLeft = 0.0f;

    [Header("ShowRay")]
    public bool showRays = false;
    public Color rayColor = Color.red;

    /*#region collisin bools
    #region variables
    private bool collision;
    private bool collisionTop;
    private bool collision.value.bottom;
    private bool collision.value.right;
    private bool collision.value.left;
    #endregion

    #region getter
    public bool Collision { get { return collision; } }
    public bool CollisionTop { get { return collisionTop; } }
    public bool collision.value.bottom { get { return collision.value.bottom; } }
    public bool collision.value.right { get { return collision.value.right; } }
    public bool collision.value.left { get { return collision.value.left; } }
    #endregion
    #endregion*/

    /*#region collision GameObjects
    #region variables
    private GameObject collisionObject;
    private GameObject collision.gameObject.top;
    private GameObject collision.gameObject.bottom;
    private GameObject collision.gameObject.right;
    private GameObject collision.gameObject.left;
    #endregion

    #region getter
    public GameObject CollisionObject { get { return collisionObject; } }
    public GameObject collision.gameObject.top { get { return collision.gameObject.top; } }
    public GameObject collision.gameObject.bottom { get { return collision.gameObject.bottom; } }
    public GameObject collision.gameObject.right { get { return collision.gameObject.right; } }
    public GameObject collision.gameObject.left { get { return collision.gameObject.left; } }
    #endregion
    #endregion*/

    void Awake()
    {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (enableTop)
        {
            collision.gameObject.top = TopCollider();
            collision.value.top = collision.gameObject.top != null;
        }

        if (enableBottom)
        {
            collision.gameObject.bottom = BottomCollider();
            collision.value.bottom = collision.gameObject.bottom != null;
        }

        if (enableRight)
        {
            collision.gameObject.right = RightCollider();
            collision.value.right = collision.gameObject.right != null;
        }

        if (enableLeft)
        {
            collision.gameObject.left = LeftCollider();
            collision.value.left = collision.gameObject.left != null;
        }

        if (collision.value.top || collision.value.bottom || collision.value.right || collision.value.left)
        {
            collision.value.any = true;

            if (collision.gameObject.top != null) { collision.gameObject.any = collision.gameObject.top; }
            else if (collision.gameObject.bottom != null) { collision.gameObject.any = collision.gameObject.bottom; }
            else if (collision.gameObject.right != null) { collision.gameObject.any = collision.gameObject.right; }
            else if (collision.gameObject.left != null) { collision.gameObject.any = collision.gameObject.left; }
        }

        else { collision.value.any = false; collision.gameObject.any = null; }

    }

    private GameObject TopCollider()
    {
        // set origin
        Vector2 origin = new Vector2(boxCollider.bounds.center.x - boxCollider.bounds.size.x / 2, boxCollider.bounds.center.y + boxCollider.bounds.size.y / 2 + offsetTop);
        // set lenght
        float rayLenght = boxCollider.bounds.size.x;

        // draw debug line/ray
        if (showRays) { Debug.DrawRay(origin, Vector2.right * rayLenght, rayColor); }

        // cast ray
        foreach (RaycastHit2D objectHit in Physics2D.RaycastAll(origin, Vector2.right, rayLenght))
        {
            // get GameObject form ray
            GameObject hit = objectHit.transform.gameObject;

            // check if variable is not empty
            if (hit != null)
            {
                // check if entry isn't you
                if (hit != gameObject)
                {
                    // check if we should check the tag
                    if (checkTag.Length > 0)
                    {
                        foreach (string tag in checkTag)
                        {
                            // check tag
                            if (hit.tag == tag) { return hit.gameObject; }
                        }
                    }

                    // return GameObject if we do not need to check the tag
                    else { return hit.gameObject; }
                }
            }
        }

        // return null if there is no collision
        return null;
    }

    private GameObject BottomCollider()
    {
        // set origin
        Vector2 origin = new Vector2(boxCollider.bounds.center.x - boxCollider.bounds.size.x / 2, boxCollider.bounds.center.y - boxCollider.bounds.size.y / 2 - offsetBottom);
        // set lenght
        float rayLenght = boxCollider.bounds.size.x;

        // draw debug line/ray
        if (showRays) { Debug.DrawRay(origin, Vector2.right * boxCollider.bounds.size.x, rayColor); }

        // cast ray
        foreach (RaycastHit2D objectHit in Physics2D.RaycastAll(origin, Vector2.right, rayLenght))
        {
            // get GameObject form ray
            GameObject hit = objectHit.transform.gameObject;

            // check if variable is not empty
            if (hit != null)
            {
                // check if entry isn't you
                if (hit != gameObject)
                {
                    // check if we should check the tag
                    if (checkTag.Length > 0)
                    {
                        foreach (string tag in checkTag)
                        {
                            // check tag
                            if (hit.tag == tag) { return hit.gameObject; }
                        }
                    }

                    // return GameObject if we do not need to check the tag
                    else { return hit.gameObject; }
                }
            }
        }

        // return null if there is no collision
        return null;
    }

    private GameObject RightCollider()
    {
        // set origin
        Vector2 origin = new Vector2(boxCollider.bounds.center.x + boxCollider.bounds.size.x / 2 + offsetRight, boxCollider.bounds.center.y + boxCollider.bounds.size.y / 2);
        // set lenght
        float rayLenght = boxCollider.bounds.size.y;

        // draw debug line/ray
        if (showRays) { Debug.DrawRay(origin, Vector2.down * rayLenght, rayColor); }

        // cast ray
        foreach (RaycastHit2D objectHit in Physics2D.RaycastAll(origin, Vector2.down, rayLenght))
        {
            // get GameObject form ray
            GameObject hit = objectHit.transform.gameObject;

            // check if variable is not empty
            if (hit != null)
            {
                // check if entry isn't you
                if (hit != gameObject)
                {
                    // check if we should check the tag
                    if (checkTag.Length > 0)
                    {
                        foreach (string tag in checkTag)
                        {
                            // check tag
                            if (hit.tag == tag) { return hit.gameObject; }
                        }
                    }

                    // return GameObject if we do not need to check the tag
                    else { return hit.gameObject; }
                }
            }
        }

        // return null if there is no collision
        return null;
    }

    private GameObject LeftCollider()
    {
        // set origin
        Vector2 origin = new Vector2(boxCollider.bounds.center.x - boxCollider.bounds.size.x / 2 - offsetLeft, boxCollider.bounds.center.y + boxCollider.bounds.size.y / 2);
        // set lenght
        float rayLenght = boxCollider.bounds.size.y;

        // draw debug line/ray
        if (showRays) { Debug.DrawRay(origin, Vector2.down * rayLenght, rayColor); }

        // cast ray
        foreach (RaycastHit2D objectHit in Physics2D.RaycastAll(origin, Vector2.down, rayLenght))
        {
            // get GameObject form ray
            GameObject hit = objectHit.transform.gameObject;

            // check if variable is not empty
            if (hit != null)
            {
                // check if entry isn't you
                if (hit != gameObject)
                {
                    // check if we should check the tag
                    if (checkTag.Length > 0)
                    {
                        foreach (string tag in checkTag)
                        {
                            // check tag
                            if (hit.tag == tag) { return hit.gameObject; }
                        }
                    }

                    // return GameObject if we do not need to check the tag
                    else { return hit.gameObject; }
                }
            }
        }

        // return null if there is no collision
        return null;
    }
}
