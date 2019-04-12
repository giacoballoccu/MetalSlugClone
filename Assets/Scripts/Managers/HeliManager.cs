using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliManager : MonoBehaviour
{
    public int maxHeli = 5;
    public GameObject heliPrefab;

    public static List<GameObject> heliList = new List<GameObject>();
    private float secondsWait = 0;
    private bool isSpawned;

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
        heliList.Remove(gameObject);
        SetMainHeliShooter();
    }

    static public void SetMainHeliShooter()
    {
        if (heliList.Count > 0)
            SetFire(heliList[0]);
    }

    private IEnumerator WaitHeli(bool first)
    {
        yield return new WaitForSeconds(secondsWait);
        GameObject heli = Instantiate(heliPrefab, transform.position, transform.rotation, transform);
        heliList.Add(heli);
        SetMainHeliShooter();
    }

    static bool IsAlive(GameObject heli)
    {
        if (heli == null)
            return false;

        return heli.GetComponent<Health>().IsAlive();
    }

    static void SetFire(GameObject heli)
    {
        heli.GetComponent<HeliController>().SetFire(true);
    }
}
