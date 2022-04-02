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
            GameManager.GameDifficulty.Easy => 20f,
            GameManager.GameDifficulty.Medium => 10f,
            GameManager.GameDifficulty.Hard => 5f,
            _ => 20f,
        };
        private float RecoverSpeed => GameManager.Difficulty switch
        {
            GameManager.GameDifficulty.Easy => 3f,
            GameManager.GameDifficulty.Medium => 5f,
            GameManager.GameDifficulty.Hard => 7f,
            _ => 3f,
        };


        private void Update()
        {
            if(GameManager.Game == GameManager.GameState.Start)
                return;

            if(GameManager.Player == GameManager.PlayerState.Radioactive)
                IncreaseRadioactiveness();
            else
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