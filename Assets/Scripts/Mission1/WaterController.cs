using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    public Animator waterController;

    void Start()
    {
        waterController.SetBool("isInWater", false);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            waterController.SetBool("isInWater", true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            waterController.SetBool("isInWater", false);
        }
    }
}
