using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SettingsShower : MonoBehaviour
    {
        [SerializeField] private RectTransform settings;
        [SerializeField] private CanvasGroup darkenG;
        private CanvasGroup canvasG;
        private float initialPosition = -200;
        private float targetPosition = 0;


        private void Awake()
        {
            canvasG = GetComponent<CanvasGroup>();
            canvasG.alpha = 0;
            GameStarter.OnGameStart += Show;
            Controlls.OnPause += ShowMenu;
        }
        private void OnDestroy()
        {
            GameStarter.OnGameStart -= Show;
            Controlls.OnPause -= ShowMenu;
        }

        private void Show()
        {
            canvasG.alpha = 1;
        }
        private void ShowMenu(bool isPaused)
        {
            if(isPaused)
            {
                StartCoroutine(SmoothIn());
                darkenG.alpha = 1;
                darkenG.blocksRaycasts = true;
            }
            else
            {
                StartCoroutine(SmoothOut());
                darkenG.alpha = 0;
                darkenG.blocksRaycasts = false;
            }
        }


        IEnumerator SmoothIn()
        {
            float maxTime = 0.5f;
            float timer = 0;
            Vector2 newPoisition = settings.anchoredPosition;
            while(true)
            {
                yield return null;
                timer += Time.unscaledDeltaTime;
                float t = timer / maxTime;
                t = t * t * t;
                newPoisition.x = Mathf.Lerp(initialPosition, targetPosition, t);
                settings.anchoredPosition = newPoisition;
                if(newPoisition.x >= targetPosition)
                {
                    newPoisition.x = targetPosition;
                    settings.anchoredPosition = newPoisition;
                    yield break;
                }
            }
        }
        IEnumerator SmoothOut()
        {
            float maxTime = 0.5f;
            float timer = 0;
            Vector2 newPoisition = settings.anchoredPosition;
            while(true)
            {
                yield return null;
                timer += Time.unscaledDeltaTime;
                float t = timer / maxTime;
                t = t * t * t;
                newPoisition.x = Mathf.Lerp(targetPosition, initialPosition, t);
                settings.anchoredPosition = newPoisition;
                if(newPoisition.x <= initialPosition)
                {
                    newPoisition.x = initialPosition;
                    settings.anchoredPosition = newPoisition;
                    yield break;
                }
            }
        }
    }
}