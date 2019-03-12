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

    [Header("Music")]
    public AudioClip musicClip;         //The background music
    public AudioClip gameOverClip;      //Played once on game over

    [Header("Player")]
    public AudioClip marcoDeathClip;    //Marco Death Sound

    [Header("Effects")]
    public AudioClip normalShotClip;
    public AudioClip shotHitClip;
    public AudioClip grenadeHitClip;
    public AudioClip meleeHitClip;
    public AudioClip meleeTakeClip;
    public AudioClip collectibleGrabClip;

    [Header("Voice")]
    public AudioClip levelStart;

    [Header("Mixer Groups")]
    public AudioMixerGroup musicGroup;  //The music mixer group
    public AudioMixerGroup effectGroup;  //The sting mixer group
    public AudioMixerGroup enemyGroup; //The enemy mixer group
    public AudioMixerGroup playerGroup; //The player mixer group
    public AudioMixerGroup voiceGroup;  //The voice mixer group

    AudioSource musicSource;            //Reference to the generated music Audio Source
    AudioSource effectSource;            //Reference to the generated effect Audio Source
    AudioSource enemySource;            //Reference to the generated enemy Audio Source
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
        enemySource = gameObject.AddComponent<AudioSource>();
        playerSource = gameObject.AddComponent<AudioSource>();
        voiceSource = gameObject.AddComponent<AudioSource>();

        //Assign each audio source to its respective mixer group so that it is
        //routed and controlled by the audio mixer
        musicSource.outputAudioMixerGroup = musicGroup;
        effectSource.outputAudioMixerGroup = effectGroup;
        enemySource.outputAudioMixerGroup = enemyGroup;
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
        PlayLevelStartAudio();
    }

    public static void PlayLevelStartAudio()
    {
        //If there is no current AudioManager, exit
        if (current == null)
            return;

        //Play the initial level voice
        current.voiceSource.clip = current.levelStart;
        current.voiceSource.Play();
    }

    public static void PlayGameOverAudio()
    {
        //If there is no current AudioManager, exit
        if (current == null)
            return;

        //Set the clip for music audio, tell it not to loop, and then tell it to play
        current.musicSource.clip = current.gameOverClip;
        current.musicSource.loop = false;
        current.musicSource.Play();
    }

    public static void PlayDeathAudio()
    {
        //If there is no current AudioManager, exit
        if (current == null)
            return;

        //Set the clip for music audio, and then tell it to play
        current.playerSource.clip = current.marcoDeathClip;
        current.playerSource.Play();
    }

    public static void PlayNormalShotAudio()
    {
        //If there is no current AudioManager, exit
        if (current == null)
            return;

        //Don't overshadow the other sounds
        if (current.playerSource.clip != current.normalShotClip && current.playerSource.isPlaying)
            return;

        //Set the clip for music audio, and then tell it to play
        current.playerSource.clip = current.normalShotClip;
        current.playerSource.Play();
    }

    public static void PlayEnemyDeathAudio(AudioClip deathClip)
    {
        //If there is no current AudioManager, exit
        if (current == null)
            return;

        //Set the clip for music audio, and then tell it to play
        current.enemySource.clip = deathClip;
        current.enemySource.Play();
    }

    public static void PlayShotHitAudio()
    {
        //If there is no current AudioManager, exit
        if (current == null)
            return;

        //Set the clip for music audio, and then tell it to play
        current.playerSource.clip = current.shotHitClip;
        current.playerSource.Play();
    }

    public static void PlayGrenadeHitAudio()
    {
        //If there is no current AudioManager, exit
        if (current == null)
            return;

        //Set the clip for music audio, and then tell it to play
        current.playerSource.clip = current.grenadeHitClip;
        current.playerSource.Play();
    }

    public static void PlayMeleeHitAudio()
    {
        //If there is no current AudioManager, exit
        if (current == null)
            return;

        //Set the clip for music audio, and then tell it to play
        current.playerSource.clip = current.meleeHitClip;
        current.playerSource.Play();
    }

    public static void PlayMeleeTakeAudio()
    {
        //If there is no current AudioManager, exit
        if (current == null)
            return;

        //Set the clip for music audio, and then tell it to play
        current.playerSource.clip = current.meleeTakeClip;
        current.playerSource.Play();
    }
}
