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
    private float maxHealth = 1000;
    private BlinkingSprite blinkingSprite;
    public GameObject projSpawner;
    private float spawnOffsetUp = 0.05f;

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

    // Start is called before the first frame update
    void Start()
    {

        animator = GetComponent<Animator>();
        followPlayer = GameManager.GetPlayer();
        registerHealth();
        rb = GetComponent<Rigidbody2D>();
        blinkingSprite = GetComponent<BlinkingSprite>();
        
    }

    private void registerHealth()
    {
        health = GetComponent<Health>();
        if(maxHealth == -1f)
        {
            maxHealth = health.GetHealth();
        }

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

    // Update is called once per frame
    void Update()
    {
        Debug.Log(followPlayer.transform.position.x);
        if(followPlayer.transform.position.x >= 47f)
        {
            StartCoroutine(Spawn());
        }


        
    }

    private IEnumerator  Spawn()
    {
        yield return new WaitForSeconds(2f);
        rb.isKinematic = false;
        while(this.transform.position.y < -0.1f)
        {
            this.transform.position = new Vector3( this.transform.position.x, this.transform.position.y + spawnOffsetUp, this.transform.position.z);
            yield return new WaitForSeconds(1f);
        }
        rb.isKinematic = true;
        
            }

    private IEnumerator HalfHealth()
    {
        animator.SetBool("isHalfHealth", true);
        yield return new WaitForSeconds(0.11f);
    }
}
