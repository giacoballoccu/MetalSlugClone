using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 1f;

    //Sprite orientation
    private bool facingRight = true;

    //Marco Controller
    public Animator topAnimator;
    public Animator bottomAnimator;

    private Rigidbody2D rb;

    // Time shoot
    private float myTime = 0.0f;
    public float fireDelta = 0.5F;
    private float nextFire = 0.5F;

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

        myTime = myTime + Time.deltaTime;

        if (Input.GetButton("Fire1"))
        {
            topAnimator.SetBool("isFiring", true);

            if (myTime > nextFire)
            {
                nextFire = myTime + fireDelta;

                Instantiate(projectile, projSpawner.transform.position, projSpawner.transform.rotation);

                nextFire = nextFire - myTime;
                myTime = 0.0F;
            }
        } else
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
            } else if (moveH < 0 && facingRight)
            {
                //Moving left
                Flip();
            }
        } else {
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
                FlipShoot();
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
                FlipShoot();
            }
        }
    }

    //Flip sprite
    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        //transform.localEulerAngles = transform.eulerAngles + new Vector3(0, 180, -2 * transform.eulerAngles.z);
        facingRight = !facingRight;

        FlipShoot();
    }

    void FlipShoot()
    {
        if (topAnimator.GetBool("isLookingUp") && facingRight)
        {
            projSpawner.transform.localEulerAngles = new Vector3(0, 0, 90);
        } else if (topAnimator.GetBool("isLookingUp") && !facingRight) {
            projSpawner.transform.localEulerAngles = new Vector3(0, 0, 270);
        } else if (facingRight)
        {
            projSpawner.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            projSpawner.transform.localEulerAngles = new Vector3(0, 0, 180);
        }
    }
}
