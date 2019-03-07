using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image healthBar;
    public float maxHealth = 100;
    private float health;

    void Start()
    {
        health = maxHealth;
        healthBar.fillAmount = 1;
    }

    public void Hit(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / maxHealth;

        if (health <= 0)
        {
            //GameManager.PlayerDied();
            //AudioManager.PlayDeathAudio();
        }
    }
}
