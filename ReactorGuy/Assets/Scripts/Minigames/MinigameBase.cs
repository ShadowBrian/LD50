using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Game {
    public class MinigameBase : MonoBehaviour
    {
        public static System.Action<bool> OnMinigame;

        public ProperPositionChecker checker;
        [SerializeField] private Transform tempParentTransform;
        [SerializeField] private Plane holdingPlane;
        [SerializeField] private CinemachineVirtualCamera vCam;
        [SerializeField] private List<MinigameElementBase> elements;
        private bool isMinigameActive;
        private bool isMinigameOnCooldown;
        private MinigameElementBase holdingElement;
        private float cooldown = 0;
        private readonly float cooldownTime = 5f;


        private void Start()
        {
            foreach(var element in elements)
            {
                element.OnChanged += CheckIfMinigameDone;
            }
            PrepareMinigame();
            isMinigameActive = false;
            Controlls.OnMouseDown += MouseDown;
            Controlls.OnMouseUp += MouseUp;
        }
        private void OnDestroy()
        {
            Controlls.OnMouseDown -= MouseDown;
            Controlls.OnMouseUp -= MouseUp;
        }

        private void Update()
        {
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

            if(Input.GetKeyUp(KeyCode.Space) && isMinigameActive)
                EndMinigame();
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
                    if(item.type == MinigameElementBase.Type.Clickable)
                    {
                        item.ChangeState();
                    }
                    else if(item.type == MinigameElementBase.Type.Holdable)
                    {
                        holdingElement = item;
                        tempParentTransform.localPosition = holdingElement.transform.localPosition;
                        item.ParentToTemporaryTransform(tempParentTransform);
                    }
                }
            }
        }

        private void MouseUp()
        {
            if(!isMinigameActive)
                return;

            if(holdingElement)
            {
                holdingElement.ReleaseItem();
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

        public void TryActivateMinigame()
        {
            GameManager.Game = GameManager.GameState.Minigame;
            Utility.LockCursor(false);
            vCam.gameObject.SetActive(true);
            isMinigameActive = true;
            OnMinigame?.Invoke(true);
        }
        public void EndMinigame()
        {
            vCam.gameObject.SetActive(false);
            Utility.LockCursor(true);
            isMinigameActive = false;
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