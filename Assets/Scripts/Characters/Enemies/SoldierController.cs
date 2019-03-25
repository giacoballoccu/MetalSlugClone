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
    EnemyControl ec;

    private void Start()
    {
        anim = GetComponent<Animator>();
        ec = GetComponent<EnemyControl>();
    }

    void FixedUpdate()
    {
        FlipShoot();

        if (anim.GetBool("isAttacking_2"))
        {
            ThrowGranate();
        }
    }

    void ThrowGranate()
    {
        shotTime = shotTime + Time.deltaTime;

        if (shotTime > nextFire)
        {
            nextFire = shotTime + fireDelta;

            StartCoroutine(WaitGranate());

            nextFire = nextFire - shotTime;
            shotTime = 0.0f;
        }
    }

    void FlipShoot()
    {
        if (ec.facingRight)
        {
            //Fire right
            projSpawner.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            //Fire left
            projSpawner.transform.localEulerAngles = new Vector3(0, 0, 180);
        }
    }

    private IEnumerator WaitGranate()
    {
        yield return new WaitForSeconds(0.1f);
        BulletManager.GetEnemyGrenadePool().Spawn(projSpawner.transform.position, projSpawner.transform.rotation);
        yield return new WaitForSeconds(0.15f);
    }
}
