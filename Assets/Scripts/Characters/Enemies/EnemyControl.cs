using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [Header("Enemy information")]
    GameObject followPlayer;
    public float speed = 0.5f;
    public float attackDamage = 10f;
    public bool isMovable = true;
    public bool canMelee = true;
    public AudioClip deathClip;
    public AudioClip meleeAttackClip;
    public AudioClip rangeAttackClip;
    private Health health;
    private BlinkingSprite blinkingSprite;
    public GameObject projSpawner;

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
    public bool collidingDown = false;
    Vector2 velocity = Vector2.zero;

    [Header("Time shoot")]
    private float shotTime = 0.0f;
    public float fireDelta = 0.5f;
    private float nextFire = 0.5f;
    public float rangedDelta = 2f;

    private bool canFall = false;

    private void Start()
    {
        followPlayer = GameManager.GetPlayer();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        blinkingSprite = GetComponent<BlinkingSprite>();
        registerHealth();
        checkCanFall();
    }

    private void checkCanFall()
    {
        foreach (var parameter in animator.parameters)
        {
            if (parameter.name == "isFalling")
            {
                canFall = true;
                break;
            }
        }
    }

    public void setFollow(GameObject follow)
    {
        followPlayer = follow;
    }

    private void registerHealth()
    {
        health = GetComponent<Health>();
        // register health delegate
        health.onDead += OnDead;
        health.onHit += OnHit;
    }

    private void Update()
    {
        if (GameManager.IsGameOver())
            return;
    }

    void FixedUpdate()
    {
        if (GameManager.IsGameOver())
            return;

        if (health.IsAlive())
        {
            FlipShoot();
            if (canFall)
                animator.SetBool("isFalling", !collidingDown);

            float playerDistance = transform.position.x - followPlayer.transform.position.x;
            if (playerDistance < activationDistance && collidingDown)
            {
                if (Mathf.Abs(playerDistance) <= meleeDistance && canMelee)
                {
                    //Attack player - Primary attack (near)
                    animator.SetBool("isAttacking", true);
                    animator.SetBool("isAttacking_2", false);

                    if (rb)
                        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

                    shotTime = shotTime + Time.deltaTime;

                    if (shotTime > nextFire)
                    {
                        nextFire = shotTime + fireDelta;

                        // check also the correct height
                        if (Mathf.Abs(GetComponent<SpriteRenderer>().bounds.SqrDistance(followPlayer.transform.position)) <= meleeDistance)
                        {
                            followPlayer.GetComponent<Health>().Hit(attackDamage);
                            if (meleeAttackClip)
                                AudioManager.PlayEnemyAttackAudio(meleeAttackClip);
                        }

                        nextFire = nextFire - shotTime;
                        shotTime = 0.0f;
                    }
                }
                else if(Mathf.Abs(playerDistance) <= attackDistance && canThrow)
                {
                    //Attack player - Secondary attack (far)
                    animator.SetBool("isAttacking_2", true);
                    animator.SetBool("isAttacking", false);

                   if (rb && !canMelee)
                        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                    else
                        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

                    shotTime = shotTime + Time.deltaTime;

                    if (shotTime > nextFire)
                    {
                        nextFire = shotTime + rangedDelta;

                        StartCoroutine(WaitSecondaryAttack());

                        nextFire = nextFire - shotTime;
                        shotTime = 0.0f;
                    }
                }
                else
                {
                    //Move to the player
                    if (rb && isMovable)
                    {
                        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                        if (collidingDown)
                        {
                            rb.MovePosition(rb.position + new Vector2(CHANGE_SIGN * Mathf.Sign(playerDistance) * speed, rb.position.y) * Time.deltaTime);
                        }
                        /*else
                        {
                            rb.MovePosition(rb.position + new Vector2(CHANGE_SIGN * Mathf.Sign(playerDistance) * speed, rb.position.y - 0.1f) * Time.deltaTime);
                        }*/

                        animator.SetBool("isWalking", true);
                        animator.SetBool("isAttacking", false);
                        animator.SetBool("isAttacking_2", false);
                    }
                }
            }

            //Flip enemy
            if (playerDistance < 0 && !facingRight)
            {
                Flip();
            }
            else if (playerDistance > 0 && facingRight)
            {
                Flip();
            }
        }

    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        facingRight = !facingRight;
    }

    void FlipShoot()
    {
        if (projSpawner == null)
            return;

        if (facingRight)
        {
            //Fire right
            projSpawner.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            //Fire left
            projSpawner.transform.rotation = Quaternion.Euler(0, 0, -180);
        }
    }

    private void OnDead(float damage)
    {
        StartCoroutine(Die());
    }

    private void OnHit(float damage)
    {
        animator.SetTrigger("isHitten");

        GameManager.AddScore(damage);
        blinkingSprite.Play();
    }

    private IEnumerator Die()
    {
        PlayDeathAudio();
        animator.SetBool("isDying", true);
        if (rb)
            rb.isKinematic = true;
        if (GetComponent<BoxCollider2D>())
        {
            GetComponent<BoxCollider2D>().enabled = false;
        } else if (GetComponent<CapsuleCollider2D>())
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
        }

        yield return new WaitForSeconds(1.8f);
        Destroy(gameObject);
    }

    private void PlayDeathAudio()
    {
        if (deathClip)
            AudioManager.PlayEnemyDeathAudio(deathClip);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Walkable") || collision.collider.CompareTag("Marco Boat") || collision.collider.CompareTag("Water Dead"))
        {
            collidingDown = true;
        }

        if (GameManager.IsPlayer(collision))
        {
            switch(Random.Range(0, 1))
            {
                case 0:
                    gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(1f, 2f), ForceMode2D.Impulse);
                    break;
                case 1:
                    gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1f, 2f), ForceMode2D.Impulse);
                    break;
            }
           
        }

        else if (collision.collider.CompareTag("Water Dead"))
        {
            health.Hit(health.GetHealth());
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Walkable") || collision.collider.CompareTag("Marco Boat"))
        {
            collidingDown = false;
        }
    }

    private IEnumerator WaitSecondaryAttack()
    {
        yield return new WaitForSeconds(0.1f);
        if (rangeAttackClip)
            AudioManager.PlayEnemyAttackAudio(rangeAttackClip);
        Instantiate(throwableObj, projSpawner.transform.position, projSpawner.transform.rotation);
        yield return new WaitForSeconds(0.15f);
    }
}
