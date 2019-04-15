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
    private bool wasFiring2 = false;

    [Header("Marco Controller")]
    public Animator topAnimator;
    private Animator bottomAnimator;
    public GameObject bottom;
    public GameObject up;

    private Rigidbody2D rb;

    [Header("Time shoot")]
    private float shotTime = 0.0f;
    public float fireDelta = 0.3f;
    private float nextFire = 0.2f;

    [Header("Time Crouch")]
    private float crouchTime = 0.0f;
    public float crouchDelta = 0.5f;
    private float nextCrouch = 0.5f;

    [Header("Time jump")]
    private float jumpTime = 0.0f;
    public float jumpDelta = 0.8f;
    private float nextJump = 0.5f;

    [Header("Bullet")]
    public GameObject projSpawner;

    [Header("Granate")]
    public GameObject granadeSpawner;
    public GameObject granate;

    [Header("Pistol")]
    public AnimatorOverrideController pistolAnimator;
    public AnimatorOverrideController bottomPistolAnimator;

    [Header("Heavy Machine Gun")]
    public AnimatorOverrideController machineGunAnimator;
    public AnimatorOverrideController bottomMachineGunAnimator;
    private bool haveMachineGun = false;

    [Header("Melee")]
    public float meleeDistance = 0.4f;
    public float damageMelee = 200f;

    private Health health;
    private bool asObjUp = false;

    public GameObject foreground;
    Cinemachine.CinemachineBrain cinemachineBrain;

    public enum CollectibleType
    {
        HeavyMachineGun,
        Ammo,
        MedKit,
    };

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bottomAnimator = bottom.GetComponent<Animator>();
        cinemachineBrain = Camera.main.GetComponent<Cinemachine.CinemachineBrain>();
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

        CheckHeavyAmmo();
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

        if (MobileManager.GetButtonFire1())
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
            if (MobileManager.GetButtonGrenade())
            {
                GameManager.RemoveBomb();
                if (!wasFiring2)
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

                    wasFiring2 = true;
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
                wasFiring2 = false;
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

    bool IsOutsideScreen(float moveH)
    {
        //Return a value between [0;1] - 0.5 if the player is in the mid of the camera
        var playerVPPos = Camera.main.WorldToViewportPoint(transform.position);

        //Prevent walking back when camera is blending
        if (moveH < -Mathf.Epsilon && cinemachineBrain.IsBlending)
            return true;

        //Control if the camera is out of the sprite map
        if ((playerVPPos.x < 0.03f || playerVPPos.x > 1 - 0.03f))
            return true;
        return false;
    }

    void MoveHorizontally()
    {
        float moveH = MobileManager.GetAxisHorizontal();
        if (IsOutsideScreen(moveH))
            return;

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
        float moveV = MobileManager.GetAxisVertical();
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

        if (MobileManager.GetButtonJump() && isGrounded && !bottomAnimator.GetBool("isCrouched"))
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
        if (MobileManager.GetButtonCrouch() && MobileManager.GetButtonJump() && isGrounded)
        {
            isGrounded = false;
            if (crouchTime > nextCrouch)
            {
                topAnimator.SetBool("isCrouched", true);
                topAnimator.SetBool("isJumping", true);
                bottomAnimator.SetBool("isJumping", true);

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
        else if (MobileManager.GetButtonCrouch() && !MobileManager.GetButtonJump() && (!(bottomAnimator.GetBool("isWalking") && !wasCrounching) || !bottomAnimator.GetBool("isWalking")) && isGrounded)
        {
            if (crouchTime > nextCrouch)
            {
                topAnimator.SetBool("isCrouched", true);
                bottomAnimator.SetBool("isCrouched", true);

                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                bottom.GetComponent<BoxCollider2D>().enabled = true;

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
            if (!asObjUp && isGrounded)
            {
                topAnimator.SetBool("isCrouched", false);
                bottomAnimator.SetBool("isCrouched", false);

                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                bottom.GetComponent<BoxCollider2D>().enabled = false;

                if (isGrounded)
                {
                    up.GetComponent<SpriteRenderer>().enabled = true;
                }

                if (wasCrounching)
                {
                    maxSpeed += 0.4f;
                    projSpawner.transform.position = new Vector3(projSpawner.transform.position.x, projSpawner.transform.position.y + 0.14f, 0);
                }

                wasCrounching = false;
            }
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
        if (topAnimator.GetBool("isLookingUp"))
        {
            //Fire up
            projSpawner.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (topAnimator.GetBool("isCrouched") && topAnimator.GetBool("isJumping") && facingRight && !isGrounded)
        {
            //Fire down
            projSpawner.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (topAnimator.GetBool("isCrouched") && topAnimator.GetBool("isJumping") && !facingRight && !isGrounded)
        {
            //Fire down
            projSpawner.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (facingRight)
        {
            //Fire right
            projSpawner.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            //Fire left
            projSpawner.transform.rotation = Quaternion.Euler(0, 0, -180);
        }

        //Granade
        if (facingRight)
        {
            //Fire right
            granadeSpawner.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            //Fire left
            granadeSpawner.transform.rotation = Quaternion.Euler(0, 0, -180);
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Walkable") || col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Marco Boat"))
        {
            isGrounded = true;
            topAnimator.SetBool("isJumping", false);
            bottomAnimator.SetBool("isJumping", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.collider.tag);
        if (collision.collider.CompareTag("Water Dead"))
        {
            health.Hit(100);

            if (foreground != null)
                gameObject.transform.parent = foreground.transform;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Roof"))
        {
            asObjUp = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Roof"))
        {
            asObjUp = false;
        }
    }

    private IEnumerator WaitFire()
    {
        yield return new WaitForSeconds(0.1f); //Da il tempo all'animazione di fare il primo frame
        if (haveMachineGun)
        {
            AudioManager.PlayHeavyMachineShotAudio();
            BulletManager.GetNormalBulletPool().Spawn(projSpawner.transform.position, projSpawner.transform.rotation);
            yield return new WaitForSeconds(0.05f);
            BulletManager.GetNormalBulletPool().Spawn(projSpawner.transform.position, projSpawner.transform.rotation);
            yield return new WaitForSeconds(0.05f);
            BulletManager.GetNormalBulletPool().Spawn(projSpawner.transform.position, projSpawner.transform.rotation);
            GameManager.RemoveHeavyMachineAmmo();
        }
        else
        {
            AudioManager.PlayNormalShotAudio();
            BulletManager.GetNormalBulletPool().Spawn(projSpawner.transform.position, projSpawner.transform.rotation);
        }
        
        yield return new WaitForSeconds(0.2f); //Impedisce che si possa spammare il tasto
    }

    private IEnumerator WaitGranate()
    {
        yield return new WaitForSeconds(0.1f);
        BulletManager.GetGrenadePool().Spawn(granadeSpawner.transform.position, granadeSpawner.transform.rotation);
        yield return new WaitForSeconds(0.15f);
    }

    private RaycastHit2D MeleeRay()
    {
        Vector2 startPos = transform.position;

        if (topAnimator.GetBool("isCrouched"))
        {
            startPos = bottom.transform.position;
        }

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
            hit.collider?.GetComponent<Health>()?.Hit(damageMelee);
            AudioManager.PlayMeleeHitAudio();
        }
        yield return new WaitForSeconds(0.2f);
    }

    private IEnumerator WaitCrouch()
    {
        yield return new WaitForSeconds(0.25f);
        up.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.25f);
    }

    public void getCollectible(CollectibleType type)
    {
        switch (type)
        {
            case CollectibleType.HeavyMachineGun:
                if (!haveMachineGun)
                {
                    topAnimator.runtimeAnimatorController = machineGunAnimator;
                    bottomAnimator.runtimeAnimatorController = bottomMachineGunAnimator;
                    GameManager.SetHeavyMachineAmmo(120);
                    UIManager.UpdateAmmoUI();
                    haveMachineGun = true;
                }
                else
                {
                    GameManager.RechargAmmoMG();
                }
                break;
            case CollectibleType.MedKit:
                health.increaseHealth();
                break;
            case CollectibleType.Ammo:
                GameManager.AddAmmo();

                if (!haveMachineGun)
                {
                    GameManager.SetHeavyMachineAmmo(0);
                    UIManager.UpdateAmmoUI();
                }
                break;
            default:
                Debug.Log("Collectible not found");
                break;
        }
    }

    public void CheckHeavyAmmo()
    {
        if (GameManager.GetHeavyMachineAmmo() <= 0 && haveMachineGun)
        {
            topAnimator.runtimeAnimatorController = pistolAnimator;
            bottomAnimator.runtimeAnimatorController = bottomPistolAnimator;
            haveMachineGun = false;
            UIManager.UpdateAmmoUI();
        }
    }
}
