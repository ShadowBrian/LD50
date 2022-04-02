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
        [SerializeReference] private float sensitivity = 1f;
        [SerializeReference] private float playerSpeed = 0.04f;
        private CinemachinePOV aimCam;
        //private float xRotation = 0;

        private void Awake()
        {
            aimCam = lookCamera.GetCinemachineComponent<CinemachinePOV>();
            GameStarter.OnGameStart += StartMoving;
            Controlls.OnPause += PauseCamera;
            MinigameBase.OnMinigame += PauseCamera;
        }
        private void OnDestroy()
        {
            GameStarter.OnGameStart -= StartMoving;
            Controlls.OnPause -= PauseCamera;
            MinigameBase.OnMinigame -= PauseCamera;
        }

        private void PauseCamera(bool isPaused)
        {
            aimCam.m_VerticalAxis.m_MaxSpeed = isPaused ? 0 : 1;
        }

        private void StartMoving()
        {
            StartCoroutine(WaitForBlend()); 

            IEnumerator WaitForBlend()
            {
                yield return new WaitForSeconds(1.1f);
                lookCamera.transform.rotation = Quaternion.LookRotation(transform.forward);
                Utility.LockCursor(true);
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
            if(GameManager.Game != GameManager.GameState.Play)
                return;

            Movement();
            Rotation();
        }

        private void Movement()
        {
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;
            float curSpeedForward = playerSpeed * Input.GetAxisRaw("Vertical");
            float curSpeedRight = playerSpeed * Input.GetAxisRaw("Horizontal");
            transform.position += forward * curSpeedForward;
            transform.position += right * curSpeedRight;
        }

        private void Rotation()
        {
            float characterRotation = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity * 120f;
            Quaternion characterRotate = Quaternion.AngleAxis(characterRotation, Vector3.up);
            transform.rotation *= characterRotate;
        }

    }
}