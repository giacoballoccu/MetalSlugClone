using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeMovement : MonoBehaviour
{

    private Rigidbody2D rb;
   
    private float damageGrenade = 300;
    public float grenadeForce = 3;
    public Animator grenadeAnimator;
    public GameObject grenadeSpawn;
    Vector3 grenadeDirection;
    private Vector2 startingPoint;
    private Vector2 controlPoint;
    private Vector2 endingPoint;

    public float aoeRangeX = 0.95f;
    public float aoeRangeY = 0.8f;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
       StartCoroutine(Explosion(collision));
    }

    private IEnumerator Explosion(Collider2D collision)
    {
        if(rb != null)
        {
            if (GameManager.CanTriggerGrenade(collision.tag))
            {
                AudioManager.PlayGrenadeHitAudio();
                grenadeAnimator.SetBool("hasHittenSth", true);
                {
                    if(collision.tag == "Enemy")
                    {
                        collision.GetComponent<EnemyControl>().Hit(damageGrenade);
                    }
                    else if(collision.tag == "Building")
                    {
                        collision.GetComponent<BuildingController>().Hit(damageGrenade);
                    }
                    else if(collision.tag == "Boat")
                    {
                        collision.GetComponent<BoatController>().Hit(damageGrenade);
                    }
                }

                this.rb.rotation = 0;
                Destroy(rb);
                yield return new WaitForSeconds(1.7f);
                grenadeAnimator.SetBool("hasHittenSth", false);
                Destroy(gameObject);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(rb.position, new Vector3(aoeRangeX, aoeRangeY));
    }
}
