using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliController : MonoBehaviour
{
    public float speed = 0.5f;
    public bool facingRight = false;

    Rigidbody2D rb;

    private int changeDirectionX = -1;
    private int changeDirectionY = 0;
    private bool flipped = false;

    private float height;
    private Animator animator;
    private BlinkingSprite blinkingSprite;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        blinkingSprite = GetComponent<BlinkingSprite>();

        height = rb.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);

        //Vector3 speed = rb.velocity;
        if ((pos.x == 0 && !facingRight) || (pos.x == 1 && facingRight))
        {
            if (!flipped)
            {
                changeDirectionX *= -1;
                Flip();
                flipped = true;
            }
        } 
        else
        {
            if (flipped && rb.position.y < height + 0.30f && facingRight)
            {
                changeDirectionY = 1;   
            }else if (flipped && rb.position.y > height && !facingRight)
            {
                changeDirectionY = -1;
            }
            else
            {
                flipped = false;
                changeDirectionY = 0;
            }
            
            rb.MovePosition(rb.position + new Vector2(changeDirectionX * speed, changeDirectionY * speed) * Time.deltaTime);
            
        }

        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        //transform.localEulerAngles = transform.eulerAngles + new Vector3(0, 180, -2 * transform.eulerAngles.z);
        facingRight = !facingRight;
    }

    private void OnHit(float damage)
    {
        animator.SetTrigger("isHitten");

        GameManager.AddScore(damage);
        blinkingSprite.Play();
    }
}
