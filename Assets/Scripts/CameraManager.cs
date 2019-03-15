using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    //This class holds a static reference to itself to ensure that there will only be
    //one in existence. This is often referred to as a "singleton" design pattern. Other
    //scripts access this one through its public static methods
    static CameraManager current;

    public CinemachineVirtualCamera vcamZ1A;
    public CinemachineVirtualCamera vcamZ1B;
    public CinemachineVirtualCamera vcamZ2A;

    void Awake()
    {
        //If a Camera Manager exists and this isn't it...
        if (current != null && current != this)
        {
            //...destroy this and exit. There can only be one Camera Manager
            Destroy(gameObject);
            return;
        }

        //Set this as the current game manager
        current = this;

        //Persist this object between scene reloads
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    public static void AfterCrabTower()
    {
        //If there is no current Camera Manager, exit
        if (current == null)
            return;

        current.vcamZ1A.gameObject.SetActive(false);
        current.vcamZ1B.gameObject.SetActive(true);
    }

    public static void AfterSunkBoat()
    {
        //If there is no current Camera Manager, exit
        if (current == null)
            return;

        current.vcamZ1B.gameObject.SetActive(false);
        current.vcamZ2A.gameObject.SetActive(true);
    }
}
