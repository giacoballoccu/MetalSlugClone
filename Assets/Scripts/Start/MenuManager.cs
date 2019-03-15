using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour
{
    public AudioClip charSelect; // char selection
    public AudioClip marco; // marco chosen
    public AudioClip menuSound; // menu sound
    public AudioClip preselect; // any button
    public AudioClip select; // press start
    public Image start;
    public Image menu;

    [Header("Mixer Groups")]
    public AudioMixerGroup musicGroup;  //The music mixer group
    public AudioMixerGroup effectGroup;  //The effect mixer group

    AudioSource musicSource;            //Reference to the generated music Audio Source
    AudioSource effectSource;            //Reference to the generated effect Audio Source

    void Start()
    {
        //Generate the Audio Source "channels" for our game's audio
        musicSource = gameObject.AddComponent<AudioSource>();
        effectSource = gameObject.AddComponent<AudioSource>();
    }

    // Start is called before the first frame update
    public void PressStart()
    {
        menu.gameObject.SetActive(true);

        //Set the clip for effect audio
        effectSource.clip = select;
        effectSource.Play();

        //Set the clip for music audio, tell it to loop, and then tell it to play
        musicSource.clip = menuSound;
        musicSource.loop = true;
        musicSource.Play();
    }
}
