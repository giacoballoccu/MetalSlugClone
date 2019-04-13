using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarcoBoatStartMove : MonoBehaviour
{
    public GameObject boat;
    public GameObject target0;
    public GameObject target1;
    public GameObject target2;
    public GameObject target3;

    public GameObject waterSpawners;
    public GameObject waterOnSpawn1;
    public GameObject waterOnSpawn2;
    public GameObject waterOnSpawn3;
    public GameObject waterOnSpawn4;
    public GameObject thirdPart;

    public float offsetToTarget;
    private float finalOffset = 1.25f;
    private bool isDying = false;

    public float speed = 0.35f;

    private bool isStarted = false;

    public GameObject explosion;

    private void Update()
    {
        if (isStarted)
        {
            GameObject target = target1;

            float step = speed * Time.deltaTime;

            if (target0 != null)
            {
                //Enemy boat
                target = target0;
                boat.transform.position = new Vector2(Vector2.MoveTowards(boat.transform.position, new Vector2(target.transform.position.x - offsetToTarget, target.transform.position.y), step).x, boat.transform.position.y);
            }
            else if (target1 != null)
            {
                //First bridge
                target = target1;
                boat.transform.position = new Vector2(Vector2.MoveTowards(boat.transform.position, new Vector2(target.transform.position.x - offsetToTarget, target.transform.position.y), step).x, boat.transform.position.y);
            } else if(target2 != null)
            {
                //Second bridge
                target = target2;
                boat.transform.position = new Vector2(Vector2.MoveTowards(boat.transform.position, new Vector2(target.transform.position.x - offsetToTarget, target.transform.position.y), step).x, boat.transform.position.y);
            } else if(target3 != null)
            {
                //Boss bridge
                target = target3;
              
                boat.transform.position = new Vector2(Vector2.MoveTowards(boat.transform.position, new Vector2(target.transform.position.x - finalOffset, target.transform.position.y), step).x, boat.transform.position.y);
            }
            if(!isDying && boat.transform.position.x >= 44.4f)
            {
                isDying = true;
                StartCoroutine(Exploding());
            }


        }
    }

    private IEnumerator Exploding()
    {
        yield return new WaitForSeconds(1f);
        waterSpawners.transform.parent = thirdPart.transform;
        GameManager.GetPlayer().transform.parent = thirdPart.transform;

        boat.GetComponent<Animator>().SetBool("isDying", true);
        AudioManager.PlayMetalSlugDestroy3();

        yield return new WaitForSeconds(2.8f);
        explosion.GetComponent<Animator>().SetBool("isExploding", true);
        AudioManager.PlayMetalSlugDestroy1();

        yield return new WaitForSeconds(.4f);
        waterOnSpawn1.GetComponent<Animator>().SetBool("BoatExploding", true);
        waterOnSpawn2.GetComponent<Animator>().SetBool("BoatExploding", true);
        waterOnSpawn3.GetComponent<Animator>().SetBool("BoatExploding", true);
        waterOnSpawn4.GetComponent<Animator>().SetBool("BoatExploding", true);
        AudioManager.PlayMetalSlugDestroy2();

        yield return new WaitForSeconds(1.4f);
        boat.GetComponent<Animator>().SetBool("isDying", false);
        boat.GetComponent<SpriteRenderer>().enabled = false;

        //boat.GetComponent<Rigidbody2D>().isKinematic = true;
        yield return new WaitForSeconds(0.5f);
        explosion.SetActive(false);
        waterOnSpawn1.GetComponent<Animator>().SetBool("BoatExploding", false);
        waterOnSpawn2.GetComponent<Animator>().SetBool("BoatExploding", false);
        waterOnSpawn3.GetComponent<Animator>().SetBool("BoatExploding", false);
        waterOnSpawn4.GetComponent<Animator>().SetBool("BoatExploding", false);
        yield return new WaitForSeconds(0.7f);

        waterSpawners.transform.position = new Vector3(waterSpawners.transform.position.x, -1.2f, 10f);
        yield return new WaitForSeconds(0.75f);
        waterOnSpawn1.GetComponent<Animator>().SetBool("isSpawning", true);
        waterOnSpawn2.GetComponent<Animator>().SetBool("isSpawning", true);
        waterOnSpawn3.GetComponent<Animator>().SetBool("isSpawning", true);
        waterOnSpawn4.GetComponent<Animator>().SetBool("isSpawning", true);
      



        
        yield return new WaitForSeconds(4.2f);

        waterOnSpawn1.GetComponent<Animator>().SetBool("isSpawning",false);
        waterOnSpawn2.GetComponent<Animator>().SetBool("isSpawning", false);
        waterOnSpawn3.GetComponent<Animator>().SetBool("isSpawning", false);
        waterOnSpawn4.GetComponent<Animator>().SetBool("isSpawning", false);
        Destroy(waterSpawners);
        Destroy(boat);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (GameManager.IsPlayer(collider) && !isStarted)
        {
            isStarted = true;
            GameManager.GetPlayer().transform.parent = boat.transform;
        }
        else if (collider.CompareTag("Enemy"))
        {
            collider.gameObject.transform.parent = boat.transform;
        }

        
    }
}
