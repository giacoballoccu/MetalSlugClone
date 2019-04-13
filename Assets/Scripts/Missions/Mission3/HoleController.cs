using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleController : MonoBehaviour
{
    bool isTriggered;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (isTriggered)
            return;

        if (GameManager.IsPlayer(collider))
        {
            isTriggered = true;
            GetComponent<EventSpawn>().Trigger();
        }
    }
}
