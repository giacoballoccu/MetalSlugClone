using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopController : MonoBehaviour
{
    private Health health;
    private BlinkingSprite blinkingSprite;
    // Start is called before the first frame update
    void Start()
    {
        blinkingSprite = GetComponent<BlinkingSprite>();
        registerHealth();
    }

    private void registerHealth()
    {
        health = GetComponent<Health>();
    }



}
