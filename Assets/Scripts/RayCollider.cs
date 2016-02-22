using UnityEngine;
using System.Collections;

public class RayCollider : MonoBehaviour {

    private BoxCollider2D boxCollider;

    void Awake()
    {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        TopCollider();
        BottomCollider();
        RightCollider();
        LeftCollider();
    }

    private bool TopCollider()
    {
        Vector2 origin = new Vector2(boxCollider.bounds.center.x - boxCollider.bounds.size.x / 2, boxCollider.bounds.center.y + boxCollider.bounds.size.y / 2);

        Debug.DrawRay(origin, Vector2.right * boxCollider.bounds.size.x, Color.red);
        return true;
    }

    private bool BottomCollider()
    {
        Vector2 origin = new Vector2(boxCollider.bounds.center.x - boxCollider.bounds.size.x / 2, boxCollider.bounds.center.y - boxCollider.bounds.size.y / 2);

        Debug.DrawRay(origin, Vector2.right * boxCollider.bounds.size.x, Color.red);
        return true;
    }

    private bool RightCollider()
    {
        Vector2 origin = new Vector2(boxCollider.bounds.center.x + boxCollider.bounds.size.x / 2, boxCollider.bounds.center.y + boxCollider.bounds.size.y / 2);

        Debug.DrawRay(origin, Vector2.down * boxCollider.bounds.size.y, Color.red);
        return true;
    }

    private bool LeftCollider()
    {
        Vector2 origin = new Vector2(boxCollider.bounds.center.x - boxCollider.bounds.size.x / 2, boxCollider.bounds.center.y + boxCollider.bounds.size.y / 2);

        Debug.DrawRay(origin, Vector2.down * boxCollider.bounds.size.y, Color.red);
        return true;
    }
}
