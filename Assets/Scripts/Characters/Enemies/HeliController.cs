using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliController : MonoBehaviour
{
    public float speed = 0.5f;
    public GameObject projSpawner;
    public GameObject projPrefab;

    [Header("Time shoot")]
    private float shotTime = 0.0f;
    public float fireDelta = 0.5f;
    private float nextFire = 0.5f;

    private bool facingRight = false;

    Rigidbody2D rb;

    private int changeDirectionX = -1;
    private int changeDirectionY = 0;
    private bool flipped = false;
    private bool canFire = false;

    private Health health;

    private float height;
    private Animator animator;
    private BlinkingSprite blinkingSprite;
    private HeliSpawner spawner;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        blinkingSprite = GetComponent<BlinkingSprite>();

        registerHealth();

        height = rb.position.y;
    }

    private void registerHealth()
    {
        health = GetComponent<Health>();
        // register health delegate
        health.onDead += OnDead;
        health.onHit += OnHit;
    }
    
    public void RegisterSpawner(HeliSpawner spawner)
    {
        this.spawner = spawner;
    }

    void FixedUpdate()
    {
        if (!health.IsAlive())
            return;

        Move();
        FlipShoot();

        if (canFire)
        {
            Fire();
        }
    }

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
        if (facingRight)
        {
            //Fire down
            projSpawner.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else
        {
            //Fire down
            projSpawner.transform.rotation = Quaternion.Euler(0, 0, 270);
        }
    }

    private void Move()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);

        //Vector3 speed = rb.velocity;
        if ((pos.x == 0 && !facingRight) || (pos.x == 1 && facingRight))
        {
            //Flip heli if it's at the end of the camera
            if (!flipped)
            {
                changeDirectionX *= -1;
                Flip();
                flipped = true;
            }
        }
        else
        {
            //Move the heli
            if (flipped && rb.position.y < height + 0.30f && facingRight)
            {
                changeDirectionY = 1;
            }
            else if (flipped && rb.position.y > height && !facingRight)
            {
                changeDirectionY = -1;
            }
            else
            {
                flipped = false;
                changeDirectionY = 0;
            }

            rb.MovePosition(rb.position + new Vector2(changeDirectionX * speed, changeDirectionY * speed) * Time.deltaTime);

        }
    }

    private void Fire()
    {
        animator.SetBool("isFiring", true);

        shotTime = shotTime + Time.deltaTime;

        if (shotTime > nextFire)
        {
            nextFire = shotTime + fireDelta;

            Instantiate(projPrefab, projSpawner.transform.position, projSpawner.transform.rotation);

            nextFire = nextFire - shotTime;
            shotTime = 0.0f;
        }
    }

    private void OnHit(float damage)
    {
        animator.SetTrigger("isHitten");

        GameManager.AddScore(damage);
        blinkingSprite.Play();
    }

    private void OnDead(float damage)
    {
        spawner?.HeliDestroy(gameObject);
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        AudioManager.PlayMetalSlugDestroy2();
        animator.SetBool("isDying", true);
        if (rb)
            rb.isKinematic = true;
        GetComponent<PolygonCollider2D>().enabled = false;

        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    public void SetFire(bool canFire)
    {
        this.canFire = canFire;
    }
}
