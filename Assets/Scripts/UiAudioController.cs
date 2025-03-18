using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UiAudioController : MonoBehaviour
{
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private AudioClip powerUpSound;
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip enemyHitSound;

    private AudioSource audioSource;
    public AudioSource AudioSource => audioSource == null ? audioSource = GetComponent<AudioSource>() : audioSource;

    public void PlayButtonSound()
    {
        Play(buttonSound);
    }
    public void PlayPowerUpSound()
    {
        Play(powerUpSound);
    }
    public void PlayCoinSound()
    {
        Play(coinSound);
    }
    public void PlayEnemyHitSound()
    {
        Play(enemyHitSound);
    }
    private void Play(AudioClip clip)
    {
        AudioUtility.PlayAudioCue(AudioSource, clip);
    }
}
