using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum Sound { Explosion }
    public static Action<Sound> PlaySound;

    [SerializeField] private AudioClip explosion;
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        PlaySound = PlayProperClip;
    }

    private void PlayProperClip(Sound type)
    {
        AudioClip selectedClip = type switch
        {
            Sound.Explosion => explosion,
            _ => explosion
        };
        source.PlayOneShot(selectedClip);
    }
}
