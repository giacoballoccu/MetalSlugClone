using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{

    public Health van;
    public Sprite sprite;

    void Update()
    {
        if (van == null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}
