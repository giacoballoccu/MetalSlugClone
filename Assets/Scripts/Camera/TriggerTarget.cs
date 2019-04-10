using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTarget : MonoBehaviour
{
    bool isActive = true;
    Transform target;
    public GameObject prefabRoom;
    void SetActive(bool flag)
    {
        isActive = flag;
        target = GameManager.GetRunningTarget().transform;
    }

    void Start()
    {
        target = GameManager.GetRunningTarget().transform;
        if (!target)
            isActive = false;
    }

    void Update()
    {
        if (isActive)
        {
            if (Mathf.Abs(transform.position.x - target.position.x) < 6.64f)
            {
                Instantiate(prefabRoom, transform.position, transform.rotation);
                isActive = false;
            }
            //Debug.Log(Mathf.Abs(transform.position.x - target.position.x));
        }
    }
}
