using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliManager : MonoBehaviour
{
    static public int maxHeli = 5;
    public GameObject heliPrefab;

    public static List<GameObject> heliList = new List<GameObject>();
    private float secondsWait = 0;
    private bool isSpawned;
    static private int killedHeli;
    static HeliManager instance;

    void Awake()
    {
        instance = this;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        CheckAndSpawn(collider);
    }

    private void CheckAndSpawn(Collider2D collider)
    {
        if (!isSpawned && collider.CompareTag("Player"))
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

    static public void HeliDestroy(GameObject gameObject)
    {
        killedHeli++;
        heliList.Remove(gameObject);
        SetMainHeliShooter();
        CheckFinished();
    }

    static public void SetMainHeliShooter()
    {
        if (heliList.Count > 0)
            SetFire(heliList[0]);
    }

    private static void CheckFinished()
    {
        if (killedHeli >= maxHeli)
        {
            CameraManager.AfterFirstHeli();
            if (instance)
                Destroy(instance.gameObject);
        }
    }

    private IEnumerator WaitHeli(bool first)
    {
        yield return new WaitForSeconds(secondsWait);
        GameObject heli = Instantiate(heliPrefab, transform.position, transform.rotation, transform);
        heliList.Add(heli);
        SetMainHeliShooter();
    }

    static void SetFire(GameObject heli)
    {
        heli.GetComponent<HeliController>().SetFire(true);
    }
}
