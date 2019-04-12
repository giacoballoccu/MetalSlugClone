using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour
{
    private Animator animator;
    private Collider2D colllider;
    private bool hitten = false;
    public float damage = 50f;
    private float animationTime;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().onHit(damage);
        }

        if (collision.CompareTag("Walkable"))
        {
            StartCoroutine(rockFalling());
        }
    }

    private IEnumerator rockFalling()
    {
        animator.SetBool("isGrounded", true);
        yield return new WaitForSeconds(3.2f);
        animator.SetBool("isGrounded", false);
        Destroy(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Bullet") || collision.collider.CompareTag("Granate"))
        {
            StartCoroutine(rockHitten());
        }
    }

    private IEnumerator rockHitten()
    {
        animator.SetBool("isHitten", true);
        yield return new WaitForSeconds(0.6f);
        animator.SetBool("isHitten", false);
        Destroy(this);
    }
}
