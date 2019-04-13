// This script is a Manager that controls the the flow and control of the game. It keeps
// track of player data (score, total game time) and interfaces with
// the UI Manager. All game commands are issued through the static methods of this class

using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //This class holds a static reference to itself to ensure that there will only be
    //one in existence. This is often referred to as a "singleton" design pattern. Other
    //scripts access this one through its public static methods
    static GameManager current;

    public enum Difficulty {Easy = 1, Medium = 2, Hard = 3 }
    public enum Missions { Home = 0, Mission1, Mission2, Mission3, Mission3Boss }

    float totalGameTime;                        //Length of the total game time
    bool isGameOver;                            //Is the game currently over?
    int score = 0;
    int bombs = 200;
    int heavyMachineAmmo = 0;
    Difficulty difficulty = Difficulty.Medium;
    float bgmAudio = 1f;
    float sfxAudio = 1f;
    Missions currentMission = Missions.Home;
    float mission1Points = 0f;
    float mission2Points = 0f;
    float mission3Points = 0f;

    [Header("Layers")]
    public LayerMask enemyLayer;
    public LayerMask buildingLayer;
    public LayerMask walkableLayer;
    public LayerMask playerLayer;

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

        //Persist this object between scene reloads
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LoadSettings();
        LoadRecords();
        SaveRecords();
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

    private void SaveSettings()
    {
        Settings settings = SaveManager.GetSettings();
        if (settings == null)
            settings = new Settings();
        settings.bgmVolume = GetBgmAudio();
        settings.sfxVolume = GetSfxAudio();
        SaveManager.SetSettings(settings);
    }

    private void LoadSettings()
    {
        Settings settings = SaveManager.GetSettings();
        if (settings != null)
        {
            SetBgmAudio(settings.bgmVolume);
            SetSfxAudio(settings.sfxVolume);
            AudioManager.RefreshAudioVolume();
        }
    }

    private void SaveRecords()
    {
        Records records = SaveManager.GetRecords();
        if (records == null)
            records = new Records();
        records.mission1Points = GetMission1Points();
        records.mission2Points = GetMission2Points();
        records.mission3Points = GetMission3Points();
        SaveManager.SetRecords(records);
    }

    private void LoadRecords()
    {
        Records records = SaveManager.GetRecords();
        if (records != null)
        {
            SetMission1Points(records.mission1Points);
            SetMission2Points(records.mission2Points);
            SetMission3Points(records.mission3Points);
            AudioManager.RefreshAudioVolume();
        }
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
            return 10;

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

    public static int GetHeavyMachineAmmo()
    {
        //If there is no current Game Manager, return 0
        if (current == null)
            return 100;

        //Return the state of the game
        return current.heavyMachineAmmo;
    }

    public static void SetHeavyMachineAmmo(int ammo)
    {
        //If there is no current Game Manager, return 0
        if (current == null)
            return;

        //Return the state of the game
        current.heavyMachineAmmo = ammo;
    }

    public static void RemoveHeavyMachineAmmo()
    {
        //If there is no current Game Manager, exit
        if (current == null)
            return;

        current.heavyMachineAmmo -= 3;
        if (current.heavyMachineAmmo < 0)
            current.heavyMachineAmmo = 0;
        UIManager.UpdateAmmoUI();
    }

    public static void AddAmmo()
    {
        //If there is no current Game Manager, exit
        if (current == null)
            return;

        current.bombs += 10;
        current.heavyMachineAmmo += 120;

        UIManager.UpdateBombsUI();
        UIManager.UpdateAmmoUI();
    }

    public static void RechargAmmoMG()
    {
        //If there is no current Game Manager, exit
        if (current == null)
            return;

        current.heavyMachineAmmo += 120;

        UIManager.UpdateAmmoUI();
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

        current.StartCoroutine(current.WaitHome());
    }

    public static void PlayerWin()
    {
        //If there is no current Game Manager, exit
        if (current == null)
            return;

        UIManager.DisplayWinText();
        AudioManager.PlayLevelCompleteAudio();
        AudioManager.PlayGameOverAudio();

        current.isGameOver = true;

        current.currentMission = (Missions) SceneManager.GetActiveScene().buildIndex;

        // Update json score
        if (current.currentMission == Missions.Mission1 && current.score > GetMission1Points())
        {
            SetMission1Points(current.score);
            SaveManager.SetRecords(new Records(current.score, GetMission2Points(), GetMission3Points()));
        }
        else if (current.currentMission == Missions.Mission2 && current.score > GetMission2Points())
        {
            SetMission2Points(current.score);
            SaveManager.SetRecords(new Records(GetMission1Points(), current.score, GetMission3Points()));
        }
        else if (current.currentMission == Missions.Mission3Boss && current.score > GetMission3Points())
        {
            SetMission3Points(current.score);
            SaveManager.SetRecords(new Records(GetMission1Points(), GetMission2Points(), current.score));
        }

        if (current.currentMission >= Missions.Mission3Boss)
        {
            current.currentMission = Missions.Home;
        } else
        {
            current.currentMission++;
        }

        current.StartCoroutine(current.WaitNextMission());
    }

    public static LayerMask GetBuildingLayer()
    {
        //If there is no current Game Manager, exit
        if (current == null)
            return 0;

        return current.buildingLayer;
    }

    public static GameObject GetPlayer() // not cached
    {
        if (current == null)
            return null;
        return GameObject.FindGameObjectWithTag("Player");
    }

    public static GameObject GetPlayer(GameObject player)
    {
        if (current == null)
            return null;
        if (player.GetComponent<PlayerController>()) // return itself
            return player;
        else if (player.transform.parent.gameObject.GetComponent<PlayerController>()) // return parent
            return player.transform.parent.gameObject;
        return GameObject.FindGameObjectWithTag("Player"); // return uncached finded by tag
    }

    public static GameObject GetPlayer(Collider2D collider)
    {
        return GetPlayer(collider.gameObject);
    }

    public static GameObject GetPlayer(Collision2D collision)
    {
        return GetPlayer(collision.collider);
    }

    public static GameObject GetRunningTarget() // not cached
    {
        //If there is no current Game Manager, exit
        if (current == null)
            return null;

        return GameObject.FindGameObjectWithTag("RunningTarget");
    }

    public static bool IsPlayer(GameObject player)
    {
        //return (GetPlayerLayer() & (1<<player.layer)) != 0;
        return GetPlayerLayer() == (1<<player.layer);
    }

    public static bool IsPlayer(Collider2D collider)
    {
        return IsPlayer(collider.gameObject);
    }

    public static bool IsPlayer(Collision2D collision)
    {
        return IsPlayer(collision.collider);
    }

    public static LayerMask GetEnemyLayer()
    {
        if (current == null)
            return LayerMask.NameToLayer("Enemy");
        return current.enemyLayer;
    }

    public static LayerMask GetWalkableLayer()
    {
        if (current == null)
            return LayerMask.NameToLayer("Walkable");
        return current.walkableLayer;
    }

    public static int GetPlayerLayer()
    {
        if (current == null)
            return LayerMask.NameToLayer("Player");
        return current.playerLayer.value;
    }

    public static LayerMask GetDestructibleLayer()
    {
        if (current == null)
            return 0;

        return GetEnemyLayer() + GetBuildingLayer();
    }

    public static bool CanTriggerThrowable(Collider2D collider)
    {
        if (current == null)
            return false;
        var tag = collider.tag;
        return tag == "Enemy" || tag == "Building" || tag == "Walkable" || IsPlayer(collider) || tag == "Roof" || tag == "Bridge" || tag == "EnemyBomb";
    }

    public void SetDifficultyMode(int difficulty)
    {
        SetDifficultyMode((Difficulty)difficulty);
    }

    public static void SetDifficultyMode(Difficulty difficulty)
    {
        if (current == null)
            return;
        current.difficulty = difficulty;
    }

    public static Difficulty GetDifficultyMode()
    {
        if (current == null)
            return 0;
        return current.difficulty;
    }

    public static void SetBgmAudio(float bgmAudio, bool save = false)
    {
        if (current == null)
            return;
        current.bgmAudio = bgmAudio;
        if (save)
            current.SaveSettings();
    }

    public static float GetBgmAudio()
    {
        if (current == null)
            return 0f;
        return current.bgmAudio;
    }

    public static void SetSfxAudio(float sfxAudio, bool save = false)
    {
        if (current == null)
            return;
        current.sfxAudio = sfxAudio;
        if (save)
            current.SaveSettings();
    }

    public static float GetSfxAudio()
    {
        if (current == null)
            return 0f;
        return current.sfxAudio;
    }

    public static void SetMission1Points(float points)
    {
        if (current == null)
            return;
        current.mission1Points = points;
    }

    public static float GetMission1Points()
    {
        if (current == null)
            return 0f;
        return current.mission1Points;
    }

    public static void SetMission2Points(float points)
    {
        if (current == null)
            return;
        current.mission2Points = points;
    }

    public static float GetMission2Points()
    {
        if (current == null)
            return 0f;
        return current.mission2Points;
    }

    public static void SetMission3Points(float points)
    {
        if (current == null)
            return;
        current.mission3Points = points;
    }

    public static float GetMission3Points()
    {
        if (current == null)
            return 0f;
        return current.mission3Points;
    }

    public static void GameReset()
    {
        if (!current)
            return;
        Time.timeScale = 1;
        current.isGameOver = false;
        current.score = 0;
        current.totalGameTime = 0;
    }

    public static void PauseExit()
    {
        if (!current)
            return;
        LoadHome();
    }

    public static void LoadHome()
    {
        SceneManager.LoadScene((int)Missions.Home);
        GameReset();
    }

    public static void LoadNextMission()
    {
        // currentMission is updated in the PlayerWin method
        SceneManager.LoadScene((int)current.currentMission);
        GameReset();
    }

    public static bool CanTriggerEnemyBombs(string tag)
    {
        //If there is no current Game Manager, exit
        if (current == null)
            return false;

        return tag == "Player" || tag == "Walkable" || tag == "Marco Boat" || tag == "Bridge";
    }

    private IEnumerator WaitHome()
    {
        yield return new WaitForSeconds(7f);
        LoadHome();
    }

    private IEnumerator WaitNextMission()
    {
        yield return new WaitForSeconds(7f);
        LoadNextMission();
    }
}
