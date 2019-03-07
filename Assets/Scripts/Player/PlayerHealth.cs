using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100;

    public void Hit(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            // Die animation
        }
    }
}
