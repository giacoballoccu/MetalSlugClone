using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    public PlayerController.CollectibleType type = PlayerController.CollectibleType.HeavyMachineGun;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().getCollectible(type);
        }
    }
}
