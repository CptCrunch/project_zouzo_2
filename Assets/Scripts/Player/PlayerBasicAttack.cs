using UnityEngine;
using System.Collections;

public class PlayerBasicAttack : MonoBehaviour {

    private BoxCollider2D collider;

	void Awake () {
        collider = GetComponent<BoxCollider2D>();
	}
	
	void OnTriggerEnter2D(Collider2D other)
    {

    }

    public void Attack()
    {
        
    }
}
