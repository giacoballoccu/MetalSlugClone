using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{

    [Header("Enemy information")]
    GameObject followPlayer;
    public float attackDamage = 25f;
    public bool isMovable = true;
    public AudioClip deathClip;
    private Health health;
    private float maxHealth;
    private BlinkingSprite blinkingSprite;
    public Transform bossSpawner;
    public GameObject projSpawner;
    private float spawnOffsetUp = 0.05f;

    public GameObject boat;
    private bool isSpawned = false;

    [Header("Speeds")]
    public float speed = 0.7f;
    private float chargingSpeed = 0f;
    private float restSpeed = 0.10f;
    private float sprintSpeed = 2f;
    private float initialSpeed = 0.7f;
    

    [Header("Throwable")]
    public GameObject normalFire;
    public GameObject heavyBomb;
    public bool canThrow = true;

    [Header("Enemy activation")]
    public const float CHANGE_SIGN = -1;

    private Rigidbody2D rb;
    private Animator animator;



    [Header("Time shoot")]
    private float shotTime = 0.0f;
    public float fireDelta = 0.5f;
    private float nextFire = 2f;

    [Header("Time sprint")]
    private bool canSprint = true;

    [Header("Camera")]
    public Parallaxing parallax;
    public RunningTarget runningTarget;

    // Start is called before the first frame update
    void Start()
    {

        animator = GetComponent<Animator>();
       
        followPlayer = GameManager.GetPlayer();
        registerHealth();
        maxHealth = health.GetMaxHealth();
        rb = GetComponent<Rigidbody2D>();
        blinkingSprite = GetComponent<BlinkingSprite>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.IsGameOver())
            return;

        if (!isSpawned && followPlayer.transform.position.x >= bossSpawner.position.x)
        {
            isSpawned = true;
            parallax.setActive(false);
            AudioManager.StartBossAudio();
            StartCoroutine(Spawn());
        }

        if (isSpawned)
        {
            if (health.IsAlive())
            {
                /*Check health*/
                if (health.GetHealth() <= maxHealth / 2)
                {
                    StartCoroutine(HalfHealth());
                }
                
                /*Run and attacks*/
                float playerDistance = transform.position.x - followPlayer.transform.position.x;
                if (rb && isMovable)
                {
                    rb.MovePosition(rb.position + new Vector2(1 * speed, 0) * Time.deltaTime);
                }

                if (canSprint && Random.Range(0, 100) < 10) // 10% chance of sprint
                {
                    canSprint = false;
                    StartCoroutine(Sprint());
                }

                if (!(health.GetHealth() <= maxHealth / 2) && this.transform.position.y >= -.9f)
                {
                    shotTime = shotTime + Time.deltaTime;

                    if (shotTime > nextFire)
                    {
                        nextFire = shotTime + fireDelta;

                        StartCoroutine(WaitFire(normalFire));

                        nextFire = nextFire - shotTime;
                        shotTime = 0.0f;
                    }
                }
                else if (this.transform.position.y >= -.9f && health.GetHealth() <= maxHealth / 2)
                {
                    shotTime = shotTime + Time.deltaTime;

                    if (shotTime > nextFire)
                    {
                        nextFire = shotTime + fireDelta;

                        StartCoroutine(WaitFire(heavyBomb));

                        nextFire = nextFire - shotTime;
                        shotTime = 0.0f;
                    }
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
        
    }


    private void OnHit(float damage)
    {
        GameManager.AddScore(damage);
        blinkingSprite.Play();
    }

    private IEnumerator Die()
    {
        AudioManager.PlayMetalSlugDestroy3();
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
        AudioManager.PlayMetalSlugDestroy1();
        Destroy(gameObject);
    }

    public void setFollow(GameObject follow)
    {
        followPlayer = follow;
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(7f);

        while (this.transform.position.y < -.1f)
        {
            this.transform.position = new Vector3( this.transform.position.x, this.transform.position.y + spawnOffsetUp, this.transform.position.z);
            yield return new WaitForSeconds(.1f);
        }

        CameraManager.AfterBossSpawn();
        runningTarget.SetRunning(true);

        rb.simulated = true;
        yield return new WaitForSeconds(1.75f);

        runningTarget.SetSpeed(initialSpeed);
    }

    private IEnumerator HalfHealth()
    {
        animator.SetBool("isHalfHealth", true);
        projSpawner.transform.position.Set(-0.63f, -0.21f,0);
        yield return new WaitForSeconds(1f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider != null)
        {
            if (GameManager.IsPlayer(collision))
            {
                followPlayer.GetComponent<Health>().Hit(attackDamage);
                followPlayer.GetComponent<Rigidbody2D>().AddForce(new Vector2(3f, 0f), ForceMode2D.Impulse);
            }
            else if (collision.collider.CompareTag("Enemy"))
            {
                collision.collider.gameObject.GetComponent<Health>().onHit(attackDamage);

                collision.collider.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(2f, 0f), ForceMode2D.Impulse);

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject != null)
        {
            if (collider.CompareTag("Walkable"))
            {
                GameObject bridge = collider.gameObject;
                StartCoroutine(DestroyBridge(bridge));
            }
        }

    }

    private IEnumerator WaitFire(GameObject throwableObj)
    {
        yield return new WaitForSeconds(0.1f);
        Instantiate(throwableObj, projSpawner.transform.position, projSpawner.transform.rotation);
        yield return new WaitForSeconds(0.15f);
    }

    private IEnumerator DestroyBridge(GameObject bridge)
    {
        if (bridge)
        {
            if (bridge)
                bridge.GetComponent<Animator>().SetBool("onDestroy", true);
            yield return new WaitForSeconds(0.2f);
            if (bridge)
                bridge.GetComponent<Collider2D>().enabled = false;
            yield return new WaitForSeconds(1f);
            if (bridge)
                bridge.GetComponent<Animator>().SetBool("onDestroy", false);
            Destroy(bridge);
        }

    }

    private void OnDead(float damage)
    {
        this.GetComponent<Animator>().SetBool("isDying", true);

        StopCoroutine("Sprint");
        StopCoroutine("WaitFire");
        GameManager.PlayerWin();
        StopBossCoroutines();
    }

    private void StopBossCoroutines()
    {
        StopAllCoroutines();
    }

    private IEnumerator Sprint()
    {
        speed = chargingSpeed;
        yield return new WaitForSeconds(1.5f);
        runningTarget.SetRunning(true);
        speed = sprintSpeed;
        yield return new WaitForSeconds(1.2f);
        speed = restSpeed;
        yield return new WaitForSeconds(1f);
        speed = initialSpeed;
        yield return new WaitForSeconds(5f); // wait until next possible sprint
        canSprint = true;
    }
}
