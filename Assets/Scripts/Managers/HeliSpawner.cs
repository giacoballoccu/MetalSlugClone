using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeliSpawner : MonoBehaviour
{
    public int maxHeli = 5;
    public GameObject heliPrefab;
    public UnityEvent triggerCamera;

    public static List<GameObject> heliList = new List<GameObject>();
    private float secondsWait = 0;
    private bool isSpawned;
    private int killedHeli;

    /*void Start()
    {
        if (!GetComponent<BoxCollider2D>()) // spawn immediately if trigger collider is missing
        {
            isSpawned = true;
            Initialize();
        }
    }*/

    void OnTriggerEnter2D(Collider2D collider)
    {
        CheckAndSpawn(collider);
    }

    private void CheckAndSpawn(Collider2D collider)
    {
        if (!isSpawned && GameManager.IsPlayer(collider))
        {
            isSpawned = true;
            Initialize();
        }
    }

    void Initialize()
    {
        for (int i = 0; i < maxHeli; i++)
        {
            StartCoroutine(WaitHeli(i == 0));
            secondsWait += 1.5f;
        }
    }

    public void HeliDestroy(GameObject heli)
    {
        killedHeli++;
        heliList.Remove(heli);
        SetMainHeliShooter();
        CheckFinished();
    }

    public void SetMainHeliShooter()
    {
        if (heliList.Count > 0)
            SetFire(heliList[0]);
    }

    private void CheckFinished()
    {
        if (killedHeli >= maxHeli)
        {
            triggerCamera?.Invoke();
            Destroy(gameObject);
        }
    }

    private IEnumerator WaitHeli(bool first)
    {
        yield return new WaitForSeconds(secondsWait);
        GameObject heli = Instantiate(heliPrefab, transform.position, transform.rotation, transform);
        heli.GetComponent<HeliController>().RegisterSpawner(this);
        heliList.Add(heli);
        SetMainHeliShooter();
    }

    void SetFire(GameObject heli)
    {
        heli.GetComponent<HeliController>().SetFire(true);
    }
}
