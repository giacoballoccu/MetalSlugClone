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

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        StartCoroutine(Explosion(collider));
    }

    private IEnumerator Explosion(Collider2D collider)
    {
        if (rb != null)
        {
            if (GameManager.IsPlayer(collider) || collider.tag == "Marco Boat" || collider.tag == "Granate" || collider.tag == "Bullet")
            {
                vanBombAnimator.SetBool("hasHittenSth", true);
                {
                    if (GameManager.IsPlayer(collider))
                    {
                        GameManager.GetPlayer().GetComponent<Health>().Hit(damageBomb);
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
