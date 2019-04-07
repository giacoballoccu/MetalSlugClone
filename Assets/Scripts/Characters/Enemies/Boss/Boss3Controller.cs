using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Controller : MonoBehaviour
{
    [Header("Attack1 time")]
    private float attack1Time = 0f;
    public float attack1Delta = 11f;

    public GameObject headSpawner;
    public GameObject bulletPrefab;
    public GameObject prjSpawner;

    private Health health;
    private BlinkingSprite blinkingSprite;

    private float halfAngleofCone = 50f;

    private Rigidbody2D rb;
    private float initialY;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialY = transform.position.y;

        attack1Time = attack1Delta - 4;
    }

    void Update()
    {
        // Oscillate Y
        transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time) * 0.1f + initialY);

        // Attack 1 delta time
        attack1Time += Time.deltaTime;
        if (attack1Time > attack1Delta && !headSpawner.GetComponent<Animator>().GetBool("prepareAttack1"))
        {
            StartCoroutine(Fire1());

            attack1Time = 0.0f;
        }

    }

    private IEnumerator Fire1()
    {
        int nFires = 0;

        headSpawner.GetComponent<Animator>().SetBool("prepareAttack1", true);

        yield return new WaitForSeconds(2f);

        // Fire 10 proj
        while (nFires < 10)
        {
            GameObject bullet = Instantiate(bulletPrefab, prjSpawner.transform.position, prjSpawner.transform.rotation * Quaternion.Euler(0, 0, Random.Range(-halfAngleofCone, halfAngleofCone)));
            nFires++;

            yield return new WaitForSeconds(0.4f);
        }
        
        headSpawner.GetComponent<Animator>().SetBool("prepareAttack1", false);
    }
}
