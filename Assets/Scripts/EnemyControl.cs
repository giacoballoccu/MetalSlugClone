using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public Transform player;
    public float speed = 0.5f;
    public float health = 100f;

    public float activationDistance = 2;
    public float attackDistance = 0.5f;
    public const float CHANGE_SIGN = -1;

    private Rigidbody2D rb;
    private Animator ac;
    private bool facingRight = false;

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

        float playerDistance = transform.position.x - player.position.x;


        if (playerDistance < activationDistance)
        {
            if (Mathf.Abs(playerDistance) <= attackDistance)
            {
                //Attack player
                ac.SetBool("isAttacking", true);
                rb.isKinematic = true;
            }
            else
            {
                //Move to the player
                rb.isKinematic = false;
                rb.MovePosition(rb.position + new Vector2(CHANGE_SIGN * Mathf.Sign(playerDistance) * speed, 0) * Time.deltaTime);

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
}
