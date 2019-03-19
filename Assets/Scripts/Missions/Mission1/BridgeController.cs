using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    public int nBridge;
    public Sprite sprite;

    public void OnDestroy()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        if (nBridge == 1)
            CameraManager.AfterFirstVan();
        else if (nBridge == 2)
            CameraManager.AfterSecondVan();
    }
}
