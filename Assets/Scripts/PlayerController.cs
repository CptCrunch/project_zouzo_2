using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
public class PlayerController : MonoBehaviour {

    public float speed = 11f;
    public float jumpPower = 150f;
    public string playerAxis;
    private bool mirror = true;

    private bool grounded;

    private Rigidbody2D rb2d;

	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate () {
        Movement();
    }

    void Movement()
    {
        float move = Input.GetAxis(playerAxis + "_Horizontal");
        rb2d.velocity = new Vector2(move * speed, rb2d.velocity.y);

        if (Input.GetButtonDown(playerAxis + "_Jump") && grounded) { 
            rb2d.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
        }

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

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.transform.tag);
        if(other.transform.tag == "Ground")
        {
            grounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.tag == "Ground")
        {
            grounded = false;
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
