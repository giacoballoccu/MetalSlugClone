using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterTheMosquitos : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (GameManager.IsPlayer(collider))
        {
            CameraManager.AfterMosquitos();
            Destroy(gameObject);
        }
    }
}
