using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
public class PlayerController : MonoBehaviour {

    public float speed = 11f;
    public float jumpPower = 150f;
    public string playerAxis;
    private bool mirror = true;

    public bool grounded;

    private Rigidbody2D rb2d;

	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
	}

    float test;

	void FixedUpdate () {
        float move = Input.GetAxis(playerAxis + "_Horizontal");
        //if (Mathf.Abs(move) < 0.2)
        //{
        //    test = move;
        //    Debug.Log(move);
        //    move = 0;
        //}
        rb2d.velocity = new Vector2(move * speed, rb2d.velocity.y);

            if (move > 0 && !mirror)
            {
                Flip();
            }
            else
            {
                if (move < 0 && mirror)
                {
                    Flip();
                }
            }
        
	}

    void Flip()
    {
        mirror = !mirror;
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
