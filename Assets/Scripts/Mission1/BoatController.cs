using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    private Collider2D cl;
    public new Camera camera;

    public GameObject secondPart;
    private Health health;

    private void Start()
    {
        cl = GetComponent<Collider2D>();
        registerHealth();
    }

    private void registerHealth()
    {
        health = GetComponent<Health>();
        // register health delegate
        health.onDead += OnDead;
    }

    void OnDead(float damage)
    {
        secondPart.SetActive(true);
        camera.GetComponent<CameraController>().setIsBlocked(false);
        this.gameObject.SetActive(false);
    }
}
