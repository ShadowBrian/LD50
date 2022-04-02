using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Game {
    public class MinigameBase : MonoBehaviour
    {
        public static System.Action<bool> OnMinigame;
        public static System.Action OnMinigameFinished;

        public ProperPositionCheckerBase checker;
        [SerializeField] private Transform tempParentTransform;
        [SerializeField] private Plane holdingPlane;
        [SerializeField] private CinemachineVirtualCamera vCam;
        [SerializeField] private List<MinigameElementBase> elements;
        private bool isMinigameActive;
        private bool isMinigameOnCooldown;
        private MinigameElementBase holdingElement;
        private float cooldown = 0;
        private readonly float cooldownTime = 5f;


        protected virtual void Start()
        {
            foreach(var element in elements)
            {
                element.OnChanged += CheckIfMinigameDone;
            }
            PrepareMinigame();
            isMinigameActive = false;
            Controlls.OnMouseDown += MouseDown;
            Controlls.OnMouseUp += MouseUp;
            Controlls.OnMouseRightDown += ExitMinigame;
            Reactor.OnReactorOverheat += EmergencyExitMinigame;
            Player.OnPlayerRadioactive += EmergencyExitMinigame;
            
        }
        private void OnDestroy()
        {
            Controlls.OnMouseDown -= MouseDown;
            Controlls.OnMouseUp -= MouseUp;
            Controlls.OnMouseRightDown -= ExitMinigame;
            Reactor.OnReactorOverheat += EmergencyExitMinigame;
            Player.OnPlayerRadioactive += EmergencyExitMinigame;
        }

        protected virtual void Update()
        {
            if(GameManager.Game == GameManager.GameState.Paused)
                return;

            if(isMinigameOnCooldown)
            {
                cooldown += Time.deltaTime;
                if(cooldown > cooldownTime)
                {
                    PrepareMinigame();
                }
                return;
            }

            if(!isMinigameActive)
                return;

            if(holdingElement)
            {
                (bool isHit, RaycastHit hitData) = RaycastManager.GetRaycastHitTempPlane();
                if(isHit)
                {
                    tempParentTransform.position = hitData.point;
                }
            }
        }

        private void PrepareMinigame()
        {
            isMinigameOnCooldown = false;
            cooldown = 0;
            checker.CheckPosition();
            foreach(var element in elements)
            {
                element.ResetElement();
            }
        }

        private void MouseDown(RaycastHit hitData)
        {
            if(!isMinigameActive)
                return;

            foreach(var item in elements)
            {
                if(ReferenceEquals(item.transform, hitData.transform))
                {
                    if(item is ClickableMinigameElement clickable)
                    {
                        clickable.ChangeState();
                    }
                    else if(item is HoldableMinigameElement holdable)
                    {
                        holdingElement = holdable;
                        tempParentTransform.localPosition = holdingElement.transform.localPosition;
                        holdable.ParentToTemporaryTransform(tempParentTransform);
                    }
                }
            }
        }

        private void MouseUp()
        {
            if(!isMinigameActive)
                return;

            if(holdingElement && holdingElement is HoldableMinigameElement holdable)
            {
                holdable.ReleaseItem();
                holdingElement = null;
            }
        }


        private void CheckIfMinigameDone()
        {
            bool isDone = true;
            foreach(var item in elements)
            {
                isDone &= item.IsOnProperPosition;
            }
            Debug.Log("Minigame done? " + isDone);
            if(isDone)
                EndMinigame();
        }

        public virtual bool TryActivateMinigame()
        {
            if(isMinigameOnCooldown)
                return false;

            GameManager.Game = GameManager.GameState.Minigame;
            Utility.LockCursor(false);
            vCam.gameObject.SetActive(true);
            isMinigameActive = true;
            OnMinigame?.Invoke(true);
            return true;
        }

        public virtual void ExitMinigame()
        {
            if(!isMinigameActive)
                return;

            vCam.gameObject.SetActive(false);
            Utility.LockCursor(true);
            StartCoroutine(WaitForBlend());

            IEnumerator WaitForBlend()
            {
                yield return new WaitForSeconds(1f);
                GameManager.Game = GameManager.GameState.Play;
                OnMinigame?.Invoke(false);
            }
        }
        private void EmergencyExitMinigame()
        {
            vCam.gameObject.SetActive(false);
            Utility.LockCursor(false);
        }

        public virtual void EndMinigame()
        {
            vCam.gameObject.SetActive(false);
            Utility.LockCursor(true);
            isMinigameActive = false;
            OnMinigameFinished?.Invoke();
            StartCoroutine(WaitForBlend());

            IEnumerator WaitForBlend()
            {
                yield return new WaitForSeconds(1f);
                GameManager.Game = GameManager.GameState.Play;
                isMinigameOnCooldown = true;
                OnMinigame?.Invoke(false);
            }
        }
    }
}