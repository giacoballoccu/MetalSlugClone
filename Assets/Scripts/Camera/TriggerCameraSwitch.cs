using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerCameraSwitch : MonoBehaviour
{
    public UnityEvent triggerCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (GameManager.IsPlayer(collider))
        {
            triggerCamera.Invoke();
            Destroy(gameObject);
        }
    }
}
