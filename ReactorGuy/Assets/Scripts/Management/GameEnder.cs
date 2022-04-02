using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameEnder : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasG;
        [SerializeField] private List<CanvasGroup> winTexts;
        [SerializeField] private List<CanvasGroup> looseReactorTexts;
        [SerializeField] private List<CanvasGroup> looseRadiationTexts;
        int textIndex;
        bool isReactorFault = false;

        private void Awake()
        {
            textIndex = 0;
            Reactor.OnReactorOverheat += TurnOnReactor;
            Player.OnPlayerRadioactive += TurnOnRadiation;
        }
        private void OnDestroy()
        {
            Reactor.OnReactorOverheat -= TurnOnReactor;
            Player.OnPlayerRadioactive -= TurnOnRadiation;
        }

        private void TurnOnReactor()
        {
            isReactorFault = true;
            StartCoroutine(SmoothCanvasAppear(canvasG, 10f, 2f, TurnOnProperText));
        }
        private void TurnOnRadiation()
        {
            isReactorFault = false;
            StartCoroutine(SmoothCanvasAppear(canvasG, 5f, 2f, TurnOnProperText));
        }

        private void TurnOnProperText()
        {
            if(GameManager.IsGameWon)
                ShowProperTextsList(winTexts);
            else
                ShowProperTextsList(isReactorFault ? looseReactorTexts : looseRadiationTexts);
        }

        private void ShowProperTextsList(List<CanvasGroup> canvases)
        {
            int lastIndex = Mathf.Max(0, textIndex - 1);
            if(canvases[lastIndex].alpha == 1)
            {
                StartCoroutine(SmoothCanvasDisappear(canvases[lastIndex], () => ShowProperTextsList(canvases)));
            }
            else
            {
                StartCoroutine(SmoothCanvasAppear(canvases[textIndex], 2f, 1f,
                   () =>
                   {
                       if(++textIndex < canvases.Count)
                           ShowProperTextsList(canvases);
                       else
                       {
                           Debug.Log("GAME ENDED");
                           Utility.LockCursor(false);
                           Cursor.lockState = CursorLockMode.None;
                       }
                   }));
            }
        }

        private IEnumerator SmoothCanvasAppear(CanvasGroup canvas, float appearTime, float pause, System.Action OnEnd)
        {
            float maxTime = appearTime;
            float timer = 0;
            while(true)
            {
                yield return null;
                timer += Time.deltaTime;
                float t = timer / maxTime;
                float newAlpha = Mathf.Lerp(0, 1, t);
                canvas.alpha = newAlpha;
                if(canvas.alpha >= 1)
                {
                    yield return new WaitForSecondsRealtime(pause);
                    OnEnd?.Invoke();
                    yield break;
                }
            }
        }
        private IEnumerator SmoothCanvasDisappear(CanvasGroup canvas, System.Action OnEnd)
        {
            float maxTime = 2f;
            float timer = 0;
            while(true)
            {
                yield return null;
                timer += Time.deltaTime;
                float t = timer / maxTime;
                float newAlpha = Mathf.Lerp(1, 0, t);
                canvas.alpha = newAlpha;
                if(canvas.alpha <= 0)
                {
                    OnEnd?.Invoke();
                    yield break;
                }
            }
        }


    }
}