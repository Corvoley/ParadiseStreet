using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour
{
    [SerializeField] private AudioController audioController;

    [Header("UI Elements")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private GameObject settingsButton;

    private void OnEnable()
    {
        UpdateUI();
    }

    private void OnDisable()
    {
        audioController.SaveAudioPreferences();
    }

    private void UpdateUI()
    {
        masterSlider.value = audioController.MasterVolume;
        musicSlider.value = audioController.MusicVolume;
        sfxSlider.value = audioController.SFXVolume;
    }

    public void Close()
    {
        settingsButton.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnMasterVolumeChange(float value)
    {
        audioController.MasterVolume = value;
    }

    public void OnMusicVolumeChange(float value)
    {
        audioController.MusicVolume = value;
    }

    public void OnSFXVolumeChange(float value)
    {
        audioController.SFXVolume = value;
    } 
}