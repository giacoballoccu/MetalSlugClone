using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableMovement : MonoBehaviour
{
    [Header("Throwable Details")]
    private float throwableDamagePlayer = 300f;
    private float throwableDamageEnemy = 25f;
    private float throwableDamageHeavybomb = 50f;
    public float throwableForce = 2.5f;

    public enum LauncherType
    {
        Player,
        Enemy
    };
    public LauncherType launcher = LauncherType.Player;

    public enum ThrowableType
    {
        Grenade,
        BossHeavyBomb,
        EnemyGrenade,
        Vomit,
        Bubble
    };
    public ThrowableType throwable = ThrowableType.Grenade;

    public bool canExplode = true;


    private Animator throwableAnimator;
    private Rigidbody2D rb;

    Vector3 throwableDirection;

    private bool hasHit;
    private bool isSpawned;

    private void Start()
    {
        throwableAnimator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        Init();
    }

    void Init()
    {
        rb = GetComponent<Rigidbody2D>();
        switch (rb.rotation)
        {
            case 0:
                throwableDirection = Quaternion.AngleAxis(45, Vector3.forward) * Vector3.right;
                break;
            case 180:
                throwableDirection = Quaternion.AngleAxis(-45, Vector3.forward) * Vector3.left;
                break;
            case -90:
                throwableDirection = Quaternion.AngleAxis(-45, Vector3.forward) * Vector3.left;
                break;
            case 90:
                throwableDirection = Quaternion.AngleAxis(45, Vector3.forward) * Vector3.right;
                break;
        }

        rb.gravityScale = .5f;
        rb.rotation = 0;
        rb.AddForce(throwableDirection * throwableForce, ForceMode2D.Impulse);
        hasHit = false;
        isSpawned = true;
    }

    private void Despawn()
    {
        if (!isSpawned)
            return;


        isSpawned = false;

        if (throwable == ThrowableType.Grenade)
        {
            //Is a Grenade
            BulletManager.GetGrenadePool()?.Despawn(this.gameObject);
        }
        else
        {
            //Is an enemy throwable
            Destroy(gameObject);
        }
        
    }

    //Destroy the bulled when out of camera
    private void OnBecameInvisible()
    {
        Despawn();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit)
            return;



        if (GameManager.CanTriggerThrowable(collision.tag) && !(collision.tag == "Player" && launcher == LauncherType.Player) && !(collision.tag == "Enemy" && launcher == LauncherType.Enemy))
        {
            hasHit = true;

            if (canExplode)
            {
                if (throwable == ThrowableType.BossHeavyBomb)
                {
                    if (collision.tag == "Bridge")
                    {
                        Destroy(collision.gameObject);
                    }
                }
                StartCoroutine(Explosion(collision));
                }
            else
                {
                    ResetMovement(collision);
                    Despawn();
                }
        }
    }

    private IEnumerator Explosion(Collider2D collision)
    {
        AudioManager.PlayGrenadeHitAudio();
        throwableAnimator.SetBool("hasHittenSth", true);

        ResetMovement(collision);

        yield return new WaitForSeconds(1.7f);
        throwableAnimator.SetBool("hasHittenSth", false);
        Despawn();
    }


    private void ResetMovement(Collider2D collision)
    {
        switch(throwable)
        {
            case ThrowableType.Grenade:
                collision.GetComponent<Health>()?.Hit(throwableDamagePlayer);
                break;
            case ThrowableType.EnemyGrenade:
                collision.GetComponent<Health>()?.Hit(throwableDamageEnemy);
                break;
            case ThrowableType.BossHeavyBomb:
                collision.GetComponent<Health>()?.Hit(throwableDamageHeavybomb);
                break;
           
        }
            
        

        rb.angularVelocity = 0;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
    }
}
