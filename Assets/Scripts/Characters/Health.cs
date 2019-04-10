using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    public float health;
    private bool isPlayer;

    public delegate void OnDamageEvent(float damage);    public OnDamageEvent onHit;    public OnDamageEvent onDead;

    void Start()
    {
        isPlayer = GetComponent<PlayerController>() != null; // PlayerController is just for player
        var difficulty = GameManager.GetDifficultyMode(); // get difficulty

        if (!isPlayer) // only if enemy
        {
            if (difficulty == GameManager.Difficulty.Easy) // hp - 30%
                maxHealth *= 0.7f;
            else if(difficulty == GameManager.Difficulty.Hard) // hp + 30%
                maxHealth *= 1.3f;
        }
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

    public void increaseHealth()
    {
        health += maxHealth * 0.2f;
        if (health > maxHealth)
            health = maxHealth;
        UIManager.UpdateHealthUI(health, maxHealth);
    }

    public void Hit(float damage)
    {
        if (!IsAlive() || GameManager.IsGameOver()) // skip already dead or gameover
            return;

        if (isPlayer) // only if player
        {
            var difficulty = GameManager.GetDifficultyMode(); // get difficulty
            if (difficulty == GameManager.Difficulty.Easy) // dmg - 30%
                damage *= 0.7f;
            else if (difficulty == GameManager.Difficulty.Hard) // dmg + 30%
                damage *= 1.3f;
        }
        health -= damage; // take the hit

        onHit?.Invoke(damage);

        if (!IsAlive()) // check if died
            onDead?.Invoke(damage);
    }
}
