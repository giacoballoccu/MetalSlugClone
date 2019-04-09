using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunkBoatOpenDoor : MonoBehaviour
{
    public GameObject sunkBoat;
    private bool isDone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDone)
            return;

        if (collision.CompareTag("Player"))
        {
            isDone = true;
            sunkBoat.GetComponent<SunkBoatController>().OpenDoor();
        }
    }
}
