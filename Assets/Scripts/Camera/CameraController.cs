using UnityEngine;


public class CameraController : MonoBehaviour
{
    private Camera camera2;
    private GameObject player;
    private Vector2 playerVPPos;
    private Vector2 oldPosition;

    void Start()
    {
        player = GameManager.GetPlayer();
        camera2 = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        //Return a value between [0;1] - 0.5 if the player is in the mid of the camera
        playerVPPos = camera2.WorldToViewportPoint(player.transform.position);
        
        //Control if the camera is out of the sprite map
        float dx = oldPosition.x - player.transform.position.x;

        if ((playerVPPos.x < 0.03f || playerVPPos.x > 1 - 0.03f) && Mathf.Abs(dx) < 1)
        {
            //Restore old player position (block him)
            player.transform.position = new Vector2(oldPosition.x, player.transform.position.y);
        }

        oldPosition = player.transform.position;
    }
}
