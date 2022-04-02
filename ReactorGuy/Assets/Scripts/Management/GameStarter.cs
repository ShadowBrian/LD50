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
        [SerializeField] private Cinemachine.CinemachineVirtualCameraBase playerCam;

        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Canvas>().enabled = true;
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(StartGame);
        }

        private void StartGame()
        {
            playerCam.gameObject.SetActive(true);
            OnGameStart?.Invoke();
            gameObject.SetActive(false);
        }
    }
}