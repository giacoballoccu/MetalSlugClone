using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    //Enemy information
    public GameObject player;
    public float speed = 0.5f;
    public float health = 100f;

    //Enemy activation
    public float activationDistance = 2;
    public float attackDistance = 0.5f;
    public const float CHANGE_SIGN = -1;

    private Rigidbody2D rb;
    private Animator ac;
    private bool facingRight = false;

    //Enemy gravity
    private bool collidingDown = false;
    Vector2 velocity = Vector2.zero;

    // Time shoot
    private float shotTime = 0.0f;
    public float fireDelta = 0.5f;
    private float nextFire = 0.5f;

    private void Start()
    {
        ac = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (health <= 0)
        {
            ac.SetBool("isDying", true);
            StartCoroutine(Die());
        }
    }

    void FixedUpdate()
    {
        //transform.Rotate(new Vector3(0, -90, 0), Space.Self);//correcting the original rotation

        float playerDistance = transform.position.x - player.transform.position.x;

        if (health <= 0)
        {
            return;
        }


        if (playerDistance < activationDistance)
        {
            if (Mathf.Abs(playerDistance) <= attackDistance)
            {
                //Attack player
                ac.SetBool("isAttacking", true);
                rb.isKinematic = true;


                shotTime = shotTime + Time.deltaTime;

                if (shotTime > nextFire)
                {
                    nextFire = shotTime + fireDelta;


                    player.GetComponent<PlayerController>().Hit(100f);

                    nextFire = nextFire - shotTime;
                    shotTime = 0.0f;
                }
            }
            else
            {
                //Move to the player
                rb.isKinematic = false;

                if (collidingDown)
                {
                    rb.MovePosition(rb.position + new Vector2(CHANGE_SIGN * Mathf.Sign(playerDistance) * speed, rb.position.y) * Time.deltaTime);
                }
                else
                {
                    //velocity.y -= 9.81f * Time.deltaTime;
                    //rb.MovePosition(new Vector2(transform.position.x, velocity.y));
                    rb.MovePosition(rb.position + new Vector2(CHANGE_SIGN * Mathf.Sign(playerDistance) * speed, rb.position.y - 0.1f) * Time.deltaTime);
                }

                ac.SetBool("isWalking", true);
                ac.SetBool("isAttacking", false);
            }
        }

        //Flip enemy
        if (playerDistance < 0 && !facingRight)
        {
            Flip();
        } else if (playerDistance > 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        facingRight = !facingRight;
    }

    public void hit()
    {
        ac.SetTrigger("isHitten");
        health -= 33.33f; 
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Walkable")
        {
            collidingDown = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Walkable")
        {
            collidingDown = false;
        }
    }
}
