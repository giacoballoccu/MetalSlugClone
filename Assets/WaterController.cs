using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{

    public Animator waterController;
    // Start is called before the first frame update
    void Start()
    {
        waterController.SetBool("isInWater", true);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (waterController.GetBool("isInWater") )
            {
                waterController.SetBool("isInWater", false);
            }
            else
            {
                waterController.SetBool("isInWater", true);
            }
        }
    }
}
