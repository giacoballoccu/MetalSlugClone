using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class toBoss : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.IsPlayer(collision))
        {
            GameManager.LoadScene((int)GameManager.Missions.Mission3Boss, true);
        }
    }
}
