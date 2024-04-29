using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SFXSliderUI : MonoBehaviour
{
    [Header("Sound Variables")]
    public AudioMixerGroup effects;
    public Slider soundEffects;

    // Start is called before the first frame update
    void Start()
    {
        OnEffectVolumeChange();
    }

    /// <summary>
    /// Occurs when a change in the SFXSlider happens
    /// </summary>
    public void OnEffectVolumeChange()
    {
        // Start with the slider value
        float newVolume = soundEffects.value;
        if (newVolume <= 0)
        {
            // If we are at zero, lowest volume
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
        effects.audioMixer.SetFloat("SFXVol", newVolume);
    }
}
