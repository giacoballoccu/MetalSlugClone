using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SunkBoatController : MonoBehaviour
{
    public Collider2D cam1p;
    public Collider2D cam2p;
    public GameObject vCam;
    public Animator doorAnimator;

    void OnFinish()
    {
        vCam.GetComponent<CinemachineConfiner>().m_BoundingShape2D = cam2p;
        vCam.GetComponent<CinemachineConfiner>().InvalidatePathCache();

        this.gameObject.SetActive(false);
    }

    public void OpenDoor()
    {
        doorAnimator.SetTrigger("open");

        GetComponent<EventSpawn>().onFinish += OnFinish;
        GetComponent<EventSpawn>().Trigger();
    }
}
