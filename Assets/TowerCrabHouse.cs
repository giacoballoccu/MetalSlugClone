using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCrabHouse : MonoBehaviour
{
    public GameObject vcamIn;
    public GameObject vcamOut;
    private Health health;

    private void Start()
    {
        // register health delegate
        health = GetComponent<Health>();
        health.onDead += OnDead;
    }

    void OnDead(float damage)
    {
        vcamIn.SetActive(false);
        vcamOut.SetActive(true);
    }
}
