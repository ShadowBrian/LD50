using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class GameManager : MonoBehaviour
    {
        public enum GameState { Start, Play, Minigame, Paused, End }
        public enum PlayerState { Radioactive, Resting }
        public enum GameDifficulty { Easy, Medium, Hard }

        public static GameState Game { get; set; } = GameState.Start;
        public static PlayerState Player { get; set; } = PlayerState.Resting;
        public static GameDifficulty Difficulty { get; set; } = GameDifficulty.Easy;

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