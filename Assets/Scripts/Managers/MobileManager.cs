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
    PlayerController playerController;
    static MobileManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        var player = GameManager.GetPlayer();
        playerController = player.GetComponent<PlayerController>();
    }

    static public bool GetButtonBomb()
    {
        return Input.GetKeyDown(KeyCode.G);
    }

    static public bool GetButtonFire1()
    {
        return Input.GetButton("Fire1");
    }

    static public bool GetButtonJump()
    {
        return Input.GetButton("Jump");
    }

    static public bool GetButtonCrouch()
    {
#if UNITY_STANDALONE
        return instance.joystick.Vertical < -0.6f;
        return Input.GetButton("Crouch");
#else
        return instance.joystick.Vertical < -0.6f;
#endif
    }

    static public float GetAxisHorizontal()
    {
#if UNITY_STANDALONE
        return instance.joystick.Horizontal;
        return Input.GetAxis("Horizontal");
#else
        return instance.joystick.Horizontal;
#endif
    }

    static public float GetAxisVertical()
    {
#if UNITY_STANDALONE
        return instance.joystick.Vertical;
        return Input.GetAxis("Vertical");
#else
        return instance.joystick.Vertical;
#endif
    }
}
