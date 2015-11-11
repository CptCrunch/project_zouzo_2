using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JumpPad : MonoBehaviour {

    public float strength;
    private bool rayHit;
    private RaycastHit2D objectHit;
	
	// Update is called once per frame
    void Update() {

        RaycastHit2D[] rays = new RaycastHit2D[3]{
            Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + GetComponent<BoxCollider2D>().bounds.size.y / 2 + 0.01f), Vector2.up, 0.01f),
            Physics2D.Raycast(new Vector2(transform.position.x + GetComponent<BoxCollider2D>().bounds.size.x / 2, transform.position.y + GetComponent<BoxCollider2D>().bounds.size.y / 2 + 0.01f), Vector2.up, 0.01f),
            Physics2D.Raycast(new Vector2(transform.position.x - GetComponent<BoxCollider2D>().bounds.size.x / 2, transform.position.y + GetComponent<BoxCollider2D>().bounds.size.y / 2 + 0.01f), Vector2.up, 0.01f)
        };

        rayHit = false;

        foreach (RaycastHit2D hit in rays) {
            if (hit.collider != null) {
                Debug.DrawLine(transform.position, hit.point, Color.blue);

                if (hit.collider.gameObject.tag == "Player") {
                    objectHit = hit;
                    rayHit = true;
                }
            }
        }

        if (rayHit) { objectHit.collider.gameObject.GetComponent<Player>().velocity.y = strength; }
    }
}