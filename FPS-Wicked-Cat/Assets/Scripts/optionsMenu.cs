using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class optionsMenu : MonoBehaviour
{
    [Header("--- Mixer ---")]
    public AudioMixer audioMixer;
    public string mixerName;

    [Header("--- Silders ---")]
    public Slider slider;

    [Header("--- Volume ---")]
    public float defaultVol = 0.5f;

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat(mixerName, Mathf.Log10(volume) * 20f);
    }

    public void ApplyBtn()
    {
        PlayerPrefs.SetFloat(mixerName, slider.value);
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey(mixerName))
        {
            slider.value = PlayerPrefs.GetFloat(mixerName);
        }
        else
        {
            slider.value= defaultVol;
        }
    }
}
