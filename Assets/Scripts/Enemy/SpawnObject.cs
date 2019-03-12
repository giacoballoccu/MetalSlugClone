using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{

    public GameObject objectToSpawn;
    public GameObject follow;
    public int quantity = 1;

    // Start is called before the first frame update
    void Start()
    {
        /* todo annihilate this rubbish
        for (; quantity > 0; quantity--)
        {
            if (follow != null)
            {
                objectToSpawn.GetComponent<EnemyControl>().setFollow(follow);
            }
            Instantiate(objectToSpawn, new Vector2(transform.position.x-(quantity/8f), transform.position.y), transform.rotation);
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
