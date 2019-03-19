using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeMovement : MonoBehaviour
{

    private Rigidbody2D rb;
   
    private float damageGrenade = 300;
    public float grenadeForce = 2.5f;
    public Animator grenadeAnimator;
    public GameObject grenadeSpawn;
    Vector3 grenadeDirection;
    private Vector2 startingPoint;
    private Vector2 controlPoint;
    private Vector2 endingPoint;
    private bool hasHit;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        switch (rb.rotation)
        {
            case 0:
                grenadeDirection = Quaternion.AngleAxis(45, Vector3.forward) * Vector3.right;
                break;
            case 180:
                grenadeDirection = Quaternion.AngleAxis(-45, Vector3.forward) * Vector3.left;
                break;
            case -90:
                grenadeDirection = Quaternion.AngleAxis(-45, Vector3.forward) * Vector3.left;
                break;
            case 90:
                grenadeDirection = Quaternion.AngleAxis(45, Vector3.forward) * Vector3.right;
                break;
        }
        rb.rotation = 0;
        rb.AddForce(grenadeDirection * grenadeForce, ForceMode2D.Impulse);

    }

    //Destroy the bulled when out of camera
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit)
            return;

        if (GameManager.CanTriggerGrenade(collision.tag))
        {
            hasHit = true;
            StartCoroutine(Explosion(collision));
        }
    }

    private IEnumerator Explosion(Collider2D collision)
    {
        AudioManager.PlayGrenadeHitAudio();
        grenadeAnimator.SetBool("hasHittenSth", true);

        collision.GetComponent<Health>()?.Hit(damageGrenade);

        Destroy(rb);
        yield return new WaitForSeconds(1.7f);
        grenadeAnimator.SetBool("hasHittenSth", false);
        Destroy(gameObject);
    }
}
