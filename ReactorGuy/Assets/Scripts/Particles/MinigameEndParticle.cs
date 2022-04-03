using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameEndParticle : MonoBehaviour
{
    private ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        Game.MinigameBase.OnMinigameFinished += PlayParticle;
    }
    private void OnDestroy()
    {
        Game.MinigameBase.OnMinigameFinished -= PlayParticle;
    }

    private void PlayParticle()
    {
        if(particle)
            particle.Play();
    }
}
