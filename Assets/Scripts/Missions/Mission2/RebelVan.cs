using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RebelVan : MonoBehaviour
{
    GameObject followPlayer;
    public GameObject soldier;
    public GameObject spawn;
    public Animator soldierSpawning;
    public int maxSpawn = 8;
    private Health health;
    private float halfHealth;
    private Animator animator;
    private Rigidbody2D rb;
    private BlinkingSprite blinkingSprite;
    private float activationDistance;
    private float trigger = 3f;
    private bool hasHalfHealth = false;

    [Header("Time shoot")]
    private float shotTime = 0.0f;
    public float fireDelta = 4f;
    private float nextFire = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        followPlayer = GameManager.GetPlayer();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        blinkingSprite = GetComponent<BlinkingSprite>();
        registerHealth();
    }

    private void Update()
    {
        if (maxSpawn <= 0)
            return;

        float playerDistance = transform.position.x - followPlayer.transform.position.x;
        //Debug.Log(Mathf.Abs(playerDistance) + "" + trigger);
        if (playerDistance <= trigger)
        {
            shotTime = shotTime + Time.deltaTime;

            if (shotTime > nextFire)
            {
                nextFire = shotTime + fireDelta;

                StartCoroutine(Spawn());

                nextFire = nextFire - shotTime;
                shotTime = 0.0f;
            }
        }
    }


    private IEnumerator Spawn()
    {
        maxSpawn--;
        if (!hasHalfHealth)
        {
            animator.SetBool("isSpawning", true);
        }

        soldierSpawning.SetBool("isSpawning", true);
        yield return new WaitForSeconds(0.875f);
        soldierSpawning.SetBool("isSpawning", false);
        yield return new WaitForSeconds(0.25f);
        Instantiate(soldier, spawn.transform.position, spawn.transform.rotation);
        animator.SetBool("isSpawning", false);
    }

    private void registerHealth()
    {
        health = GetComponent<Health>();
        // register health delegate
        health.onDead += OnDead;
        health.onHit += OnHit;
    }

    private void OnHit(float damage)
    {
        if(health.GetHealth() < (health.GetMaxHealth() / 2f)){
            animator.SetBool("isDamaged", true);
            hasHalfHealth = true;
        }
        GameManager.AddScore(damage);
        blinkingSprite.Play();
    }

    private void OnDead(float damage)
    {
        StopAllCoroutines();
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        animator.SetBool("isExploding", true);
        if (rb)
            rb.isKinematic = true;
        if (GetComponent<BoxCollider2D>())
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (GetComponent<CapsuleCollider2D>())
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
        }

        yield return new WaitForSeconds(2.08f);
        Destroy(gameObject);
    }
}
