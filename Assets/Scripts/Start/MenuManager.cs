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

    public TMPro.TextMeshProUGUI startText;

    [Header("Menu Groups")]
    public Image start;
    public Image choose;
    public Image chooseMode;
    public Image stats;
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

    void Start()
    {
        //Generate the Audio Source "channels" for our game's audio
        musicSource = gameObject.AddComponent<AudioSource>();
        effectSource = gameObject.AddComponent<AudioSource>();

        StartCoroutine("blinkStart");
    }

    // Start is called before the first frame update
    public void PressStart()
    {
        choose.gameObject.SetActive(true);
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
}
