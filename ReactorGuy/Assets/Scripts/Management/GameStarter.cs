using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameStarter : MonoBehaviour
    {
        public static System.Action OnGameStart;

        [SerializeField] private Button startButton;
        [SerializeField] private Button easyModeStartButton;
        [SerializeField] private Cinemachine.CinemachineVirtualCameraBase playerCam;

        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Canvas>().enabled = true;
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(StartGame);
            easyModeStartButton.onClick.RemoveAllListeners();
            easyModeStartButton.onClick.AddListener(StartEasyGame);
        }

        private void StartGame()
        {
            GameManager.Mode = GameManager.GameMode.Normal;
            playerCam.gameObject.SetActive(true);
            OnGameStart?.Invoke();
            gameObject.SetActive(false);
        }
        private void StartEasyGame()
        {
            GameManager.Mode = GameManager.GameMode.Easy;
            playerCam.gameObject.SetActive(true);
            OnGameStart?.Invoke();
            gameObject.SetActive(false);
        }
    }
}