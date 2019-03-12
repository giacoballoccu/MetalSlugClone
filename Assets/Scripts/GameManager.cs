// This script is a Manager that controls the the flow and control of the game. It keeps
// track of player data (score, total game time) and interfaces with
// the UI Manager. All game commands are issued through the static methods of this class

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //This class holds a static reference to itself to ensure that there will only be
    //one in existence. This is often referred to as a "singleton" design pattern. Other
    //scripts access this one through its public static methods
    static GameManager current;

    float totalGameTime;                        //Length of the total game time
    bool isGameOver;                            //Is the game currently over?
    int score = 0;
    int bombs = 200;

    [Header("Layers")]
    public LayerMask enemyLayer;
    public LayerMask walkableLayer;

    void Awake()
    {
        //If a Game Manager exists and this isn't it...
        if (current != null && current != this)
        {
            //...destroy this and exit. There can only be one Game Manager
            Destroy(gameObject);
            return;
        }

        //Set this as the current game manager
        current = this;

        //Persis this object between scene reloads
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        //If the game is over, exit
        if (isGameOver)
            return;

        //Update the total game time and tell the UI Manager to update
        totalGameTime += Time.deltaTime;
        // UIManager.UpdateTimeUI(totalGameTime); // todo implement or delete
    }

    public static void AddScore(float amount)
    {
        AddScore((int)amount);
    }

    public static void AddScore(int amount)
    {
        //If there is no current Game Manager, exit
        if (current == null)
            return;

        current.score += amount;
        UIManager.UpdateScoreUI();
    }

    public static int GetScore()
    {
        //If there is no current Game Manager, return 0
        if (current == null)
            return 0;

        //Return the state of the game
        return current.score;
    }

    public static int GetBombs()
    {
        //If there is no current Game Manager, return 0
        if (current == null)
            return 2;

        //Return the state of the game
        return current.bombs;
    }

    public static void RemoveBomb()
    {
        //If there is no current Game Manager, exit
        if (current == null)
            return;

        current.bombs--;
        UIManager.UpdateBombsUI();
    }

    public static bool IsGameOver()
    {
        //If there is no current Game Manager, return false
        if (current == null)
            return false;

        //Return the state of the game
        return current.isGameOver;
    }

    public static void PlayerDied()
    {
        //If there is no current Game Manager, exit
        if (current == null)
            return;

        //The game is now over
        current.isGameOver = true;

        //Tell UI Manager to show the game over text and tell the Audio Manager to play
        //game over audio
        UIManager.DisplayGameOverText();
        AudioManager.PlayGameOverAudio();
    }

    public static LayerMask GetEnemyLayer()
    {
        //If there is no current Game Manager, exit
        if (current == null)
            return 0;

        return current.enemyLayer;
    }

    public static LayerMask GetWalkableLayer()
    {
        //If there is no current Game Manager, exit
        if (current == null)
            return 0;

        return current.walkableLayer;
    }

    public static bool CanTriggerGrenade(string tag)
    {
        //If there is no current Game Manager, exit
        if (current == null)
            return false;

        return tag == "Enemy" || tag == "Building" || tag == "Boat" || tag == "Terrain" || tag == "Walkable"
    }
}
