using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour
{
    private Animator animator;
    private Collider2D colllider;
    private float damage = 15f;
    private float animationTime;
    private bool isFallingOnGround;
    private int bulletHits = 5;
    public BoxCollider2D bodyCollider;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (isFallingOnGround)
        {
            //bodyCollider.gameObject.transform.Translate(Vector3.down * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!isFallingOnGround) // skip if not on ground yet
            return;

        GetComponent<BlinkingSprite>().Play();
        if (collider.CompareTag("Bullet"))
            bulletHits--;
        else if (collider.CompareTag("Granate"))
            bulletHits = 0;

        if (bulletHits <= 0)
        {
            StartCoroutine(rockHitten());
        }
    }

    private IEnumerator rockFalling()
    {
        animator.SetBool("isGrounded", true);
        isFallingOnGround = true;
        GetComponent<Rigidbody2D>().isKinematic = true;
        yield return new WaitForSeconds(3.2f);
        animator.SetBool("isGrounded", false);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isFallingOnGround)
            return;

        if (collision.collider.CompareTag("Walkable"))
        {
            StartCoroutine(rockFalling());
        }
        else if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().Hit(damage);
            StartCoroutine(rockHitten());
        }
    }

    private IEnumerator rockHitten()
    {
        //animator.SetBool("isHitten", true);
        GetComponent<Rigidbody2D>().isKinematic = true;
        bodyCollider.isTrigger = true;
        //yield return new WaitForSeconds(0.6f);
        yield return new WaitForSeconds(0.2f);
        //animator.SetBool("isHitten", false);
        Destroy(gameObject);
    }
}
