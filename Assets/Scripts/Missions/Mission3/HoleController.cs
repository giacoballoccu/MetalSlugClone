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
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isTriggered = true;
            GetComponent<EventSpawn>().Trigger();
        }
    }
}
