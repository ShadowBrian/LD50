using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Game
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeReference] private CinemachineVirtualCamera lookCamera;
        [SerializeReference] private CinemachineVirtualCamera layDownCamera;
        [SerializeReference] private float playerSpeed = 0.1f;
        private CinemachinePOV aimCam;
        private CinemachineBasicMultiChannelPerlin noise;
        public static float Sensitivity { get; private set; } = 1f;

        private void Awake()
        {
            aimCam = lookCamera.GetCinemachineComponent<CinemachinePOV>();
            noise = lookCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            aimCam.m_VerticalAxis.m_MaxSpeed = Sensitivity;

            GameStarter.OnGameStart += StartMoving;
            Controlls.OnPause += PauseCamera;
            MinigameBase.OnMinigame += PauseCamera;
            Reactor.OnReactorOverheat += PauseCameraWithShake;
            Player.OnPlayerRadioactive += PauseCameraWithLayDown;
            SettingsManager.OnSensitivityChange += UpdateSensitivity;
            PauseCamera(true);
        }
        private void OnDestroy()
        {
            GameStarter.OnGameStart -= StartMoving;
            Controlls.OnPause -= PauseCamera;
            MinigameBase.OnMinigame -= PauseCamera;
            Reactor.OnReactorOverheat -= PauseCameraWithShake;
            Player.OnPlayerRadioactive -= PauseCameraWithLayDown;
            SettingsManager.OnSensitivityChange -= UpdateSensitivity;
        }

        private void UpdateSensitivity(float newSensitivity)
        {
            Sensitivity = newSensitivity;
        }

        private void PauseCameraWithShake()
        {
            PauseCamera(true);
            StartCoroutine(IncreaseShake());
            IEnumerator IncreaseShake()
            {
                float maxTime = 10f;
                float timer = 0;
                while (true)
                {
                    yield return null;
                    timer += Time.deltaTime;
                    float t = timer / maxTime;
                    float newAlpha = Mathf.Lerp(0, 10, t);
                    noise.m_AmplitudeGain = newAlpha;
                    if (newAlpha >= 10)
                    {
                        noise.m_AmplitudeGain = 0;
                        yield break;
                    }
                }
            }
        }
        private void PauseCameraWithLayDown()
        {
            PauseCamera(true);
            layDownCamera.gameObject.SetActive(true);
            lookCamera.gameObject.SetActive(false);
            noise = layDownCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            StartCoroutine(IncreaseShake());
            IEnumerator IncreaseShake()
            {
                float maxTime = 10f;
                float timer = 0;
                while (true)
                {
                    yield return null;
                    timer += Time.deltaTime;
                    float t = timer / maxTime;
                    float newAlpha = Mathf.Lerp(0, 5, t);
                    noise.m_AmplitudeGain = newAlpha;
                    if (newAlpha >= 5)
                    {
                        noise.m_AmplitudeGain = 0;
                        yield break;
                    }
                }
            }
        }
        private void PauseCamera(bool isPaused)
        {
            aimCam.m_VerticalAxis.m_MaxSpeed = isPaused ? 0 : Sensitivity;
        }

        private void StartMoving()
        {
            StartCoroutine(WaitForBlend());

            IEnumerator WaitForBlend()
            {
                yield return new WaitForSeconds(1.1f);
                lookCamera.transform.rotation = Quaternion.LookRotation(transform.forward);
                Utility.LockCursor(true);
                PauseCamera(false);
                GameManager.Game = GameManager.GameState.Play;
                GameManager.Player = GameManager.PlayerState.Resting;
            }
        }


        private void OnDisable()
        {
            Utility.LockCursor(false);
        }

        void Update()
        {
            if (GameManager.Game != GameManager.GameState.Play)
                return;

            Movement();
            Rotation();
        }

        public Transform VRHead;

        private void Movement()
        {
            float curSpeedForward = UnityXRInputBridge.instance.GetVec2(XR2DAxisMasks.primary2DAxis, XRHandSide.LeftHand).y;
            float curSpeedRight = UnityXRInputBridge.instance.GetVec2(XR2DAxisMasks.primary2DAxis, XRHandSide.LeftHand).x;
            Vector3 positionChange = VRHead.transform.forward * curSpeedForward + VRHead.transform.right * curSpeedRight;

            positionChange.y = 0f;

            transform.position += playerSpeed * Time.deltaTime * positionChange.normalized;
        }

        private void Rotation()
        {
            //float mouseX = UnityXRInputBridge.instance.GetVec2(XR2DAxisMasks.primary2DAxis, XRHandSide.RightHand).x * Time.deltaTime * Sensitivity * 120f;
            //transform.Rotate(Vector3.up, mouseX);
        }

    }
}