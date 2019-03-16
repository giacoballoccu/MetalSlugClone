using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    public Health van;
    public int nBridge;
    public Sprite sprite;

    void Start()
    {
        van.onDead += OnDead;
    }

    void OnDead(float damage)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        if (nBridge == 1)
            CameraManager.AfterFirstVan();
        else if (nBridge == 2)
            CameraManager.AfterSecondVan();
    }
}
