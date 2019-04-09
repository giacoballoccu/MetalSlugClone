using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileManager : MonoBehaviour
{
    public Button grenadeButton;
    public Button jumpButton;
    public Button shootButton;
    public FloatingJoystick joystick;
    static MobileManager current;
    bool btnPressGrenade;
    bool btnPressShoot;
    bool btnPressJump;
    private bool forceOnStandalone = true;

    void Awake()
    {
        current = this;
    }

    public void ClickButtonShoot()
    {
        btnPressShoot = true;
    }

    public void ClickButtonJump()
    {
        btnPressJump = true;
    }

    public void ClickButtonGrenade()
    {
        btnPressGrenade = true;
    }

    static public bool GetButtonGrenade()
    {
        if (!current)
            return false;

#if UNITY_STANDALONE
        if (current.forceOnStandalone)
           return current.btnPressGrenade;
        return Input.GetKeyDown(KeyCode.G);
#else
        return current.btnPressGrenade;
#endif
    }

    static public bool GetButtonFire1()
    {
        if (!current)
            return false;

#if UNITY_STANDALONE
        if (current.forceOnStandalone)
            return current.btnPressShoot;
        return Input.GetButton("Fire1");
#else
        return current.btnPressShoot;
#endif
    }

    static public bool GetButtonJump()
    {
        if (!current)
            return false;

#if UNITY_STANDALONE
        if (current.forceOnStandalone)
            return current.btnPressJump;
        return Input.GetButton("Jump");
#else
        return current.btnPressJump;
#endif
    }

    void LateUpdate()
    {
        btnPressShoot = false;
        btnPressJump = false;
        btnPressGrenade = false;
    }

    static public bool GetButtonCrouch()
    {
        if (!current)
            return false;

#if UNITY_STANDALONE
        if (current.forceOnStandalone)
            return current.joystick.Vertical < -0.6f;
        return Input.GetButton("Crouch");
#else
        return current.joystick.Vertical < -0.6f;
#endif
    }

    static public float GetAxisHorizontal()
    {
        if (!current)
            return 0;

#if UNITY_STANDALONE
        if (current.forceOnStandalone)
            return current.joystick.Horizontal;
        return Input.GetAxis("Horizontal");
#else
        return current.joystick.Horizontal;
#endif
    }

    static public float GetAxisVertical()
    {
        if (!current)
            return 0;

#if UNITY_STANDALONE
        if (current.forceOnStandalone)
            return current.joystick.Vertical;
        return Input.GetAxis("Vertical");
#else
        return current.joystick.Vertical;
#endif
    }
}
