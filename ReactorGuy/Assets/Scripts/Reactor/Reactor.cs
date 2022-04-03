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
        [SerializeField] private ParticleSystemRenderer particle1;
        [SerializeField] private ParticleSystemRenderer particle2;
        [SerializeField] private Color lightFrom;
        [SerializeField] private Color lightTo;
        private Material newParticleMaterial;

        private readonly float maxSliderValue = 0.999f;
        protected Renderer meshRenderer;
        protected MaterialPropertyBlock propertyBlock;

        private float ReactorMaxHeatTime => GameManager.Difficulty switch
        {
            GameManager.GameDifficulty.Easy => 40f,
            GameManager.GameDifficulty.Medium => 32.5f,
            GameManager.GameDifficulty.Hard => 25f,
            GameManager.GameDifficulty.Impossible => 15f,
            _ => 20f,
        };
        private float ReactorRecover => GameManager.Difficulty switch
        {
            GameManager.GameDifficulty.Easy => 0.5f,
            GameManager.GameDifficulty.Medium => 0.55f,
            GameManager.GameDifficulty.Hard => 0.6f,
            GameManager.GameDifficulty.Impossible => 0.3f,
            _ => 0.3f,
        };

        private void Awake()
        {
            newParticleMaterial = new Material(particle1.sharedMaterial);
            particle1.sharedMaterial = newParticleMaterial;
            particle2.sharedMaterial = newParticleMaterial;

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
            ReactorHeat *= ReactorRecover;
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
                    SoundManager.ForcePlaySound(SoundManager.Sound.Explosion);
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
            newParticleMaterial.color = newColor;

        }
    }
}