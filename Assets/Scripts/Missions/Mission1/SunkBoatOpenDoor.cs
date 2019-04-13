using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunkBoatOpenDoor : MonoBehaviour
{
    public GameObject sunkBoat;
    private bool isDone;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (isDone)
            return;

        if (GameManager.IsPlayer(collider))
        {
            isDone = true;
            sunkBoat.GetComponent<SunkBoatController>().OpenDoor();
        }
    }
}
