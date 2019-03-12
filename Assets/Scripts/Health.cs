using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    private float health;

    public delegate void OnEvent();    public OnEvent onHit;    public OnEvent onDead;

    void Start()
    {
        health = maxHealth;
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealth()
    {
        return health;
    }

    public void Hit(float damage)
    {
        if (!IsAlive()) // skip already dead
            return;

        health -= damage; // take the hit
        if (onHit != null)
           onHit();

        if (!IsAlive()) // check if died
        {
            if (onDead != null)
                onDead();
        }
    }
}
