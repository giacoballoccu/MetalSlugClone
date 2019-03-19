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

    [Header("Zone 1")]
    public CinemachineVirtualCamera vcamZ1A;
    public CinemachineVirtualCamera vcamZ1B;
    public CinemachineVirtualCamera vcamZ1C;
    public CinemachineVirtualCamera vcamZ1D;
    public CinemachineVirtualCamera vcamZ1E;
    [Header("Zone 2")]
    public CinemachineVirtualCamera vcamZ2A;
    public CinemachineVirtualCamera vcamZ2B;
    public CinemachineVirtualCamera vcamZ2C;

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

    public void SwitchZ1AtoZ1B()
    {
        SwitchCameras(vcamZ1A, vcamZ1B);
    }

    public static void SwitchCameras(CinemachineVirtualCamera cam1, CinemachineVirtualCamera cam2)
    {
        cam1.gameObject.SetActive(false);
        cam2.gameObject.SetActive(true);
    }

    #region Mission 1 Switches
    public static void AfterCrabTower()
    {
        //If there is no current Camera Manager, exit
        if (current == null)
            return;

        SwitchCameras(current.vcamZ1A, current.vcamZ1B);
    }

    public static void AfterSunkBoat()
    {
        //If there is no current Camera Manager, exit
        if (current == null)
            return;

        SwitchCameras(current.vcamZ1B, current.vcamZ1C);
    }

    public static void AfterSunkCartel()
    {
        //If there is no current Camera Manager, exit
        if (current == null)
            return;

        SwitchCameras(current.vcamZ1C, current.vcamZ1D);
    }

    public static void AfterMosquitos()
    {
        //If there is no current Camera Manager, exit
        if (current == null)
            return;

        SwitchCameras(current.vcamZ1D, current.vcamZ1E);
    }

    public static void AfterMarcoBoatCartel()
    {
        //If there is no current Camera Manager, exit
        if (current == null)
            return;

        SwitchCameras(current.vcamZ1E, current.vcamZ2A);
    }

    public static void AfterFirstVan()
    {
        //If there is no current Camera Manager, exit
        if (current == null)
            return;

        SwitchCameras(current.vcamZ2A, current.vcamZ2B);
    }

    public static void AfterSecondVan()
    {
        //If there is no current Camera Manager, exit
        if (current == null)
            return;

        SwitchCameras(current.vcamZ2B, current.vcamZ2C);
    }
    #endregion


    #region Mission 2 Switches
    #endregion












}
