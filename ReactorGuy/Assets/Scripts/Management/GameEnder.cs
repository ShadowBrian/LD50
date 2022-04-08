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

        public void Button_PlayAgain()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        public void Update()
        {
            if (looseRadiationTexts[1].alpha >= 1f)
            {
                if (UnityXRInputBridge.instance.GetButtonDown(XRButtonMasks.primaryButton, XRHandSide.LeftHand))
                {
                    Button_PlayAgain();
                }
            }
        }


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
            if (GameManager.IsGameWon)
                ShowProperTextsList(winTexts);
            else
                ShowProperTextsList(isReactorFault ? looseReactorTexts : looseRadiationTexts);
        }

        private void ShowProperTextsList(List<CanvasGroup> canvases)
        {
            int lastIndex = Mathf.Max(0, textIndex - 1);
            if (canvases[lastIndex].alpha == 1)
            {
                StartCoroutine(SmoothCanvasDisappear(canvases[lastIndex], () => ShowProperTextsList(canvases)));
            }
            else
            {
                float pauseTime = textIndex + 1 < canvases.Count ? 1f : 0f;
                StartCoroutine(SmoothCanvasAppear(canvases[textIndex], 2f, pauseTime,
                   () =>
                   {
                       if (++textIndex < canvases.Count)
                           ShowProperTextsList(canvases);
                       else
                       {
                           Debug.Log("GAME ENDED");
                           canvases[textIndex - 1].ignoreParentGroups = true;
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
            while (true)
            {
                yield return null;
                timer += Time.deltaTime;
                float t = timer / maxTime;
                float newAlpha = Mathf.Lerp(0, 1, t);
                canvas.alpha = newAlpha;
                if (canvas.alpha >= 1)
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
            while (true)
            {
                yield return null;
                timer += Time.deltaTime;
                float t = timer / maxTime;
                float newAlpha = Mathf.Lerp(1, 0, t);
                canvas.alpha = newAlpha;
                if (canvas.alpha <= 0)
                {
                    OnEnd?.Invoke();
                    yield break;
                }
            }
        }


    }
}