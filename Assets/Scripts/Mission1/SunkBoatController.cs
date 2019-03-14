using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunkBoatController : MonoBehaviour
{
    public new Camera camera;
    public GameObject secondPart;
    public Animator doorAnimator;

    void OnFinish()
    {
        secondPart.SetActive(true);
        camera.GetComponent<CameraController>().setIsBlocked(false);
        //Debug.Log("unlocked camera");
        this.gameObject.SetActive(false);
    }

    public void OpenDoor()
    {
        doorAnimator.SetTrigger("open");
        GetComponent<RegenSpawn>().onFinish += OnFinish;
        GetComponent<RegenSpawn>().Trigger();
    }
}
