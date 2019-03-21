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
    public TextMeshProUGUI startText;

    [Header("Menu Groups")]
    public GameObject start;
    public GameObject choose;
    public GameObject chooseMode;
    public GameObject settings;
    public GameObject stats;
    public GameObject missionMode;

    [Header("Difficulty Texts")]
    public TextMeshProUGUI easy;
    public TextMeshProUGUI medium;
    public TextMeshProUGUI hard;

    [Header("Missions viewer")]
    public Image missionViewer;
    public List<Sprite> missionSprites;

    [Header("Settings")]
    public TextMeshProUGUI bgmText;
    public TextMeshProUGUI sfxText;
    public TextMeshProUGUI bgmTextCounter;
    public TextMeshProUGUI sfxTextCounter;

    private GameObject currentMenu;

    void Start()
    {
        RefreshAudioText();
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
        AudioManager.PlayMenuSelect();

        //Set the clip for music audio, tell it to loop, and then tell it to play
        AudioManager.PlayMenuBGM();
    }

    public void PressMainMission()
    {
        chooseMode.gameObject.SetActive(true);

        //Set the clip for effect audio
        AudioManager.PlayMenuSelect();
    }

    public void PressMissionMode()
    {
        missionMode.gameObject.SetActive(true);

        //Set the clip for effect audio
        AudioManager.PlayMenuSelect();
    }

    public void PressSettings()
    {
        settings.gameObject.SetActive(true);
        choose.gameObject.SetActive(false);
        currentMenu = settings;

        //Set the clip for effect audio
        AudioManager.PlayMenuSelect();
    }

    public void PressStats()
    {
        stats.gameObject.SetActive(true);
        choose.gameObject.SetActive(false);
        currentMenu = stats;

        //Set the clip for effect audio
        AudioManager.PlayMenuSelect();
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
        SceneManager.LoadScene(GetMissionIndex() + 1);
    }
    /* End mission mode selection */

    /* Start settings */
    public void SetBgmCounterPressed()
    {
        //Click color
        bgmText.color = new Color32(255, 255, 255, 255);
        sfxText.color = new Color32(255, 141, 0, 255);
        bgmTextCounter.color = new Color32(255, 255, 255, 255);
        sfxTextCounter.color = new Color32(255, 141, 0, 255);

        float cnt = GameManager.GetBgmAudio();
        cnt += .1f;
        if (cnt >= 1.1f)
            cnt = 0f;
        GameManager.SetBgmAudio(cnt, true);

        AudioManager.RefreshAudioVolume();
        RefreshAudioText();
    }

    public void SetSfxCounterPressed()
    {
        //Click color
        bgmText.color = new Color32(255, 141, 0, 255);
        sfxText.color = new Color32(255, 255, 255, 255);
        bgmTextCounter.color = new Color32(255, 141, 0, 255);
        sfxTextCounter.color = new Color32(255, 255, 255, 255);

        float cnt = GameManager.GetSfxAudio();
        cnt += .1f;
        if (cnt >= 1.1f)
            cnt = 0f;
        GameManager.SetSfxAudio(cnt, true);

        AudioManager.RefreshAudioVolume();
        RefreshAudioText();
    }

    void RefreshAudioText()
    {
        float bgmCnt = GameManager.GetBgmAudio();
        if (bgmCnt < 0.1f)
            bgmTextCounter.SetText("OFF");
        else
            bgmTextCounter.SetText(Math.Round(bgmCnt * 10).ToString());

        float sfxCnt = GameManager.GetSfxAudio();
        if (sfxCnt < 0.1f)
            sfxTextCounter.SetText("OFF");
        else
            sfxTextCounter.SetText(Math.Round(sfxCnt * 10).ToString());
    }
    /* End settings */
}
