using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMixer : MonoBehaviour
{
    public static MusicMixer instance;

    [Range(0f, 1f)]
    [SerializeField] float volume = 1;
    // [SerializeField] AudioClip fullSong;
    // [SerializeField] AudioClip startClip;
    [SerializeField] AudioClip lowClip;
    [SerializeField] AudioClip highClip;
    [SerializeField] AudioClip[] noteClips;
    // [SerializeField] AudioClip endClip;
    [SerializeField] float segmentLength = 1.6f;

    private AudioSource musicSource;
    private AudioSource noteSource;

    private float switchTime = -1f;
    private string switchName;

    private int nextNote = 0;
    private int noteDirection = 1;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Length < 2)
        {
            Debug.Log("MusicMixer needs 2 audiosources");
            return;
        }
        musicSource = audioSources[0];
        noteSource = audioSources[1];
        StartMusic();
    }

    void Update()
    {
        musicSource.volume = volume;

        if (switchTime >= 0f && musicSource.time >= switchTime)
        {
            switchTime = -1f;
            musicSource.Stop();
            switch (switchName)
            {
                case "low":
                    musicSource.clip = lowClip;
                    musicSource.loop = true;
                    musicSource.Play();
                    break;
                case "high":
                    musicSource.clip = highClip;
                    musicSource.loop = true;
                    musicSource.Play();
                    break;
                    // case "end":
                    //     musicSource.clip = endClip;
                    //     musicSource.loop = false;
                    //     musicSource.Play();
                    //     break;
            }
        }
    }

    private float CalculateSwitchTime()
    {
        float clipLength = musicSource.clip.length;
        float currentTime = musicSource.time;
        float proposedSwitchTime = segmentLength * Mathf.Ceil(currentTime / segmentLength);
        return proposedSwitchTime < clipLength ? proposedSwitchTime : 0f;
    }

    // public void PlayFullSong() {
    //     musicSource.clip = fullSong;
    //     musicSource.loop = true;
    //     musicSource.Play();
    // }

    public void StartMusic()
    {
        // switchTime = startClip.length;
        // switchName = "low";
        musicSource.Stop();
        // musicSource.clip = startClip;

        musicSource.clip = lowClip;
        musicSource.volume = volume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void QueueLow()
    {
        if (switchName == "low")
        {
            return;
        }
        switchTime = CalculateSwitchTime();
        switchName = "low";
    }

    public void QueueHigh()
    {
        if (switchName == "high")
        {
            return;
        }
        switchTime = CalculateSwitchTime();
        switchName = "high";
    }

    // public void QueueEnd() {
    //     switchTime = CalculateSwitchTime();
    //     switchName = "end";
    // }

    public void StopMusic()
    {
        switchTime = -1f;
        musicSource.Stop();
    }

    public void PlayNextNote()
    {
        noteSource.Stop();
        noteSource.loop = false;
        noteSource.volume = volume;

        noteSource.clip = noteClips[nextNote];
        noteSource.Play();

        nextNote += noteDirection;
        if (nextNote == 0 || nextNote == noteClips.Length - 1)
        {
            noteDirection = -noteDirection;
        }
    }
}
