using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public float health = 100;
    public Sprite destroyedSprite;

    private SpriteRenderer sr;
    private Collider2D cl;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        cl = GetComponent<Collider2D>();
    }


    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            sr.sprite = destroyedSprite;
            cl.enabled = false;
        }
    }

    public void Hit(float damage)
    {
        if (health > 0)
        {
            GameManager.AddScore(damage);
        }
        health -= damage;
          }
}
