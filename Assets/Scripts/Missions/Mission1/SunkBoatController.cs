using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SunkBoatController : MonoBehaviour
{
    public Animator doorAnimator;

    void OnFinish()
    {
        CameraManager.AfterSunkBoat();
        this.gameObject.SetActive(false);
    }

    public void OpenDoor()
    {
        doorAnimator.SetTrigger("open");

        GetComponent<EventSpawn>().onFinish += OnFinish;
        GetComponent<EventSpawn>().Trigger();
    }
}
