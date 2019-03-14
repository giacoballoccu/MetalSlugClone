using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunkBoatOpenDoor : MonoBehaviour
{
    public GameObject sunkBoat;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            sunkBoat.GetComponent<SunkBoatController>().OpenDoor();
        }
    }
}
