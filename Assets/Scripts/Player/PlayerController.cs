using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 1f;
    public float maxJump = 1f;
    private bool isGrounded = false;

    //Sprite orientation
    private bool facingRight = true;

    //Marco Controller
    public Animator topAnimator;
    public Animator bottomAnimator;

    private Rigidbody2D rb;

    // Time shoot
    private float shotTime = 0.0f;
    public float fireDelta = 0.5f;
    private float nextFire = 0.5f;

    //Bullet
    public GameObject projectile;

    //Bullet spawner
    public GameObject projSpawner;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        shotTime = shotTime + Time.deltaTime;

        if (Input.GetButton("Fire1"))
        {
            topAnimator.SetBool("isFiring", true);

            if (shotTime > nextFire)
            {
                nextFire = shotTime + fireDelta;

                Instantiate(projectile, projSpawner.transform.position, projSpawner.transform.rotation);

                nextFire = nextFire - shotTime;
                shotTime = 0.0f;
            }
        }
        else
        {
            topAnimator.SetBool("isFiring", false);
        }
    }

    void FixedUpdate()
    {
        float moveH = Input.GetAxis("Horizontal");
        float moveV = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(moveH * maxSpeed, rb.velocity.y);

        //The player is moving horizontally?
        if (moveH != 0)
        {
            //Yes
            topAnimator.SetBool("isWalking", true);
            bottomAnimator.SetBool("isWalking", true);

            //Flip sprite orientantion if the user is walking right or left
            if (moveH > 0 && !facingRight)
            {
                //Moving right
                Flip();
            }
            else if (moveH < 0 && facingRight)
            {
                //Moving left
                Flip();
            }
        }
        else
        {
            //No
            topAnimator.SetBool("isWalking", false);
            bottomAnimator.SetBool("isWalking", false);
        }

        //The player is moving vertically?
        if (moveV != 0)
        {
            //Yes
            
            //bottomAnimator.SetBool("isWalking", true);

            //Flip sprite orientantion if the user is walking right or left
            if (moveV > 0)
            {
                //Moving UP
                topAnimator.SetBool("isLookingUp", true);
            }
            else if (moveH < 0)
            {
                //Moving down
            }
        }
        else
        {
            //No
            if (topAnimator.GetBool("isLookingUp"))
            {
                topAnimator.SetBool("isLookingUp", false);
            }
        }

        //Jump
        if (Input.GetButton("Jump") && isGrounded)
        {
            rb.AddForce(new Vector3(0, maxJump, 0), ForceMode2D.Impulse);
            topAnimator.SetBool("isJumping", true);
            bottomAnimator.SetBool("isJumping", true);
            isGrounded = false;
        }

        //Crouch
        if (Input.GetButton("Crouch"))
        {
            topAnimator.SetBool("isCrouched", true);
        }
        else
        {
            topAnimator.SetBool("isCrouched", false);
        }

        FlipShoot();
    }

    //Flip sprite
    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        //transform.localEulerAngles = transform.eulerAngles + new Vector3(0, 180, -2 * transform.eulerAngles.z);
        facingRight = !facingRight;
    }

    void FlipShoot()
    {
        if (topAnimator.GetBool("isLookingUp") && facingRight)
        {
            //Fire up
            projSpawner.transform.localEulerAngles = new Vector3(0, 0, 90);
        } else if (topAnimator.GetBool("isLookingUp") && !facingRight) {
            //Fire up
            projSpawner.transform.localEulerAngles = new Vector3(0, 0, 270);
        } else if (topAnimator.GetBool("isCrouched") && !isGrounded && facingRight)
        {
            //Fire down
            projSpawner.transform.localEulerAngles = new Vector3(0, 0, 270);
        }
        else if (topAnimator.GetBool("isCrouched") && !isGrounded && !facingRight)
        {
            //Fire down
            projSpawner.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
        else if (facingRight)
        {
            //Fire right
            projSpawner.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            //Fire left
            projSpawner.transform.localEulerAngles = new Vector3(0, 0, 180);
        }
    }

    void OnCollisionStay2D()
    {
        isGrounded = true;
        topAnimator.SetBool("isJumping", false);
        bottomAnimator.SetBool("isJumping", false);
    }
}
