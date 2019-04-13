using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private float expireTime;
    private bool isSpawned;

    public float bulletForce = 3;
    public float lifeTime = 5;
    public float damageShot = 100;
    public enum LauncherType
    {
        Player,
        Enemy
    };
    public LauncherType launcher = LauncherType.Player;

    void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        expireTime = lifeTime;
        Vector3 bulletDirection = transform.right;
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(bulletDirection * bulletForce, ForceMode2D.Impulse);
        isSpawned = true;
    }

    void Update()
    {
        if (!isSpawned)
            return;

        expireTime -= Time.deltaTime;
        if (expireTime <= 0)
            Despawn();
    }

    private void Despawn()
    {
        if (!isSpawned)
            return;
        isSpawned = false;

        if (launcher == LauncherType.Player)
        {
            BulletManager.GetNormalBulletPool()?.Despawn(this.gameObject);
        }
        else
        {
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
        if ((launcher != LauncherType.Enemy && (collision.CompareTag("Enemy")) || collision.CompareTag("EnemyBomb")) || (collision.CompareTag("Player") && launcher != LauncherType.Player) || collision.CompareTag("Building") || collision.CompareTag("Roof"))
        {
            if (collision.gameObject.GetComponent<Health>())
            {
                collision.gameObject.GetComponent<Health>().Hit(damageShot);
            }

            AudioManager.PlayShotHitAudio();
            Despawn();
        }
    }
}
