using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeMovement : MonoBehaviour
{

    private Rigidbody2D rb;
   
    //public float lifeTime = 5;
    private float damageGrenade = 300;
    public float grenadeForce;
    public Animator grenadeAnimator;
    public GameObject grenadeSpawn;
    Vector3 grenadeDirection;
    private Vector2 startingPoint;
    private Vector2 controlPoint;
    private Vector2 endingPoint;

    public float aoeRangeX;
    public float aoeRangeY;

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
            if (collision.tag == "Enemy" || collision.tag == "Building" || collision.tag == "Boat" || collision.tag == "Terrain" || collision.tag == "Walkable")
            {
                grenadeAnimator.SetBool("hasHittenSth", true);
                Collider2D[] thingsToDamage = Physics2D.OverlapBoxAll(rb.position, new Vector2(aoeRangeX, aoeRangeY), 0, GameManager.GetEnemyLayer());
                foreach (Collider2D thing in thingsToDamage)
                {
                    if(thing.tag == "Enemy")
                    {
                        thing.GetComponent<EnemyControl>().Hit(damageGrenade);
                    }
                    else if(thing.tag == "Building")
                    {
                        thing.GetComponent<BuildingController>().Hit(damageGrenade);
                    }
                    else if(thing.tag == "Boat")
                    {
                        thing.GetComponent<BoatController>().Hit(damageGrenade);
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
