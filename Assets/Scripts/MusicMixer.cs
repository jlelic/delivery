using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMixer : MonoBehaviour
{
    public static MusicMixer instance;

    [Range(0f, 2f)]
    [SerializeField] float musicVolume = 1;
    [SerializeField] float effectsVolume = 1;
    [SerializeField] AudioClip[] musicClips;
    [SerializeField] AudioClip[] noteClips;
    [SerializeField] AudioClip[] effectClips;
    [SerializeField] float segmentLength = 1.6f;

    private AudioSource musicSource;
    private AudioSource[] noteSources;

    private float switchTime = -1f;
    private int switchIndex;
    private int currentMusic = 0;

    private int nextNote = 0;
    private int noteDirection = 1;
    private int currentNoteSourceIndex;

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
        noteSources = new AudioSource[2] { audioSources[1], audioSources[2] };
        StartMusic();
    }

    void Update()
    {
        musicSource.volume = musicVolume;
        noteSources[0].volume = effectsVolume;
        noteSources[1].volume = effectsVolume;

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

    public void PlayNextNote(float pitch =1f)
    {
        var noteSource = noteSources[currentNoteSourceIndex];
        currentNoteSourceIndex = 1 - currentNoteSourceIndex;

        noteSource.pitch = pitch;
        noteSource.Stop();
        noteSource.loop = false;

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
        var noteSource = noteSources[currentNoteSourceIndex];
        currentNoteSourceIndex = 1 - currentNoteSourceIndex;

        noteSource.pitch = pitch;
        noteSource.Stop();
        noteSource.loop = false;
        noteSource.clip = noteClips[note];
        noteSource.Play();
    }

    public void PlayEffect(int effectId, float pitch = 1)
    {
        var noteSource = noteSources[currentNoteSourceIndex];
        currentNoteSourceIndex = 1 - currentNoteSourceIndex;

        noteSource.pitch = pitch;
        noteSource.Stop();
        noteSource.loop = false;
        noteSource.clip = effectClips[effectId];
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


    public void SetMusicVolume(float volume)
    {
        this.musicVolume = volume;
    }

    public void SetEffectsVolume(float volume)
    {
        this.effectsVolume = volume;
    }
}
