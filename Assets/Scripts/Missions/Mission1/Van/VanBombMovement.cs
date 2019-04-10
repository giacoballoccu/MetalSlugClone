using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanBombMovement : MonoBehaviour
{

    private Rigidbody2D rb;

    public float damageBomb = 15;
    public Animator vanBombAnimator;
    public float lifeTime = 5;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(Explosion(collision));
    }

    private IEnumerator Explosion(Collider2D collision)
    {
        if (rb != null)
        {
            if (collision.tag == "Player" || collision.tag == "Marco Boat" || collision.tag == "Granate" || collision.tag == "Bullet")
            {
                vanBombAnimator.SetBool("hasHittenSth", true);
                {
                    if (collision.CompareTag("Player"))
                    {
                        collision.GetComponent<Health>().Hit(damageBomb);
                    }
                }
               this.enabled = false;
                Destroy(rb);
                yield return new WaitForSeconds(1.3f);
                vanBombAnimator.SetBool("hasHittenSth", false);
                Destroy(gameObject);
            }
        }
    }
}
