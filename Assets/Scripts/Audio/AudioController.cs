using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    private const string MasterVolumeParameter = "MasterVolume";
    private const string MusicVolumeParameter = "MusicVolume";
    private const string SFXVolumeParameter = "SFXVolume";

    private const int minVolumeDb = -60;
    private const int maxVolumeDb = 0;

    [SerializeField] private AudioMixer mixer;

    public float MasterVolume
    {
        get => GetMixerParameter(MasterVolumeParameter);
        set => SetMixerVolumeParameter(MasterVolumeParameter, value);
    }
    public float MusicVolume
    {
        get => GetMixerParameter(MusicVolumeParameter);
        set => SetMixerVolumeParameter(MusicVolumeParameter, value);
    }
    public float SFXVolume
    {
        get => GetMixerParameter(SFXVolumeParameter);
        set => SetMixerVolumeParameter(SFXVolumeParameter, value);
    }

    private void Start()
    {
        LoadAudioPreferences();
    }

    private void LoadAudioPreferences()
    {
        MasterVolume = PlayerPrefs.GetFloat(MasterVolumeParameter, MasterVolume);
        MusicVolume = PlayerPrefs.GetFloat(MusicVolumeParameter, MusicVolume);
        SFXVolume = PlayerPrefs.GetFloat(SFXVolumeParameter, SFXVolume);

    }

    public void SaveAudioPreferences()
    {
        PlayerPrefs.SetFloat(MasterVolumeParameter, MasterVolume);
        PlayerPrefs.SetFloat(MusicVolumeParameter, MusicVolume);
        PlayerPrefs.SetFloat(SFXVolumeParameter, SFXVolume);
    }

    private void SetMixerVolumeParameter(string key, float volume)
    {
        float volumeValue = Mathf.Lerp(minVolumeDb, maxVolumeDb, volume);
        mixer.SetFloat(key, volumeValue);
    }

    private float GetMixerParameter(string key)
    {
        if (mixer.GetFloat(key, out var value))
        {
            return Mathf.InverseLerp(minVolumeDb, maxVolumeDb, value);
        }
        return 1;
    }
}
