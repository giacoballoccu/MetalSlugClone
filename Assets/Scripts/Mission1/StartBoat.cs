using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBoat : MonoBehaviour
{
    public GameObject boat;
    public GameObject target;

    public float speed = 0.35f;

    private bool isStarted = false;

    private void Update()
    {
        if (isStarted)
        {
            //Move boat
            float step = speed * Time.deltaTime;
            boat.transform.position = new Vector2(Vector2.MoveTowards(boat.transform.position, new Vector2(target.transform.position.x-1.5f, target.transform.position.y), step).x, boat.transform.position.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            isStarted = true;
            collision.gameObject.transform.parent = boat.transform;
        }
    }
}
