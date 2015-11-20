using UnityEngine;
using System.Collections;

public class PlayerBasicAttack : MonoBehaviour {

    private BoxCollider2D collider;
    private bool attacking;

	void Awake () {
        collider = GetComponent<BoxCollider2D>();
	}
	
	void OnCollisionStay2D(Collision2D other)
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            other.gameObject.GetComponent<Player>().playerVitals.GetDamage(GetComponentInParent<Player>().basicAttackDamage);
            print("Attack");
        }
    }

    private IEnumerator Attack(float _timeBetweenAttacks)
    {
        attacking = true;
        yield return new WaitForSeconds(_timeBetweenAttacks);
        attacking = false;
    }
}
