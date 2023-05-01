using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMixer : MonoBehaviour
{
    public static MusicMixer instance;

    [Range(0f, 2f)]
    [SerializeField] float volume = 1;
    [SerializeField] AudioClip[] musicClips;
    [SerializeField] AudioClip[] noteClips;
    [SerializeField] float segmentLength = 1.6f;

    private AudioSource musicSource;
    private AudioSource noteSource;

    private float switchTime = -1f;
    private int switchIndex;
    private int currentMusic = 0;

    private int nextNote = 0;
    private int noteDirection = 1;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
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
        noteSource.volume = volume;

        if (switchTime >= 0f && musicSource.time >= switchTime)
        {
            switchTime = -1f;
            currentMusic = switchIndex;
            musicSource.Stop();
            musicSource.clip = musicClips[switchIndex];
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    private float CalculateSwitchTime()
    {
        float clipLength = musicSource.clip.length;
        float currentTime = musicSource.time;
        float proposedSwitchTime = segmentLength * Mathf.Ceil(currentTime / segmentLength);
        return proposedSwitchTime < clipLength ? proposedSwitchTime : 0f;
    }

    public void StartMusic()
    {
        musicSource.Stop();
        musicSource.clip = musicClips[0];
        musicSource.volume = volume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void QueueLower()
    {
        if (switchIndex == 0)
        {
            return;
        }
        switchTime = CalculateSwitchTime();
        switchIndex = currentMusic - 1;
    }

    public void QueueHigher()
    {
        if (switchIndex == musicClips.Length - 1)
        {
            return;
        }
        switchTime = CalculateSwitchTime();
        switchIndex = currentMusic + 1;

    }

    public void StopMusic()
    {
        switchTime = -1f;
        musicSource.Stop();
    }

    public void PlayNextNote()
    {
        noteSource.pitch = 1;
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

    public void PlayPitchedNote(int note, float pitch)
    {
        noteSource.pitch = pitch;
        noteSource.Stop();
        noteSource.loop = false;
        noteSource.volume = volume;
        noteSource.clip = noteClips[note];
        noteSource.Play();
    }

    public void HandleRatingReceived(int rating)
    {
        if (rating <= 1)
        {
            QueueLower();
        }
        else if (rating >= 4)
        {
            QueueHigher();
        }
    }
}
