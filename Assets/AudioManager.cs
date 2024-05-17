using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Audio Manager controls the playing of music and sound effects.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("Theme");
    }

    /// <summary>
    /// Method searches for the name of the requested music; if found, it will play, if not, an error is displayed in the log.
    /// </summary>
    /// <param name="name">Name of the music clip requested to play.</param>
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    /// <summary>
    /// Method searches for the name of the requested sfx; if found, it will play, if not, an error is displayed in the log.
    /// </summary>
    /// <param name="name">Name of the sfx clip requested to play.</param>
    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    /// <summary>
    /// Method mutes or unmutes the music.
    /// </summary>
    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    /// <summary>
    /// Method mutes or unmutes SFX audio.
    /// </summary>
    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    /// <summary>
    /// Sets the volume of the music.
    /// </summary>
    /// <param name="volume">The volume at which to play the music.</param>
    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    /// <summary>
    /// Sets the volume of the SFX audio.
    /// </summary>
    /// <param name="volume">The volume at which to play the SFX.</param>
    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
