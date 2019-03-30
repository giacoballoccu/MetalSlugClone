using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanBombMovement : MonoBehaviour
{

    private Rigidbody2D rb;

    private float damageBomb = 100;
    public Animator vanBombAnimator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(Explosion(collision));
    }

    private IEnumerator Explosion(Collider2D collision)
    {
        if (rb != null)
        {
            if (GameManager.CanTriggerThrowable(collision.tag))
            {
                vanBombAnimator.SetBool("hasHittenSth", true);
                {
                    if (collision.CompareTag("Player"))
                    {
                        collision.GetComponent<Health>().Hit(damageBomb);
                    }
                }
                Destroy(rb);
                yield return new WaitForSeconds(1.7f);
                vanBombAnimator.SetBool("hasHittenSth", false);
                Destroy(gameObject);
            }
        }
    }
}
