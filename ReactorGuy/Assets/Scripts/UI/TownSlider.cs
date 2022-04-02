using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game
{
    public class TownSlider : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        private Slider slider;
        


        private void Awake()
        {
            slider = GetComponent<Slider>();
            text.gameObject.SetActive(false);
            GameManager.OnTownEscaped += ShowText;
        }
        private void OnDestroy()
        {
            GameManager.OnTownEscaped -= ShowText;
        }

        private void Update()
        {
            slider.value = GameManager.TownEscaped;
        }
        private void ShowText()
        {
            text.gameObject.SetActive(true);
        }
    }
}