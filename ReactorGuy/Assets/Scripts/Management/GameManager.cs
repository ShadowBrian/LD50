using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class GameManager : MonoBehaviour
    {
        public static Action OnTownEscaped;

        public enum GameState { Start, Play, Minigame, Paused, End }
        public enum PlayerState { Radioactive, Resting }
        public enum GameDifficulty { Easy, Medium, Hard }

        public static GameState Game { get; set; } = GameState.Start;
        public static PlayerState Player { get; set; } = PlayerState.Resting;
        public static GameDifficulty Difficulty { get; set; } = GameDifficulty.Easy;

        public static bool IsGameWon { get; private set; } = false;
        public static float TownEscaped { get; private set; } = 0f;
        private readonly float maxGameTime = 10f;

        private void Awake()
        {
            GameStarter.OnGameStart += StartGame;
            Controlls.OnMouseDown += TryStartMinigame;
        }
        private void OnDestroy()
        {
            GameStarter.OnGameStart -= StartGame;
            Controlls.OnMouseDown -= TryStartMinigame;
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
                    IsGameWon = true;
                    OnTownEscaped?.Invoke();
                }
            }
        }

        private void TryStartMinigame(RaycastHit hitData)
        {
            if(hitData.transform.CompareTag("Minigame") && Game != GameState.Minigame)
            {
                //start minigame
                MinigameBase minigame = hitData.transform.GetComponent<MinigameBase>();
                minigame.TryActivateMinigame();
            }
        }
    }
}