using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum Sound { Explosion }
    public static Action<Sound> PlaySound;
    public static Action<float> SetVolume;

    [SerializeField] private AudioClip explosion;
    private AudioSource source;
    public static float Volume { get; private set; } = 0.5f;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.volume = Volume;
        PlaySound = PlayProperClip;
        SettingsManager.OnVolumeChange += SetNewVolume;
    }
    private void OnDestroy()
    {
        SettingsManager.OnVolumeChange -= SetNewVolume;
    }

    private void PlayProperClip(Sound type)
    {
        AudioClip selectedClip = type switch
        {
            Sound.Explosion => explosion,
            _ => explosion
        };
        if(source)
            source.PlayOneShot(selectedClip);
    }

    private void SetNewVolume(float volume)
    {
        Volume = volume;
        if(source)
            source.volume = volume;
    }
}
