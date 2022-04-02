using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game {
    public class RadioactiveSlider : MonoBehaviour
    {
        private Slider slider;

        private void Awake()
        {
            slider = GetComponent<Slider>();
        }

        private void Update()
        {
            if(GameManager.Game == GameManager.GameState.Start)
                return;

            slider.value = Player.RadioactiveMeter;
        }
    }
}