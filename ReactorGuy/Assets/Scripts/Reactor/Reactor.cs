using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Reactor : MonoBehaviour
    {
        public static System.Action OnReactorOverheat;

        public static float ReactorHeat { get; set; } = 0f;

        private readonly float maxSliderValue = 0.999f;

        private float ReactorMaxHeatTime => GameManager.Difficulty switch
        {
            GameManager.GameDifficulty.Easy => 20f,
            GameManager.GameDifficulty.Medium => 10f,
            GameManager.GameDifficulty.Hard => 5f,
            _ => 20f,
        };

        private void Awake()
        {
            MinigameBase.OnMinigameFinished += LowerReactorHeat;
        }
        private void OnDestroy()
        {
            MinigameBase.OnMinigameFinished -= LowerReactorHeat;
        }


        private void LowerReactorHeat()
        {
            ReactorHeat = Mathf.Max(0, ReactorHeat - 0.5f);
        }


        private void Update()
        {
            if(GameManager.Game == GameManager.GameState.Start || GameManager.Game == GameManager.GameState.End)
                return;

            if(GameManager.Game == GameManager.GameState.Play || GameManager.Game == GameManager.GameState.Minigame)
            {
                float reactorTimer = Time.deltaTime / ReactorMaxHeatTime;
                ReactorHeat = Mathf.Min(maxSliderValue, ReactorHeat + reactorTimer);
                if(ReactorHeat >= maxSliderValue)
                {
                    Debug.Log("FINISH, game over");
                    GameManager.Game = GameManager.GameState.End;
                    SoundManager.PlaySound(SoundManager.Sound.Explosion);
                    OnReactorOverheat?.Invoke();
                }
            }
        }
    }
}