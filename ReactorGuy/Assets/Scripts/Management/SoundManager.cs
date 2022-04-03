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

    private void Awake()
    {
        source = GetComponent<AudioSource>();
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
        if(source)
            source.volume = volume;
    }
}
