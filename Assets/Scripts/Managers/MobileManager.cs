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
        return current.btnPressGrenade; // todo deleteme
#if UNITY_STANDALONE
        return Input.GetKeyDown(KeyCode.G);
#else
        return current.btnPressGrenade;
#endif
    }

    static public bool GetButtonFire1()
    {
        return current.btnPressShoot; // todo deleteme
#if UNITY_STANDALONE
        return Input.GetButton("Fire1");
#else
        return current.btnPressShoot;
#endif
    }

    static public bool GetButtonJump()
    {
        return current.btnPressJump; // todo deleteme
#if UNITY_STANDALONE
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
        return current.joystick.Vertical < -0.6f; // todo deleteme
#if UNITY_STANDALONE
        return Input.GetButton("Crouch");
#else
        return current.joystick.Vertical < -0.6f;
#endif
    }

    static public float GetAxisHorizontal()
    {
        return current.joystick.Horizontal; // todo deleteme
#if UNITY_STANDALONE
        return Input.GetAxis("Horizontal");
#else
        return current.joystick.Horizontal;
#endif
    }

    static public float GetAxisVertical()
    {
        return current.joystick.Vertical; // todo deleteme
#if UNITY_STANDALONE
        return Input.GetAxis("Vertical");
#else
        return current.joystick.Vertical;
#endif
    }
}
