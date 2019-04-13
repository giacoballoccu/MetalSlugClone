using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableMovement : MonoBehaviour
{
    [Header("Throwable Details")]
    private float throwableDamagePlayer = 300f;
    private float throwableDamageEnemy = 10f;
    private float throwableDamageBoss = 25f;
    private float throwableDamageHeavybomb = 50f;
    private float throwableDamageVomit = 25f;
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
        BossBomb,
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

        if (throwable == ThrowableType.Grenade) //Is a Grenade
        {
            BulletManager.GetGrenadePool()?.Despawn(this.gameObject);
        }
        else //if (throwable == ThrowableType.BossHeavyBomb || throwable == ThrowableType.BossBomb) //Is an enemy throwable
        {
            Destroy(gameObject);
        }

    }

    //Destroy the bulled when out of camera
    private void OnBecameInvisible()
    {
        Despawn();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (hasHit)
            return;

        if (GameManager.CanTriggerThrowable(collider) && !(launcher == LauncherType.Player && GameManager.IsPlayer(collider)) && !(launcher == LauncherType.Enemy && (collider.CompareTag("Enemy")|| collider.CompareTag("EnemyBomb"))))
        {
            hasHit = true;

            if (canExplode)
            {
                if (throwable == ThrowableType.BossHeavyBomb)
                {
                    if (collider.CompareTag("Walkable"))
                    {
                        GameObject hittenTerrain = collider.gameObject;
                        StartCoroutine(DestroyHitten(hittenTerrain));
                    }
                }
                StartCoroutine(Explosion(collider));
            }
            else
            {
                ResetMovement(collider);
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


    private void ResetMovement(Collider2D collider)
    {
        var target = collider.gameObject;
        if (GameManager.IsPlayer(collider))
            target = GameManager.GetPlayer(collider);

        switch (throwable)
        {
            case ThrowableType.Grenade:
                target.GetComponent<Health>()?.Hit(throwableDamagePlayer);
                break;
            case ThrowableType.EnemyGrenade:
                target.GetComponent<Health>()?.Hit(throwableDamageEnemy);
                break;
            case ThrowableType.BossHeavyBomb:
                target.GetComponent<Health>()?.Hit(throwableDamageHeavybomb);
                break;
            case ThrowableType.BossBomb:
                target.GetComponent<Health>()?.Hit(throwableDamageBoss);
                break;
            case ThrowableType.Vomit:
                target.GetComponent<Health>()?.Hit(throwableDamageVomit);
                break;
        }

        rb.angularVelocity = 0;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
    }

    private IEnumerator DestroyHitten(GameObject hittenTerrain)
    {
        yield return new WaitForSeconds(0.25f);
        hittenTerrain.GetComponent<Collider2D>().enabled = false;
        hittenTerrain.GetComponent<Animator>().SetBool("onDestroy", true);
        yield return new WaitForSeconds(1.2f);
        hittenTerrain.GetComponent<Animator>().SetBool("onDestroy", false);
        Destroy(hittenTerrain);
    }
}
