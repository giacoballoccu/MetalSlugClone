using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenSpawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoints;
    public float spawnDelay;
    private int killCount;
    private int totalKillCount;

    public delegate void OnVoidEvent();    public OnVoidEvent onFinish; // when all the mobs are dead (todo make as event)
    private bool isFinished;

    void OnKill(float damage) // triggered by child.gameObject.health.onKill+=OnKill
    {
        if (isFinished)
            return;

        killCount += 1;
        if (killCount >= spawnPoints.childCount)
        {
            isFinished = true;
            onFinish?.Invoke();
        }
    }
    
    public void Trigger() // called by other scripts to start spawning
    {
        StartCoroutine("Spawn");
    }

    private IEnumerator Spawn()
    {
        foreach (Transform point in spawnPoints)
        {
            yield return new WaitForSeconds(spawnDelay);
            GameObject mob = Instantiate(enemyPrefab, point.position, point.rotation);
            mob.GetComponent<Health>().onDead += OnKill;
        }
    }
}
