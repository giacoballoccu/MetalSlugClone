using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private float chargingTime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAfter());
    }

 private IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(chargingTime);

        Destroy(gameObject);
    }

}
