using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Controller : MonoBehaviour
{
    [Header("Attack1 time")]
    public float attack1Delta = 11f;
    private float attack1Time = 0f;

    [Header("Attack2 time")]
    public float attack2Delta = 11f;
    private float attack2Time = 0f;

    [Header("Hit time")]
    public float hitDamage = 25;
    public float hitDelta = 1.25f;
    private float hitTime = 0f;

    [Header("Head spawners")]
    public GameObject headSpawner;
    public GameObject bulletPrefab;
    public GameObject prjSpawner;

    [Header("Mouth spawners")]
    public GameObject mouthSpawner;

    [Header("Movements")]
    public float attackSpeed = 0.5f;
    public GameObject start;
    public GameObject center;
    public GameObject end;

    private Health health;
    private BlinkingSprite blinkingSprite;
    private GameObject player;

    private float halfAngleofCone = 50f;

    private Rigidbody2D rb;
    private float initialY;

    //Attack 1 bool
    private bool isAttack1 = false;

    // Attack 2 bool
    private bool isAttack2 = false;
    private bool isInPositionAttack2 = false;
    private bool isFinishedAttack2 = false;

    [Header("Audio")]
    public AudioClip thunderClip;
    public AudioClip sunClip;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        blinkingSprite = GetComponent<BlinkingSprite>();
        player = GameManager.GetPlayer();
        registerHealth();
        registerPlayerHealth();

        initialY = transform.position.y;

        attack1Time = attack1Delta - 4;

        attack2Time = attack2Delta - 10;
    }

    void Update()
    {
        if (GameManager.IsGameOver())
            return;

        if (health.IsAlive())
        {
            if (isAttack2)
            {
                if (transform.position.x <= start.transform.position.x && !isInPositionAttack2 && !isFinishedAttack2)
                {
                    //Move to start position
                    rb.MovePosition(rb.position + new Vector2(attackSpeed, 0) * Time.deltaTime);
                } else if (isFinishedAttack2)
                {
                    //Return enemy to center
                    if (transform.position.x <= center.transform.position.x)
                    {
                        mouthSpawner.GetComponent<BoxCollider2D>().enabled = false;
                        rb.MovePosition(rb.position + new Vector2(attackSpeed, 0) * Time.deltaTime);
                    } else
                    {
                        isFinishedAttack2 = false;
                        isAttack2 = false;
                    }
                }
                else
                {
                    if (isInPositionAttack2)
                    {
                        // Start fire ray animation
                        mouthSpawner.GetComponent<BoxCollider2D>().enabled = true;
                        mouthSpawner.GetComponent<Animator>().SetBool("canAttack", true);
                        rb.MovePosition(rb.position + new Vector2(-attackSpeed, 0) * Time.deltaTime);
                    }
                    else
                    {
                        // Start ray animation
                        mouthSpawner.GetComponent<Animator>().SetBool("prepareAttack2", true);
                        StartCoroutine(Fire2Wait());
                    }

                    if (transform.position.x <= end.transform.position.x)
                    {
                        // Enemy in end position
                        mouthSpawner.GetComponent<Animator>().SetBool("prepareAttack2", false);
                        mouthSpawner.GetComponent<Animator>().SetBool("canAttack", false);
                        isInPositionAttack2 = false;
                        isFinishedAttack2 = true;
                    }
                }
            } else
            {
                // Attack 1 and 2 delta time
                attack1Time += Time.deltaTime;
                attack2Time += Time.deltaTime;

                // Oscillate Y
                transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time) * 0.1f + initialY);

                if (attack1Time > attack1Delta && !headSpawner.GetComponent<Animator>().GetBool("prepareAttack1") && !mouthSpawner.GetComponent<Animator>().GetBool("prepareAttack2") && !isAttack2)
                {
                    isAttack1 = true;
                    StartCoroutine(Fire1());

                    attack1Time = 0.0f;
                }
                else if (attack2Time > attack2Delta && !headSpawner.GetComponent<Animator>().GetBool("prepareAttack1") && !mouthSpawner.GetComponent<Animator>().GetBool("prepareAttack2") && !isAttack1)
                {
                    headSpawner.GetComponent<Animator>().SetBool("prepareAttack1", false);

                    isAttack2 = true;

                    attack2Time = 0.0f;
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

    private void registerPlayerHealth()
    {
        Health playerHealth = player.GetComponent<Health>();
        // register health delegate
        playerHealth.onDead += OnPlayerDead;
    }

    private void StopBossCoroutines()
    {
        StopAllCoroutines();
    }

    private void OnDead(float damage)
    {
        headSpawner.GetComponent<Animator>().SetBool("isDying", true);
        mouthSpawner.GetComponent<Animator>().SetBool("isDying", true);

        StopCoroutine(Fire1());
        StopCoroutine(Fire2Wait());
        GameManager.PlayerWin();
        AudioManager.PlayMetalSlugDestroy2();
        GetComponent<Animator>().SetBool("isDying", true);
        StopBossCoroutines();
    }

    private void OnPlayerDead(float damage)
    {
        StopBossCoroutines();
    }

    private void OnHit(float damage)
    {
        GameManager.AddScore(damage);
        blinkingSprite.Play();
    }

    private IEnumerator Fire1()
    {
        headSpawner.GetComponent<Animator>().SetBool("prepareAttack1", true);
        yield return new WaitForSeconds(0.3f);

        if (sunClip)
            AudioManager.PlayEnemyAttackAudio(sunClip);
        yield return new WaitForSeconds(1.7f);

        headSpawner.GetComponent<Animator>().SetBool("prepareAttack1", false);
        // Fire 10 proj
        int nFires = 0;
        while (nFires < 10 && !GameManager.IsGameOver())
        {
            GameObject bullet = Instantiate(bulletPrefab, prjSpawner.transform.position, prjSpawner.transform.rotation * Quaternion.Euler(0, 0, Random.Range(-halfAngleofCone, halfAngleofCone)));
            nFires++;

            yield return new WaitForSeconds(0.35f);
        }
        isAttack1 = false;
    }

    private IEnumerator Fire2Wait()
    {
        if (thunderClip)
            AudioManager.PlayEnemyAttackAudio(thunderClip);
        yield return new WaitForSeconds(0.8f);
        isInPositionAttack2 = true;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        hitTime += Time.deltaTime;
        if (GameManager.IsPlayer(collider))
        {
            if (hitTime > hitDelta)
            {
                player.GetComponent<Health>().Hit(hitDamage);

                hitTime = 0.0f;
            }
        }
    }
}
