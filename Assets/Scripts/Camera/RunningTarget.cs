using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningTarget : MonoBehaviour
{
    public float speedX = 0.4f;
    public float speedY = 0f;
    public bool running = false;

    public void SetRunning(bool flag)
    {
        running = flag;
    }

    void FixedUpdate()
    {
        if (!GameManager.IsGameOver() && running)
            transform.position = new Vector2(
                transform.position.x + speedX * Time.deltaTime,
                transform.position.y + speedY * Time.deltaTime);
    }
}
