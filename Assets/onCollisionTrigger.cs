using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onCollisionTrigger : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            animator.SetBool("isSpawning", true);
        }
    }
}
