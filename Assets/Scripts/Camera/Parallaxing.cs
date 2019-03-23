// Original script from: https://github.com/irunthis/Parallax2D

using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour
{
    private Transform background;     //array (list) of all back- and foregrounds to be parallaxed
    public float parallaxScaleX = -20;  //the proportion of the camera's movement to move the backgrounds by
    public float parallaxScaleY = 0;  //the proportion of the camera's movement to move the backgrounds by
    public float smoothing = 1f;    //how smooth the parallax is going to be, Must be above 0 otherwize the parallax will not work
    public bool isActive = true;

    private Transform cam;  //reference to the camera's transform
    private Vector3 previousCamPos;     //the position of the camera in the previous frame

    //called before Start(), using to assign references.
    void Awake()
    {
        //set up camera the reference
        cam = Camera.main.transform;
    }

    // Use this for initialization
    void Start()
    {
        background = GetComponent<Transform>();
        // store previous frame
        previousCamPos = cam.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isActive)
        {
            //the parallax is the opposite of the camera movement because the previous frame multiplied by the scale
            float parallaxX = (previousCamPos.x - cam.position.x) * parallaxScaleX;
            float parallaxY = (previousCamPos.y - cam.position.y) * parallaxScaleY;

            //set a target x position that is the current position plus the parallax
            float backgroundTargetPosX = background.position.x + parallaxX;
            float backgroundTargetPosY = background.position.y + parallaxY;

            //create a target position which is the backgrounds current position with its target x position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgroundTargetPosY, background.position.z);

            //fade batween current position and the target position using lerp
            background.position = Vector3.Lerp(background.position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        //set the previousCamPos to the camera's position at the end of the frame
        previousCamPos = cam.position;
    }

    public void setActive(bool isActive)
    {
        this.isActive = isActive;
    }
}
