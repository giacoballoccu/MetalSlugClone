using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    public float bulletForce = 3;
    public float lifeTime = 5;
    public float damageShot = 100;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector3 bulletDirection = transform.right;

        rb.AddForce(bulletDirection * bulletForce, ForceMode2D.Impulse);
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
            Destroy(this.gameObject);
    }

    //Destroy the bulled when out of camera
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyControl>().Hit(damageShot);
            AudioManager.PlayShotHitAudio();
            GameManager.AddScore(damageShot);
            Destroy(gameObject);
        }
        else if (collision.tag == "Building")
        {
            collision.gameObject.GetComponent<BuildingController>().Hit(damageShot);
            AudioManager.PlayShotHitAudio();
            GameManager.AddScore(damageShot);
            Destroy(gameObject);
        }
    }
}
