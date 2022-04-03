using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SoundManager : MonoBehaviour
    {
        public enum Sound { 
            Blip,
            Die,
            MinigameEnter,
            MinigameDone,
            Explosion, 
            RadiationHit,
            RadiationRest,
            Win,
        }
        public static Action<Sound> PlaySound;
        public static Action<Sound> ForcePlaySound;
        public static Action<float> SetVolume;

        [SerializeField] private AudioSource soundSource;
        [SerializeField] private List<AudioClip> blips = new List<AudioClip>();
        [SerializeField] private AudioClip die;
        [SerializeField] private AudioClip minigameEnter;
        [SerializeField] private AudioClip minigameDone;
        [SerializeField] private AudioClip explosion;
        [SerializeField] private AudioClip radHit;
        [SerializeField] private AudioClip radRest;
        [SerializeField] private AudioClip win;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private List<AudioClip> musics = new List<AudioClip>();
        public static float Volume { get; private set; } = 0.5f;

        private void Awake()
        {
            soundSource.volume = Volume;
            musicSource.volume = Volume;
            PlaySound = PlayProperClip;
            ForcePlaySound = ForcePlaySoundPlayProperClip;
            SettingsManager.OnVolumeChange += SetNewVolume;
            GameStarter.OnGameStart += PlayMusic;
            GameManager.OnDifficultyChange += ChangeMusic;
            Player.OnPlayerRadioactive += TurnMusicOff;
            Reactor.OnReactorOverheat += TurnMusicOff;
            Controlls.OnPause += PauseMusic;
        }
        private void OnDestroy()
        {
            SettingsManager.OnVolumeChange -= SetNewVolume;
            GameStarter.OnGameStart -= PlayMusic;
            GameManager.OnDifficultyChange -= ChangeMusic;
            Player.OnPlayerRadioactive -= TurnMusicOff;
            Reactor.OnReactorOverheat -= TurnMusicOff;
            Controlls.OnPause -= PauseMusic;
        }

        private void PlayProperClip(Sound type)
        {
            if(GameManager.Game == GameManager.GameState.Start || GameManager.Game == GameManager.GameState.End)
                return;

            AudioClip selectedClip = type switch
            {
                Sound.Blip => blips.GetRandom(),
                Sound.Die => die,
                Sound.MinigameEnter => minigameEnter,
                Sound.MinigameDone => minigameDone,
                Sound.Explosion => explosion,
                Sound.RadiationHit => radHit,
                Sound.RadiationRest => radRest,
                Sound.Win => win,
                _ => explosion
            };
            if(soundSource)
                soundSource.PlayOneShot(selectedClip);
        }
        private void ForcePlaySoundPlayProperClip(Sound type)
        {
            AudioClip selectedClip = type switch
            {
                Sound.Blip => blips.GetRandom(),
                Sound.Die => die,
                Sound.MinigameDone => minigameDone,
                Sound.Explosion => explosion,
                Sound.RadiationHit => radHit,
                Sound.RadiationRest => radRest,
                Sound.Win => win,
                _ => explosion
            };
            if(soundSource)
                soundSource.PlayOneShot(selectedClip);
        }

        private void SetNewVolume(float volume)
        {
            Volume = volume;
            if(soundSource)
                soundSource.volume = volume;
            if(musicSource)
                musicSource.volume = volume;
        }

        private void PlayMusic()
        {
            musicSource.clip = musics[0];
            musicSource.Play();
        }
        private void ChangeMusic()
        {
            if(GameManager.Difficulty == GameManager.GameDifficulty.Medium)
            {
                musicSource.clip = musics[1];
                musicSource.Play();
            }
            else if(GameManager.Difficulty == GameManager.GameDifficulty.Hard)
            {
                musicSource.clip = musics[2];
                musicSource.Play();
            }
        }

        private void TurnMusicOff()
        {
            StartCoroutine(SmoothTurnMusicOff());
        }


        private IEnumerator SmoothTurnMusicOff()
        {
            float maxTime = 4f;
            float timer = 0;
            float initialVolume = musicSource.volume;
            while(true)
            {
                yield return null;
                timer += Time.deltaTime;
                float t = timer / maxTime;
                float newAlpha = Mathf.Lerp(initialVolume, 0, t);
                musicSource.volume = newAlpha;
                if(newAlpha <= 0)
                {
                    musicSource.Stop();
                    yield break;
                }
            }
        }

        private void PauseMusic(bool shouldPause)
        {
            if(GameManager.Game == GameManager.GameState.End || GameManager.Game == GameManager.GameState.Start)
                return;

            if(shouldPause)
                musicSource.Pause();
            else
                musicSource.UnPause();
        }
    }
}