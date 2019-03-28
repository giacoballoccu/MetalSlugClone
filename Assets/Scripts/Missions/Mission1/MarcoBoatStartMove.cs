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
    public float offsetToTarget;

    public float speed = 0.35f;

    private bool isStarted = false;

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
            }else if (target1 != null)
            {
                //First bridge
                target = target1;
            } else if(target2 != null)
            {
                //Second bridge
                target = target2;
            } else if(target3 != null)
            {
                //Boss bridge
                target = target3;
            }

            //Move boat
            boat.transform.position = new Vector2(Vector2.MoveTowards(boat.transform.position, new Vector2(target.transform.position.x-offsetToTarget, target.transform.position.y), step).x, boat.transform.position.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isStarted = true;
            collision.gameObject.transform.parent = boat.transform;
        }
    }
}
