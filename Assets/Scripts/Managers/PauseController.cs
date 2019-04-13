using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class PauseController : MonoBehaviour
{
    public GameObject canvas;

    [Header("Settings")]
    public TextMeshProUGUI bgmText;
    public TextMeshProUGUI sfxText;
    public TextMeshProUGUI bgmTextCounter;
    public TextMeshProUGUI sfxTextCounter;
    public TextMeshProUGUI godModeText;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            showMenu();
        }
    }

    public void Open()
    {
        RefreshAudioText();
        showMenu();
    }

    public void Exit()
    {
        GameManager.PauseExit();
    }

    public void Back()
    {
        canvas.SetActive(false);
        Time.timeScale = 1;
    }

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

    public void ToggleGodMode()
    {
        if (GameManager.ToggleGodMode())
            godModeText.SetText("GOD MODE ON");
        else
            godModeText.SetText("GOD MODE OFF");
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

    void showMenu()
    {
        canvas.SetActive(!canvas.activeSelf);
        Time.timeScale = (Time.timeScale + 1) % 2;
    }
}
