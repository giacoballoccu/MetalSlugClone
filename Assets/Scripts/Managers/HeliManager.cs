using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliManager : MonoBehaviour
{
    public static int nHeli = 5;
    public GameObject heli;

    static ArrayList heliList = new ArrayList();
    private float secondsWait = 0;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        for (int i = 0; i < nHeli; i++)
        {
            StartCoroutine(WaitHeli(i == 0));
            secondsWait += 1.5f;
        }
    }

    private IEnumerator WaitHeli(bool first)
    {
        yield return new WaitForSeconds(secondsWait);
        GameObject instantiated = Instantiate(heli, transform.position, transform.rotation, transform);
        heliList.Add(instantiated);

        if (first)
            SetFire(instantiated);
    }

    public static void HeliKilled()
    {
        for (int i = 0; i < nHeli; i++)
        {
            try
            {
                if (IsAlive((GameObject)heliList[i]))
                {
                    SetFire((GameObject)heliList[i]);
                    break;
                }
            }
            catch (System.IndexOutOfRangeException /*io*/)
            {

            }
        }
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
