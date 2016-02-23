using UnityEngine;
using System.Collections;

public class RayCollider : MonoBehaviour {

    private BoxCollider2D boxCollider;

    public bool topEnable = true;
    public bool bottomEnable = true;
    public bool rightEnable = true;
    public bool leftEnable = true;

    private bool collisionTop;
    private bool collisionBottom;
    private bool collisionRight;
    private bool collisionLeft;

    private GameObject collisionbjectTop;
    private GameObject collisionbjectBottom;
    private GameObject collisionbjectRight;
    private GameObject collisionbjectLeft;

    void Awake()
    {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

	void Start () {
	
	}
	
	void Update () {

        if (topEnable)
        {
            TopCollider();
        }

        if (bottomEnable)
        {
            BottomCollider();
        }

        if (rightEnable)
        {
            RightCollider();
        }

        if (leftEnable)
        {
            LeftCollider();
        }
    }

    private bool TopCollider()
    {
        // set origin
        Vector2 origin = new Vector2(boxCollider.bounds.center.x - boxCollider.bounds.size.x / 2, boxCollider.bounds.center.y + boxCollider.bounds.size.y / 2);
        // set lenght
        float rayLenght = boxCollider.bounds.size.x;

        // draw debug line/ray
        Debug.DrawRay(origin, Vector2.right * rayLenght, Color.red);

        // cast ray
        foreach (RaycastHit2D objectHit in Physics2D.RaycastAll(origin, Vector2.right, rayLenght))
        {
            return true;
        }

        return false;
    }

    private bool BottomCollider()
    {
        // set origin
        Vector2 origin = new Vector2(boxCollider.bounds.center.x - boxCollider.bounds.size.x / 2, boxCollider.bounds.center.y - boxCollider.bounds.size.y / 2);
        // set lenght
        float rayLenght = boxCollider.bounds.size.x;

        // draw debug line/ray
        Debug.DrawRay(origin, Vector2.right * boxCollider.bounds.size.x, Color.red);

        // cast ray
        foreach (RaycastHit2D objectHit in Physics2D.RaycastAll(origin, Vector2.right, rayLenght))
        {
            return true;
        }

        return false;
    }

    private bool RightCollider()
    {
        // set origin
        Vector2 origin = new Vector2(boxCollider.bounds.center.x + boxCollider.bounds.size.x / 2, boxCollider.bounds.center.y + boxCollider.bounds.size.y / 2);
        // set lenght
        float rayLenght = boxCollider.bounds.size.y;

        // draw debug line/ray
        Debug.DrawRay(origin, Vector2.down * rayLenght, Color.red);

        // cast ray
        foreach (RaycastHit2D objectHit in Physics2D.RaycastAll(origin, Vector2.down, rayLenght))
        {
            return true;
        }

        return false;
    }

    private bool LeftCollider()
    {
        // set origin
        Vector2 origin = new Vector2(boxCollider.bounds.center.x - boxCollider.bounds.size.x / 2, boxCollider.bounds.center.y + boxCollider.bounds.size.y / 2);
        // set lenght
        float rayLenght = boxCollider.bounds.size.y;

        // draw debug line/ray
        Debug.DrawRay(origin, Vector2.down * rayLenght, Color.red);

        // cast ray
        foreach (RaycastHit2D objectHit in Physics2D.RaycastAll(origin, Vector2.down, rayLenght))
        {
            return true;
        }

        return false;
    }
}
