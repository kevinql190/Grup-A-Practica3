using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSliderHandler : MonoBehaviour
{
    [SerializeField] private Slider musicSlider, sfxSlider, masterSlider;
    public AudioMixer mixer;
    private void Start()
    {
        mixer = AudioManager.Instance.mixer;
        if (musicSlider == null || sfxSlider == null || masterSlider == null) return;
        AudioManager.Instance.LoadAllVolume();
        SliderGetVolumes();
    }
    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        mixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        mixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        mixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }
    private void SliderGetVolumes()
    {
        mixer.GetFloat("Master", out float rawMasterVolume);
        masterSlider.value = Mathf.Pow(10, rawMasterVolume / 20);
        mixer.GetFloat("Music", out float rawMusicVolume);
        musicSlider.value = Mathf.Pow(10, rawMusicVolume / 20);
        mixer.GetFloat("SFX", out float rawSFXVolume);
        sfxSlider.value = Mathf.Pow(10, rawSFXVolume / 20);
    }
}
