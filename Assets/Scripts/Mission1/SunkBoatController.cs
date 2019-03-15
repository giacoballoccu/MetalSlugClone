using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SunkBoatController : MonoBehaviour
{
    public GameObject vcamIn;
    public GameObject vcamOut;
    public Animator doorAnimator;

    void OnFinish()
    {
        vcamIn.SetActive(false);
        vcamOut.SetActive(true);

        this.gameObject.SetActive(false);
    }

    public void OpenDoor()
    {
        doorAnimator.SetTrigger("open");

        GetComponent<EventSpawn>().onFinish += OnFinish;
        GetComponent<EventSpawn>().Trigger();
    }
}
