using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granate_movement : MonoBehaviour
{

    private Rigidbody2D rb;
   
    //public float lifeTime = 5;
    private float damageGranate = 300;
    public float granateForce;
    public Animator granateAnimator;
    public GameObject granateSpawn;
    Vector3 granateDirection;
    private Vector2 startingPoint;
    private Vector2 controlPoint;
    private Vector2 endingPoint;

    public float aoeRangeX;
    public float aoeRangeY;
    public LayerMask whatIsEnemy;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        switch (rb.rotation)
        {
            case 0:
                granateDirection = Quaternion.AngleAxis(45, Vector3.forward) * Vector3.right;
                break;
            case 180:
                granateDirection = Quaternion.AngleAxis(-45, Vector3.forward) * Vector3.left;
                break;
            case -90:
                granateDirection = Quaternion.AngleAxis(-45, Vector3.forward) * Vector3.left;
                break;
            case 90:
                granateDirection = Quaternion.AngleAxis(45, Vector3.forward) * Vector3.right;
                break;
        }
        rb.rotation = 0;
        rb.AddForce(granateDirection * granateForce, ForceMode2D.Impulse);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
       StartCoroutine(Explosion(collision));
    }

    private IEnumerator Explosion(Collider2D collision)
    {
        if(rb != null)
        {
            if (collision.tag == "Enemy" || collision.tag == "Building" || collision.tag == "Terrain" || collision.tag == "Walkable")
            {
                granateAnimator.SetBool("hasHittenSth", true);
                Collider2D[] thingsToDamage = Physics2D.OverlapBoxAll(rb.position, new Vector2(aoeRangeX, aoeRangeY), 0, whatIsEnemy);
                foreach (Collider2D thing in thingsToDamage)
                {
                    if(thing.tag == "Enemy")
                    {
                        thing.GetComponent<EnemyControl>().Hit(damageGranate);
                    }
                    else if(thing.tag == "Building")
                    {
                        thing.GetComponent<BuildingController>().Hit(damageGranate);
                    }

                }

                this.rb.rotation = 0;
                Destroy(rb);
                yield return new WaitForSeconds(1.7f);
                granateAnimator.SetBool("hasHittenSth", false);
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
