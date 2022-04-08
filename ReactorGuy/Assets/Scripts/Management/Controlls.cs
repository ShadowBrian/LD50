using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Controlls : MonoBehaviour
    {
        public static System.Action<bool> OnPause;
        public static System.Action<RaycastHit> OnMouseDown;
        public static System.Action OnMouseDownF;
        public static System.Action OnMouseUp;
        public static System.Action OnMouseRightDown;
        GameManager.GameState lastState;

        private bool isHoldingMouse0;
        private bool isStop;

        // Update is called once per frame
        void Update()
        {
            //if(Input.GetKeyDown(KeyCode.Escape))
            //Cursor.lockState = CursorLockMode.None;


            if (GameManager.Game == GameManager.GameState.Start || GameManager.Game == GameManager.GameState.End)
                return;


            if (UnityXRInputBridge.instance.GetButtonDown(XRButtonMasks.secondaryButton, XRHandSide.RightHand) || UnityXRInputBridge.instance.GetButtonDown(XRButtonMasks.secondaryButton, XRHandSide.LeftHand))
            {
                if (isStop)
                {
                    GameManager.Game = lastState;

                    Time.timeScale = 1;
                    isStop = false;
                    OnPause?.Invoke(false);
                }
                else
                {
                    lastState = GameManager.Game;
                    GameManager.Game = GameManager.GameState.Paused;
                    Time.timeScale = 0;
                    isStop = true;
                    OnPause?.Invoke(true);
                }
            }

            if (UnityXRInputBridge.instance.GetButtonDown(XRButtonMasks.triggerButton, XRHandSide.LeftHand) || UnityXRInputBridge.instance.GetButtonDown(XRButtonMasks.triggerButton, XRHandSide.RightHand))
            {
                OnMouseDownF?.Invoke();
                isHoldingMouse0 = true;
            }
            if (UnityXRInputBridge.instance.GetButtonUp(XRButtonMasks.triggerButton, XRHandSide.LeftHand) || UnityXRInputBridge.instance.GetButtonUp(XRButtonMasks.triggerButton, XRHandSide.RightHand))
            {
                if (isHoldingMouse0)
                {
                    isHoldingMouse0 = false;
                    OnMouseUp?.Invoke();
                }
            }

            if (UnityXRInputBridge.instance.GetButtonDown(XRButtonMasks.primaryButton, XRHandSide.RightHand) || UnityXRInputBridge.instance.GetButtonDown(XRButtonMasks.primaryButton, XRHandSide.LeftHand))
            {
                OnMouseRightDown?.Invoke();
            }
        }
    }
}