using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class GameManager : MonoBehaviour
    {
        public static Action OnTownEscaped;
        public static Action OnDifficultyChange;

        public enum GameState { Start, Play, Minigame, Paused, End }
        public enum PlayerState { Radioactive, Neutral, Resting }
        public enum GameDifficulty { Easy, Medium, Hard, Impossible }

        public static GameState Game { get; set; } = GameState.Start;
        public static PlayerState Player { get; set; } = PlayerState.Neutral;
        public static GameDifficulty Difficulty { get; set; } = GameDifficulty.Easy;

        public static bool IsGameWon { get; private set; } = false;
        public static float TownEscaped { get; private set; } = 0f;
        private readonly float maxGameTime = 180f;

        private GameDifficulty lastDifficulty;

        private void Awake()
        {
            TownEscaped = 0f;
            IsGameWon = false;
            Player = PlayerState.Neutral;
            Difficulty = GameDifficulty.Easy;
            Game = GameState.Start;
            GameStarter.OnGameStart += StartGame;
            Controlls.OnMouseDownF += TryStartMinigame;
        }
        private void OnDestroy()
        {
            GameStarter.OnGameStart -= StartGame;
            Controlls.OnMouseDownF -= TryStartMinigame;
        }


        private void StartGame()
        {

        }

        private void Update()
        {
            if(!IsGameWon && (Game == GameState.Play || Game == GameState.Minigame))
            {
                float townTimer = Time.deltaTime / maxGameTime;
                TownEscaped += townTimer;

                if(Difficulty != lastDifficulty)
                    OnDifficultyChange?.Invoke();

                lastDifficulty = Difficulty;
                if(TownEscaped < 0.5f)
                {
                    Difficulty = GameDifficulty.Easy;
                }
                else if(TownEscaped < 0.75f)
                {
                    Difficulty = GameDifficulty.Medium;
                }
                else if(TownEscaped < 1f)
                {
                    Difficulty = GameDifficulty.Hard;
                }
                else
                {
                    Difficulty = GameDifficulty.Impossible;
                    IsGameWon = true;
                    OnTownEscaped?.Invoke();
                    SoundManager.PlaySound(SoundManager.Sound.Win);
                }
            }
        }

        private void TryStartMinigame()
        {
            if(Game == GameState.Minigame)
                return;

            (bool isHit, RaycastHit hitData) = RaycastManager.GetRaycastHitFronCam();
            if(isHit)
            {
                if(hitData.transform.CompareTag("Minigame"))
                {
                    //start minigame
                    MinigameBase minigame = hitData.transform.GetComponent<MinigameBase>();
                    minigame.TryActivateMinigame();
                }
            }
            
        }
    }
}