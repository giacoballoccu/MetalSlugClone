using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startParallax : MonoBehaviour
{
    public Parallaxing parallax;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (GameManager.IsPlayer(collider))
        {
            parallax.setActive(true);
        }
    }
}
