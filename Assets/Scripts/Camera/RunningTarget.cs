using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningTarget : MonoBehaviour
{
    float _speedX;
    float _speedY;
    bool _running = false;
    //private Transform _target;
    private Transform _target;

    //public void SetRunning(bool running, Transform target)
    public void SetRunning(bool running)
    {
        _running = running;
        //_target = target;
    }

    public void SetSpeed(float speedX = 0f, float speedY = 0f)
    {
        _speedX = speedX;
        _speedY = speedY;
    }

    void FixedUpdate()
    {
        if (!GameManager.IsGameOver() && _running /*&& _target*/)
        {
            transform.position = new Vector2(
                transform.position.x + _speedX * Time.deltaTime,
                transform.position.y + _speedY * Time.deltaTime);
            //transform.position = new Vector2(_target.position.x + 5f, transform.position.y);
        }
    }
}
