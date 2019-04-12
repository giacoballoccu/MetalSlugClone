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
    public CinemachineVirtualCamera vcamZ2D;
    public CinemachineVirtualCamera vcamZ2E;
    [Header("Zone 3")]
    public CinemachineVirtualCamera vcamZ3A;
    public CinemachineVirtualCamera vcamZ3B;

    //public bool enableParallax = true;

    void Awake()
    {
        if (current)
            Destroy(current);
        current = this;
    }

    #region Editor Switches
    public void SwitchZ1AtoZ1B()
    {
        SwitchCameras(vcamZ1A, vcamZ1B);
    }
    public void SwitchZ1BtoZ1C()
    {
        SwitchCameras(vcamZ1B, vcamZ1C);
    }
    public void SwitchZ1CtoZ1D()
    {
        SwitchCameras(vcamZ1C, vcamZ1D);
    }
    public void SwitchZ1DtoZ1E()
    {
        SwitchCameras(vcamZ1D, vcamZ1E);
    }
    public void SwitchZ1EtoZ2A()
    {
        SwitchCameras(vcamZ1E, vcamZ2A);
    }
    public void SwitchZ2AtoZ2B()
    {
        SwitchCameras(vcamZ2A, vcamZ2B);
    }
    public void SwitchZ2BtoZ2C()
    {
        SwitchCameras(vcamZ2B, vcamZ2C);
    }
    public void SwitchZ2CtoZ2D()
    {
        SwitchCameras(vcamZ2C, vcamZ2D);
    }
    public void SwitchZ2DtoZ2E()
    {
        SwitchCameras(vcamZ2D, vcamZ2E);
    }
    public void SwitchZ2CtoZ3A()
    {
        SwitchCameras(vcamZ2C, vcamZ3A);
    }
    public void SwitchZ2EtoZ3A()
    {
        SwitchCameras(vcamZ2E, vcamZ3A);
    }
    public void SwitchZ3AtoZ3B()
    {
        SwitchCameras(vcamZ3A, vcamZ3B);
    }
    #endregion

    public static void SwitchCameras(CinemachineVirtualCamera cam1, CinemachineVirtualCamera cam2)
    {
        if (cam1 != null)
            cam1.gameObject.SetActive(false);
        if (cam2 != null)
            cam2.gameObject.SetActive(true);
    }

    //public static void EnableParallax(bool flag)
    //{
    //    if (current)
    //        current.enableParallax = flag;
    //}

    #region Mission 1 Switches
    public static void AfterCrabTower()
    {
        //If there is no current Camera Manager, exit
        if (current == null)
            return;

        current.SwitchZ1AtoZ1B();
    }

    public static void AfterSunkBoat()
    {
        //If there is no current Camera Manager, exit
        if (current == null)
            return;

        current.SwitchZ1BtoZ1C();
    }

    public static void AfterBossSpawn()
    {
        //If there is no current Camera Manager, exit
        if (current == null)
            return;

        current.SwitchZ2CtoZ3A();
    }

    public static void AfterSunkCartel()
    {
        //If there is no current Camera Manager, exit
        if (current == null)
            return;

        current.SwitchZ1CtoZ1D();
    }

    public static void AfterMosquitos()
    {
        //If there is no current Camera Manager, exit
        if (current == null)
            return;

        current.SwitchZ1DtoZ1E();
    }

    public static void AfterMarcoBoatCartel()
    {
        //If there is no current Camera Manager, exit
        if (current == null)
            return;

        current.SwitchZ1EtoZ2A();
    }

    public static void AfterFirstVan()
    {
        //If there is no current Camera Manager, exit
        if (current == null)
            return;

        current.SwitchZ2AtoZ2B();
    }

    public static void AfterSecondVan()
    {
        //If there is no current Camera Manager, exit
        if (current == null)
            return;

        current.SwitchZ2BtoZ2C();
    }
    #endregion


    #region Mission 2 Switches
    public static void AfterFirstHeli()
    {
        if (current == null)
            return;
        current.SwitchZ1EtoZ2A();
    }
    public static void AfterSecondHeli()
    {
        if (current == null)
            return;
        current.SwitchZ2DtoZ2E();
    }
    #endregion












}
