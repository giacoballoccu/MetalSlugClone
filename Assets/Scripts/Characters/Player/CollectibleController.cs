using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    public PlayerController.CollectibleType type = PlayerController.CollectibleType.HeavyMachineGun;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<PlayerController>().getCollectible(type);
            Destroy(gameObject);
        }
    }
}
