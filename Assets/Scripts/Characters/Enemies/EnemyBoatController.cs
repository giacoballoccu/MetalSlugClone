using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoatController : MonoBehaviour
{
    public GameObject front;
    public GameObject center;
    public GameObject back;
    public GameObject explosion;

    public Sprite sinked;

    private Health health;
    private Animator animator;
    private BlinkingSprite blinkingSprite;
    private SpriteRenderer sr;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        blinkingSprite = GetComponent<BlinkingSprite>();
        sr = GetComponent<SpriteRenderer>();

        registerHealth();
    }

    private void Update()
    {
        if (health.GetHealth() >= 1000)
        {
            //Nothing
        }
        else if (health.GetHealth() >= 750)
        {
            front.SetActive(true);
            AudioManager.PlayMetalSlugDestroy3();
        }
        else if (health.GetHealth() >= 500)
        {
            center.SetActive(true);
            AudioManager.PlayMetalSlugDestroy1();
        }
        else if (health.GetHealth() >= 250)
        {
            back.SetActive(true);
            AudioManager.PlayMetalSlugDestroy1();
        }
        else if (!health.IsAlive())
        {
            front.SetActive(false);
            center.SetActive(false);
            back.SetActive(false);

            explosion.SetActive(true);
            explosion.GetComponent<Animator>().SetBool("isDying", true);
            AudioManager.PlayMetalSlugDestroy2();
        }
    }

    private void registerHealth()
    {
        health = GetComponent<Health>();
        // register health delegate
        health.onDead += OnDead;
    }

    private void OnDead(float damage)
    {
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        sr.sprite = sinked;

        GetComponent<PolygonCollider2D>().enabled = false;

        if (rb)
            rb.isKinematic = true;

        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
