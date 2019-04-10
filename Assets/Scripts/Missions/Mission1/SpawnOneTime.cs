using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOneTime : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<EventSpawn>().Trigger();
            GetComponent<Collider2D>().enabled = false;
        }

    }
}
