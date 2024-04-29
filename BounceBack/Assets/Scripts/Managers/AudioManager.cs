using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource soundSource;
    public AudioSource musicSource;

    // Make it a static to be accessible everywhere
    public static AudioManager Instance = null;

    // Runs before the first frame update
    public void Awake()
    {
        // Check if there's an AudioManager already
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // If there are any other instances, destroy it if it's not this
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Plays a single clip through the sound effects source
    /// </summary>
    /// <param name="audioClip"></param>
    public void Play(AudioClip audioClip)
    {
        soundSource.PlayOneShot(audioClip);
    }

    /// <summary>
    /// Play a sound track through the music source and loop it
    /// </summary>
    /// <param name="audioClip"></param>
    public void PlayMusic(AudioClip audioClip)
    {
        musicSource.clip = audioClip;
        musicSource.Play();
        musicSource.loop = true;
    }

    /// <summary>
    /// Stops playing the current soundtrack
    /// </summary>
    public void Stop()
    {
        musicSource.Stop();
    }

    /// <summary>
    /// Pauses the current soundtrack
    /// </summary>
    public void Pause()
    {
        musicSource.Pause();
    }

    /// <summary>
    /// UnPauses the current soundtrack
    /// </summary>
    public void UnPause()
    {
        musicSource.UnPause();
    }
}
