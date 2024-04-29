using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicSliderUI : MonoBehaviour
{
    [Header("Music Variables")]
    public AudioMixerGroup music;
    public Slider soundMusic;

    // Start is called before the first frame update
    void Start()
    {
        OnMusicVolumeChange();
    }

    /// <summary>
    /// Occurs when a change in the MusicSlider happens
    /// </summary>
    public void OnMusicVolumeChange()
    {
        // Start with the slider value
        float newVolume = soundMusic.value;
        if (newVolume <= 0)
        {
            // If we are at zero, lowest volyme
            newVolume = -80;
        }
        else
        {
            // We are >0, so find the log10 value
            newVolume = Mathf.Log10(newVolume);
            // Make it in the 0-20db range
            newVolume = newVolume * 20;
        }

        // Set the volume to the new volume setting
        music.audioMixer.SetFloat("MusicVol", newVolume);
    }
}
