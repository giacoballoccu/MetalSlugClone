using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    public int nBridge;
    public GameObject bridgeDestroyed;

    public void SetBridgeDestroyed()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        bridgeDestroyed.SetActive(true);

        if (nBridge == 1)
            CameraManager.AfterFirstVan();
        else if (nBridge == 2)
            CameraManager.AfterSecondVan();
    }
}
