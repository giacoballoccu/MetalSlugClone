using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startParallax : MonoBehaviour
{
    public Parallaxing parallax;

    private void OnTriggerEnter2D(Collider2D c)
    {
        if(c.tag == "Player")
        {
            parallax.setActive(true);
        }
    }
}
