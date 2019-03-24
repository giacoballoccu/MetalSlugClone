using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierController : MonoBehaviour
{
    [Header("Time shoot")]
    private float shotTime = 0.0f;
    public float fireDelta = 0.3f;
    private float nextFire = 0.2f;

    [Header("Granate")]
    public GameObject projSpawner;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (anim.GetBool("isAttacking_2"))
        {
            ThrowGranate();
        }
    }

    void ThrowGranate()
    {
        if (shotTime > nextFire)
        {
            nextFire = shotTime + fireDelta;

            StartCoroutine(WaitGranate());

            nextFire = nextFire - shotTime;
            shotTime = 0.0f;
        }
    }

    private IEnumerator WaitGranate()
    {
        yield return new WaitForSeconds(0.1f);
        BulletManager.GetGrenadePool().Spawn(projSpawner.transform.position, projSpawner.transform.rotation);
        yield return new WaitForSeconds(0.15f);
    }
}
