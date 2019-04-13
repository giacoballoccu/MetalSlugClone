using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Controller : MonoBehaviour
{
    public GameObject top;
    private Health health;
    private float maxHealth;
    private bool isBossActive = false;

    public Animator topAnimator;
    public Animator bottomAnimator;


    [Header("Scene Informations")]
    private float sceneBorderLF = -1.4f;
    private float sceneBorderRG = 1.4f;
    private float fixedYRocks = 2.5f;

    [Header("Throwable")]
    public GameObject throwableObj;
    public GameObject throwableIndicator;
    public GameObject projSpawner;

    [Header("Time shoot")]
    private float shotTime = 0.0f;
    public float fireDelta = 3f;
    private float nextFire = 0f;
    private float chargingTime = 1.25f;

    [Header("Audio")]
    public AudioClip rayClip;

    void Start()
    {
        topAnimator = top.GetComponent<Animator>();
        registerHealth();
        maxHealth = health.GetMaxHealth();
        //Debug.Log(projSpawner.transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.IsGameOver())
            return;
        if (isBossActive)
        {
            if (health.IsAlive())
            {
                shotTime = shotTime + Time.deltaTime;

                if (shotTime > nextFire)
                {
                    topAnimator.SetBool("isAttacking", true);
                    nextFire = shotTime + fireDelta;

                    StartCoroutine(Fire());

                    nextFire = nextFire - shotTime;
                    shotTime = 0.0f;

                }

            }
            }
        
           
        }

    private IEnumerator Fire()
    {
        //Debug.Log(projSpawner.transform.position);
        Vector3 initialPosition = projSpawner.transform.position;
        float randomX = Random.Range(sceneBorderLF, sceneBorderRG);
        projSpawner.transform.position = new Vector3(projSpawner.transform.position.x + randomX, projSpawner.transform.position.y);
        //Debug.Log(projSpawner.transform.position);
        Instantiate(throwableIndicator, projSpawner.transform.position, projSpawner.transform.rotation);
        if (rayClip)
            AudioManager.PlayEnemyAttackAudio(rayClip);
        yield return new WaitForSeconds(chargingTime);
       // Debug.Log(projSpawner.transform.position);
        projSpawner.transform.position = new Vector3(projSpawner.transform.position.x, projSpawner.transform.position.y + fixedYRocks);
        Instantiate(throwableObj, projSpawner.transform.position, projSpawner.transform.rotation);
        topAnimator.SetBool("isAttacking", false);
        projSpawner.transform.position = initialPosition;
    }

    public void activeBoss()
    {
        isBossActive = true;
        AudioManager.StartBossAudio();
    }

    private void registerHealth()
    {
        health = top.GetComponent<Health>();
        // register health delegate
        health.onDead += OnDead;
        health.onHit += OnHit;
    }

    private void OnHit(float damage)
    {
        GameManager.AddScore(damage);
        top.GetComponent<BlinkingSprite>().Play();

        // fasten if dying
        if (health.GetHealth() <= maxHealth / 4)
            chargingTime = .5f;
        else if (health.GetHealth() <= maxHealth / 3)
            chargingTime = .75f;
        else if (health.GetHealth() <= maxHealth / 2)
            chargingTime = 1f;
    }

    private void OnDead(float damage)
    {
        StartCoroutine(Explode());
        GameManager.PlayerWin();
        StopBossCoroutines();
    }

    private void HalfHealth()
    {
        chargingTime = 1f;
    }

    private void StopBossCoroutines()
    {
        StopAllCoroutines();
    }

    private IEnumerator Explode()
    {
        AudioManager.PlayMetalSlugDestroy3();
        //bottomAnimator.SetBool("isExploding", true);
        yield return new WaitForSeconds(1.75f);

        AudioManager.PlayMetalSlugDestroy1();
        //top.SetActive(false);
        bottomAnimator.SetBool("isDying", true);
    }
}
