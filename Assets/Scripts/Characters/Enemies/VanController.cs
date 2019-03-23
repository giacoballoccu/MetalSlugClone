using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanController : MonoBehaviour
{
    public BridgeController ownBridge;

    void OnDestroy()
    {
        ownBridge.SetBridgeDestroyed();
    }
}
