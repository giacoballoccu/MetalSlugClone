using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingSprite : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float delayTime = 0.15f;
    private float hitCount;
    private bool isRunning;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Stop()
    {
        hitCount = 0; // reset duration
        spriteRenderer.color = Color.white; // reset blink
        StopBlinking();
    }

    public void StopBlinking()
    {
        if (!isRunning)
            return;
        StopCoroutine("Blinking");
    }

    public void StartBlinking()
    {
        if (isRunning)
            return;
        StartCoroutine("Blinking");
    }

    public void Play()
    {
        hitCount++;
        StartBlinking();
    }

    public bool CanBlink()
    {
        return hitCount > 0;
    }

    IEnumerator Blinking()
    {
        isRunning = true;
        while (CanBlink())
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(delayTime);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(delayTime);
            hitCount--;
        }
        isRunning = false;
    }
}
