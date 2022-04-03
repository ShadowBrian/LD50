using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Reactor : MonoBehaviour
    {
        public static System.Action OnReactorOverheat;

        public static float ReactorHeat { get; set; } = 0f;

        [SerializeField] private Light light1;
        [SerializeField] private Light light2;
        [SerializeField] private Color lightFrom;
        [SerializeField] private Color lightTo;

        private readonly float maxSliderValue = 0.999f;
        protected Renderer meshRenderer;
        protected MaterialPropertyBlock propertyBlock;

        private float ReactorMaxHeatTime => GameManager.Difficulty switch
        {
            GameManager.GameDifficulty.Easy => 1f,
            GameManager.GameDifficulty.Medium => 35f,
            GameManager.GameDifficulty.Hard => 30f,
            GameManager.GameDifficulty.Impossible => 15f,
            _ => 20f,
        };

        private void Awake()
        {
            propertyBlock = new MaterialPropertyBlock();
            meshRenderer = GetComponentInChildren<Renderer>();
            meshRenderer.GetPropertyBlock(propertyBlock);
            ReactorHeat = 0;
            propertyBlock.SetFloat("_Percentage", ReactorHeat);
            meshRenderer.SetPropertyBlock(propertyBlock);
            UpdateLights();

            MinigameBase.OnMinigameFinished += LowerReactorHeat;
        }
        private void OnDestroy()
        {
            MinigameBase.OnMinigameFinished -= LowerReactorHeat;
        }


        private void LowerReactorHeat()
        {
            ReactorHeat *= 0.35f;
            UpdateLights();
        }


        private void Update()
        {
            if(GameManager.Game == GameManager.GameState.Start || GameManager.Game == GameManager.GameState.End)
                return;

            if(GameManager.Game == GameManager.GameState.Play || GameManager.Game == GameManager.GameState.Minigame)
            {
                float reactorTimer = Time.deltaTime / ReactorMaxHeatTime;
                ReactorHeat = Mathf.Min(maxSliderValue, ReactorHeat + reactorTimer);
                UpdateLights();
                if(ReactorHeat >= maxSliderValue)
                {
                    Debug.Log("FINISH, game over");
                    GameManager.Game = GameManager.GameState.End;
                    SoundManager.PlaySound(SoundManager.Sound.Explosion);
                    OnReactorOverheat?.Invoke();
                }
            }
        }

        private void UpdateLights()
        {
            propertyBlock.SetFloat("_Percentage", ReactorHeat);
            meshRenderer.SetPropertyBlock(propertyBlock);
            Color newColor = Color.Lerp(lightFrom, lightTo, ReactorHeat);
            light1.color = newColor;
            light2.color = newColor;
        }
    }
}