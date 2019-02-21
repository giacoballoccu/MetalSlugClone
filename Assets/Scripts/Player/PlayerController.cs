using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 1f;

    //Sprite orientation
    private bool facingRight = true;

    //Marco Controller
    private Animator mc;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mc = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(move * maxSpeed, rb.velocity.y);

        //The player is moving?
        if (move != 0)
        {
            //Yes
            mc.SetBool("isWalking", true);

            //Flip sprite orientantion if the user is walking right or left
            if (move > 0 && !facingRight)
            {
                Flip();
            } else if (move < 0 && facingRight)
            {
                Flip();
            }
        } else {
            //No
            mc.SetBool("isWalking", false);
        }
    }

    //Flip sprite
    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        facingRight = !facingRight;
    }
}
