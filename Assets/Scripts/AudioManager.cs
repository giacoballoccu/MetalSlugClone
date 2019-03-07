// This script is a Manager that controls all of the audio for the project. All audio
// commands are issued through the static methods of this class. Additionally, this 
// class creates AudioSource "channels" at runtime and manages them

using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    //This class holds a static reference to itself to ensure that there will only be
    //one in existence. This is often referred to as a "singleton" design pattern. Other
    //scripts access this one through its public static methods
    static AudioManager current;

    [Header("Audio Clips")]
    public AudioClip musicClip;         //The background music

    [Header("Mixer Groups")]
    public AudioMixerGroup musicGroup;  //The music mixer group
    public AudioMixerGroup effectGroup;  //The sting mixer group
    public AudioMixerGroup playerGroup; //The player mixer group
    public AudioMixerGroup voiceGroup;  //The voice mixer group

    AudioSource musicSource;            //Reference to the generated music Audio Source
    AudioSource effectSource;            //Reference to the generated effect Audio Source
    AudioSource playerSource;           //Reference to the generated player Audio Source
    AudioSource voiceSource;            //Reference to the generated voice Audio Source

    void Awake()
    {
        //If an AudioManager exists and it is not this...
        if (current != null && current != this)
        {
            //...destroy this. There can be only one AudioManager
            Destroy(gameObject);
            return;
        }

        //This is the current AudioManager and it should persist between scene loads
        current = this;
        DontDestroyOnLoad(gameObject);

        //Generate the Audio Source "channels" for our game's audio
        musicSource = gameObject.AddComponent<AudioSource>();
        effectSource = gameObject.AddComponent<AudioSource>();
        playerSource = gameObject.AddComponent<AudioSource>();
        voiceSource = gameObject.AddComponent<AudioSource>();

        //Assign each audio source to its respective mixer group so that it is
        //routed and controlled by the audio mixer
        musicSource.outputAudioMixerGroup = musicGroup;
        effectSource.outputAudioMixerGroup = effectGroup;
        playerSource.outputAudioMixerGroup = playerGroup;
        voiceSource.outputAudioMixerGroup = voiceGroup;

        //Being playing the game audio
        StartLevelAudio();
    }

    void StartLevelAudio()
    {
        //Set the clip for music audio, tell it to loop, and then tell it to play
        current.musicSource.clip = current.musicClip;
        current.musicSource.loop = true;
        current.musicSource.Play();
    }
}
