using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class Player : MonoBehaviour
    {
        public static System.Action OnPlayerRadioactive;
        public static float RadioactiveMeter { get; private set; }
        private float RadioactiveSpeed => GameManager.Difficulty switch
        {
            GameManager.GameDifficulty.Easy => 40f,
            GameManager.GameDifficulty.Medium => 35f,
            GameManager.GameDifficulty.Hard => 30f,
            GameManager.GameDifficulty.Impossible => 15f,
            _ => 40f,
        };
        private float RecoverSpeed => GameManager.Difficulty switch
        {
            GameManager.GameDifficulty.Easy => 5f,
            GameManager.GameDifficulty.Medium => 7f,
            GameManager.GameDifficulty.Hard => 9f,
            GameManager.GameDifficulty.Impossible => 5f,
            _ => 5f,
        };

        private void Awake()
        {
            RadioactiveMeter = 0;
        }


        private void Update()
        {
            if(GameManager.Game == GameManager.GameState.Start)
                return;

            if(GameManager.Player == GameManager.PlayerState.Radioactive)
                IncreaseRadioactiveness();
            else if(GameManager.Player == GameManager.PlayerState.Resting)
                DecreaseRadioactiveness();
        }

        private void IncreaseRadioactiveness()
        {
            if(GameManager.Game == GameManager.GameState.Start || GameManager.Game == GameManager.GameState.End)
                return;

            if(GameManager.Game == GameManager.GameState.Play || GameManager.Game == GameManager.GameState.Minigame)
            {
                RadioactiveMeter += Time.deltaTime / RadioactiveSpeed;
                RadioactiveMeter = Mathf.Clamp01(RadioactiveMeter);
                if(RadioactiveMeter >= 1)
                {
                    Debug.Log("FINISH, game over");
                    GameManager.Game = GameManager.GameState.End;
                    OnPlayerRadioactive?.Invoke();
                }
            }
        }
        private void DecreaseRadioactiveness()
        {
            RadioactiveMeter -= Time.deltaTime / RecoverSpeed;
            RadioactiveMeter = Mathf.Clamp01(RadioactiveMeter);
        }

    }
}