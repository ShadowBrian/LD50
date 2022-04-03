using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class InGameUIShower : MonoBehaviour
    {
        private CanvasGroup canvasG;


        private void Awake()
        {
            canvasG = GetComponent<CanvasGroup>();
            canvasG.alpha = 0;
            GameStarter.OnGameStart += Show;
        }
        private void OnDestroy()
        {
            GameStarter.OnGameStart -= Show;
        }

        private void Show()
        {
            canvasG.alpha = 1;
        }
    }
}