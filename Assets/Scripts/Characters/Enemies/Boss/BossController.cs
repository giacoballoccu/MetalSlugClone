using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{

    [Header("Enemy information")]
    GameObject followPlayer;
    public float speed = 0.5f;
    public float attackDamage = 10f;
    public bool isMovable = true;
    public bool canMelee = true;
    public AudioClip deathClip;
    private Health health;
    private float maxHealth = 10000;
    private BlinkingSprite blinkingSprite;
    public GameObject projSpawner;
    private float spawnOffsetUp = 0.05f;
    public GameObject waterOnSpawn1;
    public GameObject waterOnSpawn2;
    public GameObject waterOnSpawn3;
    public GameObject waterOnSpawn4;
    private bool isSpawned = false;

    [Header("Throwable")]
    public GameObject throwableObj;
    public bool canThrow = false;

    [Header("Enemy activation")]
    public float activationDistance = 1.8f;
    public float attackDistance = 0.7f;         //Far attack
    public float meleeDistance = 0.5f;          //Near attack
    public const float CHANGE_SIGN = -1;

    private Rigidbody2D rb;
    private Animator animator;
    public bool facingRight = false;

    //Enemy gravity
    private bool collidingDown = false;
    Vector2 velocity = Vector2.zero;

    [Header("Time shoot")]
    private float shotTime = 0.0f;
    public float fireDelta = 0.5f;
    private float nextFire = 0.5f;

    private bool canFall = false;
    public Parallaxing parallax;
    public RunningTarget runningTarget;

    // Start is called before the first frame update
    void Start()
    {

        animator = GetComponent<Animator>();
       
        followPlayer = GameManager.GetPlayer();
        registerHealth();
        rb = GetComponent<Rigidbody2D>();
        blinkingSprite = GetComponent<BlinkingSprite>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.IsGameOver())
            return;

        if (!isSpawned && followPlayer.transform.position.x >= 47f)
        {
            isSpawned = true;
            parallax.setActive(false);
            AudioManager.StartBossAudio();
            StartCoroutine(Spawn());
        }

        if (health.IsAlive())
        {
            float playerDistance = transform.position.x - followPlayer.transform.position.x;
            if (rb && isMovable)
            {
                rb.isKinematic = false;
                if (collidingDown)
                {
                    rb.MovePosition(rb.position + new Vector2(CHANGE_SIGN * Mathf.Sign(playerDistance) * speed, rb.position.y) * Time.deltaTime);
                }
                else
                {
                    //velocity.y -= 9.81f * Time.deltaTime;
                    //rb.MovePosition(new Vector2(transform.position.x, velocity.y));
                    rb.MovePosition(rb.position + new Vector2(CHANGE_SIGN * Mathf.Sign(playerDistance) * speed, rb.position.y - 0.1f) * Time.deltaTime);
                }



            }
        }
    }

    private void registerHealth()
    {
        health = GetComponent<Health>();


        // register health delegate
        health.onDead += OnDead;
        health.onHit += OnHit;
        if(health.GetHealth() <= maxHealth / 2)
        {
            StartCoroutine(HalfHealth());
        }
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
        animator.SetBool("isDying", true);
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

        yield return new WaitForSeconds(1.8f);
        Destroy(gameObject);
    }

    public void setFollow(GameObject follow)
    {
        followPlayer = follow;
    }

   
    private IEnumerator  Spawn()
    {
        yield return new WaitForSeconds(.3f);
        waterOnSpawn1.GetComponent<Animator>().SetBool("isSpawning", true);
        waterOnSpawn2.GetComponent<Animator>().SetBool("isSpawning", true);
        waterOnSpawn3.GetComponent<Animator>().SetBool("isSpawning", true);
        waterOnSpawn4.GetComponent<Animator>().SetBool("isSpawning", true);

        while (this.transform.position.y < -.1f)
        {
            this.transform.position = new Vector3( this.transform.position.x, this.transform.position.y + spawnOffsetUp, this.transform.position.z);
            yield return new WaitForSeconds(.1f);
        }
        waterOnSpawn1.GetComponent<Animator>().SetBool("isSpawning", false);
        waterOnSpawn2.GetComponent<Animator>().SetBool("isSpawning", false);
        waterOnSpawn3.GetComponent<Animator>().SetBool("isSpawning", false);
        waterOnSpawn4.GetComponent<Animator>().SetBool("isSpawning", false);
        CameraManager.AfterBossSpawn();
        runningTarget.SetRunning(true);
        rb.simulated = true;
    }

    private IEnumerator HalfHealth()
    {
        animator.SetBool("isHalfHealth", true);
        yield return new WaitForSeconds(0.11f);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider != null)
        {
            if (collider.CompareTag("Walkable"))
            {
                GameObject bridge = collider.gameObject;
                collider.enabled = false;
                StartCoroutine(DestroyBridge(bridge));

            }
            if (collider.CompareTag("Player"))
            {
                followPlayer.GetComponent<Health>().Hit(attackDamage);
            }
        }
        
    }

    private IEnumerator DestroyBridge(GameObject bridge)
    {

        bridge.GetComponent<Animator>().SetBool("onDestroy", true);
        yield return new WaitForSeconds(1.2f);
        bridge.GetComponent<Animator>().SetBool("onDestroy", false);
        Destroy(bridge);
    }
}
