using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    private float health;

    void Start()
    {
        health = maxHealth;
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    public void Hit(float damage)
    {
        health -= damage;
        UIManager.UpdateHealthUI(health, maxHealth);

        if (health <= 0)
        {
            GameManager.PlayerDied();
            //AudioManager.PlayDeathAudio();
        }
    }
}
