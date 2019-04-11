using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCrabHouse : MonoBehaviour
{
    private Health health;
    public SpriteRenderer bgBoat;

    private void Start()
    {
        GetComponent<Health>().onDead += OnDead; // register health delegate
    }

    void OnDead(float damage)
    {
        CameraManager.AfterCrabTower();
        AudioManager.PlayMetalSlugDestroy2();
        bgBoat.sprite = null;
    }
}
