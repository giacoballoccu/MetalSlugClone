using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOneTime : MonoBehaviour
{

    public bool random = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!random)
        {
            if (collision.CompareTag("Player"))
            {
                GetComponent<EventSpawn>().Trigger();
                GetComponent<Collider2D>().enabled = false;
            }
        }
        else
        {
            if (collision.CompareTag("Player"))
            {
                GetComponent<EventSpawn>().TriggerRandomCollectible();
                GetComponent<Collider2D>().enabled = false;
            }
        }


    }
}
