using UnityEngine;


public class CameraController : MonoBehaviour
{
    private new Camera camera;

    public Transform player;
    private Vector2 playerVPPos;
    private Vector2 oldPosition;

    private bool isBlocked = false;

    void Start()
    {
        camera = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        //Return a value between [0;1] - 0.5 if the player is in the mid of the camera
        playerVPPos = camera.WorldToViewportPoint(player.position);

        //If the player is in the right part of the screen
        if (playerVPPos.x > 0.5f && !isBlocked)
        {
            //Get the new camera position by interpolating the current position and the position of the player + 0.25
            transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x + 0.25f, transform.position.y, 0), 3f * Time.deltaTime);
        }

        //Move the camera to the player height
        if (playerVPPos.y < 0.2f && !isBlocked)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x, transform.position.y-0.1f, 0), 3f * Time.deltaTime);
        }


        //Control if the camera is out of the sprite map
        float dx = oldPosition.x - player.transform.position.x;

        if ((playerVPPos.x < 0.03f || playerVPPos.x > 1 - 0.03f) && Mathf.Abs(dx) < 1)
        {
            //Restore old player position (block him)
            player.transform.position = new Vector2(oldPosition.x, player.transform.position.y);
        }

        oldPosition = player.transform.position;
    }

    public void setIsBlocked(bool isBlocked)
    {
        this.isBlocked = isBlocked;
    }
}
