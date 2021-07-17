using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float rayLengthSide = 1f;
    [SerializeField]
    private float rayLengthDown = 0.54f;

    private bool facingRight = true;
    private bool green = false;
    //private bool red = false;

    private enum terrain
    {
        nothing,
        ground,
        slope,
        wall
    }


    private void Awake()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput < 0)
        {
            if (GroundCheck() == terrain.slope && !green) return;
            //if (rb2D.bodyType == RigidbodyType2D.Kinematic) rb2D.bodyType = RigidbodyType2D.Dynamic;
            if (facingRight) Flip();
            rb2D.velocity = new Vector2(-moveSpeed, rb2D.velocity.y);
        } else if (horizontalInput > 0)
        {
            if (GroundCheck() == terrain.slope && !green) return;
            //if (rb2D.bodyType == RigidbodyType2D.Kinematic) rb2D.bodyType = RigidbodyType2D.Dynamic;
            if (!facingRight) Flip();
            rb2D.velocity = new Vector2(moveSpeed, rb2D.velocity.y);
        } else
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
        }

        anim.SetFloat("Speed", Mathf.Abs(rb2D.velocity.x));

        // Debug rays
        Vector2 dir = facingRight ? Vector2.right : Vector2.left;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.down) * rayLengthDown, Color.red);
        Debug.DrawRay(transform.position, transform.TransformDirection(dir) * rayLengthSide, Color.red);


    }

    private void Flip()
    {
        facingRight = !facingRight;
        sprite.flipX = !sprite.flipX;
    }

    private terrain GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayLengthDown);
        if (hit.collider)
        {
            if (hit.collider.tag == "Ground")
            {
                return terrain.ground;
            }
            else if (hit.collider.tag == "Slope")
            {
                return terrain.slope;
            }
            else if (hit.collider.tag == "Wall")
            {
                return terrain.wall;
            }
        }
        return terrain.nothing;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Green")
        {
            green = true;
            anim.SetBool("Green", true);
            Destroy(collision.gameObject);
        }
    }

}
