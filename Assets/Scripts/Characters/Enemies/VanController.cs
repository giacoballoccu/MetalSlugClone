using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanController : MonoBehaviour
{
    GameObject followPlayer;
    private Health health;
    private BlinkingSprite blinkingSprite;

    [Header("Bridge activation")]
    public BridgeController ownBridge;

    [Header("Enemy activation")]
    public float activationDistance = 1.8f;
    private Rigidbody2D rb;
    private Animator animator;

    [Header("Time shoot")]
    private float shotTime = 0.0f;
    public float fireDelta = 3f;
    private float nextFire = 3f;

    [Header("Bomb")]
    public GameObject bomb;
    public Transform bombSpawner;
    private Vector3 newSpawn;
    private Random random = new Random();



    void Start()
    {
        followPlayer = GameManager.GetPlayer();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        blinkingSprite = GetComponent<BlinkingSprite>();
        registerHealth();
    }

    void Update()
    {
        if (GameManager.IsGameOver())
            return;
    }

    private void FixedUpdate()
    {
        if (GameManager.IsGameOver())
            return;

        if (health.IsAlive())
        {
            float playerDistance = transform.position.x - followPlayer.transform.position.x;
            if (playerDistance < activationDistance)
            {
                animator.SetBool("isFiring", true);
                shotTime = shotTime + Time.deltaTime;
                if (shotTime > nextFire)
                {
                    nextFire = shotTime + fireDelta;

                    StartCoroutine(SpawnBombs());
                    nextFire = nextFire - shotTime;
                    shotTime = 0.0f;
                }
                else
                {
                    animator.SetBool("isFiring", false);
                }


            }
            else
            {
                animator.SetBool("isFiring", false);
            }
        }
    }

    private void registerHealth()
    {
        health = GetComponent<Health>();
        // register health delegate
        health.onDead += OnDead;
        health.onHit += OnHit;
    }

    private void OnDead(float damage)
    {
        StartCoroutine(Die());
    }

    private void OnHit(float damage)
    {
      
        GameManager.AddScore(damage);
        blinkingSprite.Play();
    }

    private IEnumerator Die()
    {
        //PlayDeathAudio();
        animator.SetBool("isDying", true);
        if (rb)
            rb.isKinematic = true;
        GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(0.2f);
        ownBridge.SetBridgeDestroyed();
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }


        private IEnumerator SpawnBombs()
    {
        yield return new WaitForSeconds(0.12f);
        for (int i=0; i<4; i++)
        {
            newSpawn = new Vector3(Random.Range(bombSpawner.position.x-1, bombSpawner.position.x+1), bombSpawner.position.y, bombSpawner.position.z);
            Instantiate(bomb, newSpawn, bombSpawner.transform.rotation);
            yield return new WaitForSeconds(0.3f);
        }
       
    }
}
