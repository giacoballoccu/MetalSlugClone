using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    private float health;
    PlayerController playerController;

    void Start()
    {
        health = maxHealth;
        playerController = GetComponent<PlayerController>();
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    public void Hit(float damage)
    {
        health -= damage;
        UIManager.UpdateHealthUI(health, maxHealth);
        AudioManager.PlayMeeleeTakeAudio();

        if (health <= 0)
        {
            playerController.Died();
            GameManager.PlayerDied();
            AudioManager.PlayDeathAudio();
        }
    }
}
