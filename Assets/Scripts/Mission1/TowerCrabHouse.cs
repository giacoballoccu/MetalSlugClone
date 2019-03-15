using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCrabHouse : MonoBehaviour
{
    private Health health;

    private void Start()
    {
        GetComponent<Health>().onDead += OnDead; // register health delegate
    }

    void OnDead(float damage)
    {
        CameraManager.AfterCrabTower();
    }
}
