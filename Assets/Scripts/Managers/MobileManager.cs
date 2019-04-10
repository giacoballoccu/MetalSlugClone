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
#if UNITY_STANDALONE
    private bool forceOnStandalone = false;
#endif

    void Awake()
    {
        current = this;
    }

    void Start()
    {
#if UNITY_STANDALONE
        if (!forceOnStandalone)
        {
            grenadeButton.gameObject.SetActive(false);
            jumpButton.gameObject.SetActive(false);
            shootButton.gameObject.SetActive(false);
            joystick.gameObject.SetActive(false);
        }
#endif
    }

    void LateUpdate()
    {
        btnPressShoot = false;
        btnPressJump = false;
        btnPressGrenade = false;
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

    bool _GetButtonGrenade()
    {
        return btnPressGrenade;
    }

    static public bool GetButtonGrenade()
    {
        if (!current)
            return false;

#if UNITY_STANDALONE
        if (current.forceOnStandalone)
           return current._GetButtonGrenade();
        return Input.GetKeyDown(KeyCode.G);
#else
        return current._GetButtonGrenade();
#endif
    }

    bool _GetButtonFire1()
    {
        return btnPressShoot;
    }

    static public bool GetButtonFire1()
    {
        if (!current)
            return false;

#if UNITY_STANDALONE
        if (current.forceOnStandalone)
            return current._GetButtonFire1();
        return Input.GetButton("Fire1");
#else
        return current._GetButtonFire1();
#endif
    }

    bool _GetButtonJump()
    {
        return btnPressJump;
    }

    static public bool GetButtonJump()
    {
        if (!current)
            return false;

#if UNITY_STANDALONE
        if (current.forceOnStandalone)
            return current._GetButtonJump();
        return Input.GetButton("Jump");
#else
        return current._GetButtonJump();
#endif
    }

    bool _GetButtonCrouch()
    {
        return joystick.Vertical < -0.5f;
    }

    static public bool GetButtonCrouch()
    {
        if (!current)
            return false;

#if UNITY_STANDALONE
        if (current.forceOnStandalone)
            return current._GetButtonCrouch();
        return Input.GetButton("Crouch");
#else
        return current._GetButtonCrouch();
#endif
    }

    float _GetAxisHorizontal()
    {
        // remove small sensibility
        if (Mathf.Abs(joystick.Horizontal) < 0.5f)
            return 0;
        // give full sensibility
        if (joystick.Horizontal > Mathf.Epsilon)
            return 1;
        else if (joystick.Horizontal < -Mathf.Epsilon)
            return -1;
        else // 0 or whatever
            return 0; // joystick.Horizontal
    }

    static public float GetAxisHorizontal()
    {
        if (!current)
            return 0;

#if UNITY_STANDALONE
        if (current.forceOnStandalone)
            return current._GetAxisHorizontal();
        return Input.GetAxis("Horizontal");
#else
        return current._GetAxisHorizontal();
#endif
    }

    float _GetAxisVertical()
    {
        // remove small sensibility
        if (Mathf.Abs(joystick.Vertical) < 0.5f)
            return 0;
        // give full sensibility
        if (joystick.Vertical > Mathf.Epsilon)
            return 1;
        else if (joystick.Vertical < -Mathf.Epsilon)
            return -1;
        else // 0 or whatever
            return 0; // joystick.Vertical
    }

    static public float GetAxisVertical()
    {
        if (!current)
            return 0;

#if UNITY_STANDALONE
        if (current.forceOnStandalone)
            return current._GetAxisVertical();
        return Input.GetAxis("Vertical");
#else
        return current._GetAxisVertical();
#endif
    }
}
