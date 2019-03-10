using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    public float health = 100;
    private Collider2D cl;
    public new Camera camera;

    public GameObject secondPart;

    private void Start()
    {
        cl = GetComponent<Collider2D>();
    }


    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            secondPart.SetActive(true);
            camera.GetComponent<CameraController>().setIsBlocked(false);
            this.gameObject.SetActive(false);
        }
    }

    public void Hit(float damage)
    {
        health -= damage;
    }
}
