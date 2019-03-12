using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 1.2f;
    public float maxJump = 4f;
    private bool isGrounded = false;

    [Header("Sprite orientation")]
    private bool facingRight = true;
    private bool wasCrounching = false;
    private bool wasFiring = false;

    [Header("Marco Controller")]
    public Animator topAnimator;
    public Animator bottomAnimator;
    public GameObject Up;

    private Rigidbody2D rb;

    [Header("Time shoot")]
    private float shotTime = 0.0f;
    public float fireDelta = 0.6f;
    private float nextFire = 0.5f;

    [Header("Time Crouch")]
    private float crouchTime = 0.0f;
    public float crouchDelta = 0.5f;
    private float nextCrouch = 0.5f;

    [Header("Time jump")]
    private float jumpTime = 0.0f;
    public float jumpDelta = 0.8f;
    private float nextJump = 0.5f;

    [Header("Bullet")]
    public GameObject projectile;
    public GameObject projSpawner;

    [Header("Granate")]
    public GameObject granate;

    [Header("Melee")]
    public float meleeDistance = 0.4f;
    public float damageMelee = 1000f;

    private Health health;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        registerHealth();
    }

    private void registerHealth()
    {
        health = GetComponent<Health>();
        // register health delegate
        health.onDead += OnDead;
        health.onHit += OnHit;
    }

    void Update()
    {
        //Block the player from moving if it's death
        if (GameManager.IsGameOver() || !health.IsAlive())
            return;

        Fire();
        ThrowGranate();
        MoveHorizontally();
        MoveVertically();
        Jump();
        Crouch();

        FlipShoot();
    }

    private void OnDead(float damage) // health delegate onDead
    {
        Died();
        GameManager.PlayerDied();
        AudioManager.PlayDeathAudio();
    }

    private void OnHit(float damage) // health delegate onHit
    {
        UIManager.UpdateHealthUI(health.GetHealth(), health.GetMaxHealth());
        AudioManager.PlayMeleeTakeAudio();
    }

    void Died()
    {
        bottomAnimator.SetBool("isDying", true);
        StartCoroutine(WaitCrouch());
    }

    void Fire()
    {
        shotTime = shotTime + Time.deltaTime;

        if (Input.GetButton("Fire1"))
        {
            if (!wasFiring)
            {
               
                if (CanMelee())
                {
                    /*Animazione in base a se è in piedi o meno*/
                    if (bottomAnimator.GetBool("isCrouched"))
                    {
                        bottomAnimator.SetBool("isMeleeRange", true);
                    }
                    else
                    {
                        topAnimator.SetBool("isMeleeRange", true);
                    }
                    /*fine*/

                    if (shotTime > nextFire)
                    {
                        nextFire = shotTime + fireDelta;

                        StartCoroutine(WaitMelee());
                        nextFire = nextFire - shotTime;
                        shotTime = 0.0f;
                    }

                    wasFiring = true;
                }
                else
                {
                    AudioManager.PlayNormalShotAudio();
                    topAnimator.SetBool("isFiring", true);
                    bottomAnimator.SetBool("isFiring", true);

                    if (shotTime > nextFire)
                    {
                        nextFire = shotTime + fireDelta;

                        StartCoroutine(WaitFire());

                        nextFire = nextFire - shotTime;
                        shotTime = 0.0f;
                    }

                    wasFiring = true;
                }
            }
            else
            {

                bottomAnimator.SetBool("isMeleeRange", false);
                topAnimator.SetBool("isMeleeRange",false);
                topAnimator.SetBool("isFiring", false);
                bottomAnimator.SetBool("isFiring", false);
            }
        }
        else
        {
            bottomAnimator.SetBool("isMeleeRange", false);
            topAnimator.SetBool("isMeleeRange", false);
            topAnimator.SetBool("isFiring", false);
            bottomAnimator.SetBool("isFiring", false);
            wasFiring = false;
        }
    }

    void ThrowGranate()
    {
        if (GameManager.GetBombs() > 0)
        {
            
            shotTime = shotTime + Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.G))
            {

                GameManager.RemoveBomb();
                if (!wasFiring)
                {
                    /*Animazione in base a se è in piedi o meno*/
                    if (bottomAnimator.GetBool("isCrouched"))
                    {
                        bottomAnimator.SetBool("isThrowingGranate", true);
                    }
                    else
                    {
                        topAnimator.SetBool("isThrowingGranate", true);
                    }
                   /*fine*/

                    if (shotTime > nextFire)
                    {
                        nextFire = shotTime + fireDelta;

                        StartCoroutine(WaitGranate());

                        nextFire = nextFire - shotTime;
                        shotTime = 0.0f;
                    }

                    wasFiring = true;
                }
                else
                {
                    /*Animazione in base a se è in piedi o meno*/
                    if (bottomAnimator.GetBool("isCrouched"))
                    {
                        bottomAnimator.SetBool("isThrowingGranate", false);
                    }
                    else
                    {
                        topAnimator.SetBool("isThrowingGranate", false);
                    }
                    /*fine*/
                }
            }
            else
            {
                /*Animazione in base a se è in piedi o meno*/
                if (bottomAnimator.GetBool("isCrouched"))
                {
                    bottomAnimator.SetBool("isThrowingGranate", false);
                }
                else
                {
                    topAnimator.SetBool("isThrowingGranate", false);
                }
                /*fine*/
                wasFiring = false;
            }
        }
        else
        {
            /*Animazione in base a se è in piedi o meno*/
            if (bottomAnimator.GetBool("isCrouched"))
            {
                bottomAnimator.SetBool("isThrowingGranate", false);
            }
            else
            {
                topAnimator.SetBool("isThrowingGranate", false);
            }
            /*fine*/
            return;
        }
    }

    void MoveHorizontally()
    {
        float moveH = Input.GetAxis("Horizontal");

        if (moveH != 0 && !(bottomAnimator.GetBool("isCrouched") && topAnimator.GetBool("isFiring")))
        {
            rb.velocity = new Vector2(moveH * maxSpeed, rb.velocity.y);
            topAnimator.SetBool("isWalking", true);
            bottomAnimator.SetBool("isWalking", true);

            //Flip sprite orientantion if the user is walking right or left
            if (moveH > 0 && !facingRight)
            {
                //Moving right
                Flip();
            }
            else if (moveH < 0 && facingRight)
            {
                //Moving left
                Flip();
            }
        }
        else
        {
            topAnimator.SetBool("isWalking", false);
            bottomAnimator.SetBool("isWalking", false);
        }
    }

    void MoveVertically()
    {
        float moveV = Input.GetAxis("Vertical");

        if (moveV != 0)
        {
            //Yes

            //bottomAnimator.SetBool("isWalking", true);

            //Flip sprite orientantion if the user is walking right or left
            if (moveV > 0)
            {
                //Moving UP
                topAnimator.SetBool("isLookingUp", true);
            }
            else if (moveV < 0)
            {
                //Moving down
            }
        }
        else
        {
            //No
            if (topAnimator.GetBool("isLookingUp"))
            {
                topAnimator.SetBool("isLookingUp", false);
            }
        }
    }

    void Jump()
    {

        jumpTime = jumpTime + Time.deltaTime;

        if (Input.GetButton("Jump") && isGrounded && !bottomAnimator.GetBool("isCrouched"))
        {
            if (jumpTime > nextJump)
            {
                rb.AddForce(new Vector3(0, maxJump, 0), ForceMode2D.Impulse);
                topAnimator.SetBool("isJumping", true);
                bottomAnimator.SetBool("isJumping", true);
                isGrounded = false;

                nextJump = jumpTime + jumpDelta;
                nextJump = nextJump - jumpTime;
                jumpTime = 0.0f;
            }
        }
    }

    void Crouch()
    {
        crouchTime = crouchTime + Time.deltaTime;

        if (Input.GetButton("Crouch") && !Input.GetButton("Jump") && (!(bottomAnimator.GetBool("isWalking") && !wasCrounching) || !bottomAnimator.GetBool("isWalking")))
        {
            if (crouchTime > nextCrouch)
            {
                topAnimator.SetBool("isCrouched", true);
                bottomAnimator.SetBool("isCrouched", true);

                if (isGrounded)
                {
                    StartCoroutine(WaitCrouch());
                }

                if (!wasCrounching)
                {
                    maxSpeed -= 0.4f;
                    projSpawner.transform.position = new Vector3(projSpawner.transform.position.x, projSpawner.transform.position.y - 0.14f, 0);
                }
                nextCrouch = crouchTime + crouchDelta;
                nextCrouch = nextCrouch - crouchTime;
                crouchTime = 0.0f;
                wasCrounching = true;
            }

        }
        else
        {
            topAnimator.SetBool("isCrouched", false);
            bottomAnimator.SetBool("isCrouched", false);

            if (isGrounded)
            {
                Up.SetActive(true);
            }

            if (wasCrounching)
            {
                maxSpeed += 0.4f;
                projSpawner.transform.position = new Vector3(projSpawner.transform.position.x, projSpawner.transform.position.y + 0.14f, 0);
            }
            wasCrounching = false;
        }
    }

    //Flip sprite
    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        //transform.localEulerAngles = transform.eulerAngles + new Vector3(0, 180, -2 * transform.eulerAngles.z);
        facingRight = !facingRight;
    }

    void FlipShoot()
    {
        if (topAnimator.GetBool("isLookingUp") && facingRight)
        {
            //Fire up
            projSpawner.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
        else if (topAnimator.GetBool("isLookingUp") && !facingRight)
        {
            //Fire up
            projSpawner.transform.localEulerAngles = new Vector3(0, 0, 270);
        }
        else if (topAnimator.GetBool("isCrouched") && !isGrounded && facingRight)
        {
            //Fire down
            projSpawner.transform.localEulerAngles = new Vector3(0, 0, 270);
        }
        else if (topAnimator.GetBool("isCrouched") && !isGrounded && !facingRight)
        {
            //Fire down
            projSpawner.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
        else if (facingRight)
        {
            //Fire right
            projSpawner.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            //Fire left
            projSpawner.transform.localEulerAngles = new Vector3(0, 0, 180);
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Walkable" || col.gameObject.tag == "Enemy")
        {
            isGrounded = true;
            topAnimator.SetBool("isJumping", false);
            bottomAnimator.SetBool("isJumping", false);
        }
    }
    
    private IEnumerator WaitFire()
    {
        yield return new WaitForSeconds(0.1f); //Da il tempo all'animazione di fare il primo frame
        Instantiate(projectile, projSpawner.transform.position, projSpawner.transform.rotation);
        yield return new WaitForSeconds(0.2f); //Impedisce che si possa spammare il tasto
    }

    private IEnumerator WaitGranate()
    {
        yield return new WaitForSeconds(0.1f);
        Instantiate(granate, projSpawner.transform.position, projSpawner.transform.rotation);
        yield return new WaitForSeconds(0.15f);
    }

    private RaycastHit2D MeleeRay()
    {
        Vector2 startPos = transform.position;
        float distance = meleeDistance;
        LayerMask layerMask = GameManager.GetEnemyLayer();
        Vector2 direction = (facingRight) ? transform.right : -transform.right;
        Vector2 endPos = startPos + (distance * direction);
        Debug.DrawLine(startPos, endPos, Color.red, 5f);
        return Physics2D.Raycast(startPos, direction, distance, layerMask);
    }

    private bool CanMelee()
    {
        RaycastHit2D hit = MeleeRay();
        return (hit && hit.collider != null);
    }

    private IEnumerator WaitMelee()
    {
        yield return new WaitForSeconds(0.1f);
        RaycastHit2D hit = MeleeRay();
        if (hit && hit.collider != null)
        {
            hit.collider.GetComponent<Health>().Hit(damageMelee);
            AudioManager.PlayMeleeHitAudio();
        }
        yield return new WaitForSeconds(0.2f);
    }

    private IEnumerator WaitCrouch()
    {
        yield return new WaitForSeconds(0.25f);
        Up.SetActive(false);
        yield return new WaitForSeconds(0.25f);
    }
}
