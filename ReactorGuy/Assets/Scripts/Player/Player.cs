using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class Player : MonoBehaviour
    {
        public static float RadioactiveMeter { get; private set; }

        private float RadioactiveSpeed => GameManager.Difficulty switch
        {
            GameManager.GameDifficulty.Easy => 0.1f,
            GameManager.GameDifficulty.Medium => 0.2f,
            GameManager.GameDifficulty.Hard => 0.3f,
            _ => 0.1f,
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
            RadioactiveMeter += Time.deltaTime * RadioactiveSpeed;
            RadioactiveMeter = Mathf.Clamp01(RadioactiveMeter);
        }
        private void DecreaseRadioactiveness()
        {
            RadioactiveMeter -= Time.deltaTime * RadioactiveSpeed * 1.1f;
            RadioactiveMeter = Mathf.Clamp01(RadioactiveMeter);
        }

    }
}