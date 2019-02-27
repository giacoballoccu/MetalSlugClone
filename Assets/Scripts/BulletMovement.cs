using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    public float bulletForce = 3;
    public float lifeTime = 5;
    public float fireTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector3 bulletDirection = transform.right;

        fireTime = lifeTime;

        rb.AddForce(bulletDirection * bulletForce, ForceMode2D.Impulse);
    }

    void Update()
    {
        fireTime -= Time.deltaTime;
        if (fireTime <= 0)
            Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //collision.gameObject.GetComponent<Health>().loseHealth(30f);
        }

        Destroy(this.gameObject);
    }

    //Destroy the bulled when out of camera
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
