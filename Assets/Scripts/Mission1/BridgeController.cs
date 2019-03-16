using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{

    public Health van;
    public int nBridge;
    public Sprite sprite;

    private bool changedCamera = false;

    void Update()
    {
        if (van == null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite;

            if (!changedCamera)
            {
                if (nBridge == 1)
                {
                    CameraManager.AfterFirstVan();
                    changedCamera = true;
                }
                else if (nBridge == 2)
                {
                    CameraManager.AfterSecondVan();
                    changedCamera = true;
                }
            }
        }
    }
}
