using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public AudioClip charSelect; // char selection
    public AudioClip marco; // marco chosen
    public AudioClip menuSound; // menu sound
    public AudioClip preselect; // any button
    public AudioClip select; // press start

    public GameManager gameManager;

    public TMPro.TextMeshProUGUI startText;

    [Header("Menu Groups")]
    public GameObject start;
    public GameObject choose;
    public GameObject chooseMode;
    public GameObject settings;
    public GameObject stats;
    public GameObject missionMode;

    [Header("Mixer Groups")]
    public AudioMixerGroup musicGroup;  //The music mixer group
    public AudioMixerGroup effectGroup;  //The effect mixer group

    AudioSource musicSource;            //Reference to the generated music Audio Source
    AudioSource effectSource;            //Reference to the generated effect Audio Source

    [Header("Difficulty Texts")]
    public TMPro.TextMeshProUGUI easy;
    public TMPro.TextMeshProUGUI medium;
    public TMPro.TextMeshProUGUI hard;

    [Header("Missions viewer")]
    public Image missionViewer;
    public List<Sprite> missionSprites;

    [Header("Settings")]
    public TMPro.TextMeshProUGUI bgText;
    public TMPro.TextMeshProUGUI fsxText;
    public TMPro.TextMeshProUGUI bgTextCounter;
    public TMPro.TextMeshProUGUI fsxTextCounter;

    //Audio settings
    private bool isBgCounterPressed = false;
    private bool isFsxCounterPressed = false;

    private GameObject currentMenu;

    void Start()
    {
        //Generate the Audio Source "channels" for our game's audio
        musicSource = gameObject.AddComponent<AudioSource>();
        effectSource = gameObject.AddComponent<AudioSource>();

        StartCoroutine("blinkStart");
    }

    private IEnumerator blinkStart()
    {
        while (true)
        {
            while (startText.alpha > 0f)
            {
                startText.alpha -= 0.1f;
                yield return new WaitForSeconds(0.05f);
            }
            while (startText.alpha < 1f)
            {
                startText.alpha += 0.1f;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    public void Back()
    {
        currentMenu.SetActive(false);

        if(currentMenu == settings || currentMenu == stats)
        {
            choose.gameObject.SetActive(true);
        }
    }

    // Start is called before the first frame update
    public void PressStart()
    {
        choose.gameObject.SetActive(true);
        start.gameObject.SetActive(false);
        StopCoroutine("blinkStart");

        //Set the clip for effect audio
        effectSource.clip = select;
        effectSource.Play();

        //Set the clip for music audio, tell it to loop, and then tell it to play
        musicSource.clip = menuSound;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PressMainMission()
    {
        chooseMode.gameObject.SetActive(true);

        //Set the clip for effect audio
        effectSource.clip = select;
        effectSource.Play();
    }

    public void PressMissionMode()
    {
        missionMode.gameObject.SetActive(true);

        //Set the clip for effect audio
        effectSource.clip = select;
        effectSource.Play();
    }

    public void PressSettings()
    {
        settings.gameObject.SetActive(true);
        choose.gameObject.SetActive(false);
        currentMenu = settings;

        //Set the clip for effect audio
        effectSource.clip = select;
        effectSource.Play();
    }

    public void PressStats()
    {
        stats.gameObject.SetActive(true);
        choose.gameObject.SetActive(false);
        currentMenu = stats;

        //Set the clip for effect audio
        effectSource.clip = select;
        effectSource.Play();
    }

    public void SetSelectedDifficulty(int difficulty)
    {
        Color notSelected = new Color32(170, 170, 171, 255);
        Color selected = new Color32(238, 0, 0, 255);

        //Reset color
        easy.color = notSelected;
        medium.color = notSelected;
        hard.color = notSelected;

        //Selected color
        switch (difficulty)
        {
            case 1:
                easy.color = selected;
                break;
            case 2:
                medium.color = selected;
                break;
            case 3:
                hard.color = selected;
                break;
        }
    }

    /* Start mission mode selection */
    public void BackMission()
    {
        int currentMission = GetMissionIndex();

        //Can go back only if the mission exists and if the current mission is not the first
        if (currentMission != -1 && currentMission != 0)
        {
            missionViewer.sprite = missionSprites[--currentMission];
        }
    }

    public void NextMission()
    {
        int currentMission = GetMissionIndex();

        //Can go back only if the mission exists and if the current mission is not the last
        if (currentMission != -1 && currentMission != missionSprites.Count-1)
        {
            missionViewer.sprite = missionSprites[++currentMission];
        }
    }

    public int GetMissionIndex()
    {
        return missionSprites.IndexOf(missionViewer.sprite);
    }

    public void startMission()
    {
        SceneManager.LoadScene(GetMissionIndex());
    }
    /* End mission mode selection */

    /* Start settings */

    public void SetBgCounterPressed()
    {
        //Click color
        bgText.color = new Color32(255, 255, 255, 255);
        fsxText.color = new Color32(255, 141, 0, 255);
        bgTextCounter.color = new Color32(255, 255, 255, 255);
        fsxTextCounter.color = new Color32(255, 141, 0, 255);

        isBgCounterPressed = true;
        isFsxCounterPressed = false;

        int bgCounter = gameManager.GetBgAudio();

        if (isBgCounterPressed)
        {
            bgCounter++;

            if (bgCounter > 10)
            {
                bgCounter = 0;
            }

            if (bgCounter == 0)
            {
                bgTextCounter.SetText("OFF");
            }
            else
            {
                bgTextCounter.SetText(bgCounter.ToString());
            }

            gameManager.SetBgAudio(bgCounter);
        }
    }

    public void SetFsxCounterPressed()
    {
        //Click color
        bgText.color = new Color32(255, 141, 0, 255);
        fsxText.color = new Color32(255, 255, 255, 255);
        bgTextCounter.color = new Color32(255, 141, 0, 255);
        fsxTextCounter.color = new Color32(255, 255, 255, 255);

        isBgCounterPressed = false;
        isFsxCounterPressed = true;

        int fsxCounter = gameManager.GetFsxAudio();

        if (isFsxCounterPressed)
        {
            fsxCounter++;

            if (fsxCounter > 10)
            {
                fsxCounter = 0;
            }

            if (fsxCounter == 0)
            {
                fsxTextCounter.SetText("OFF");
            }
            else
            {
                fsxTextCounter.SetText(fsxCounter.ToString());
            }

            gameManager.SetFsxAudio(fsxCounter);
        }
    }

    /* End settings */
}
