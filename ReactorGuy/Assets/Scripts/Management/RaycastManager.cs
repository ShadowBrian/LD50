using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class RaycastManager : MonoBehaviour
    {
        public static System.Func<(bool isHit, RaycastHit hitData)> GetRaycastHit;
        public static System.Func<(bool isHit, RaycastHit hitData)> GetRaycastHitFronCam;
        public static System.Func<(bool isHit, RaycastHit hitData)> GetRaycastHitTempPlane;

        [SerializeField] private Transform playerCamera;
        [SerializeField] private LayerMask maskTempAndTriggers;
        [SerializeField] private LayerMask maskTemporaryparent;

        public Transform RayObject;

        private void Awake()
        {
            GetRaycastHit = TryAndGetRaycastHit;
            GetRaycastHitTempPlane = TryAndGetRaycastHitOnTempPlane;
            GetRaycastHitFronCam = TryAndGetRaycastHitFrontCamera;
        }

        private (bool, RaycastHit) TryAndGetRaycastHitFrontCamera()
        {
            bool isHit = false;
            Ray ray = new Ray(playerCamera.position, playerCamera.forward);//new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hitData, 2f, maskTempAndTriggers))
                isHit = true;
            return (isHit, hitData);
        }
        private (bool, RaycastHit) TryAndGetRaycastHit()
        {
            bool isHit = false;
            Ray ray = new Ray(RayObject.position, RayObject.forward);
            if (Physics.Raycast(ray, out RaycastHit hitData, 1f, maskTempAndTriggers))
                isHit = true;
            return (isHit, hitData);
        }
        private (bool, RaycastHit) TryAndGetRaycastHitOnTempPlane()
        {
            bool isHit = false;
            Ray ray = new Ray(RayObject.position, RayObject.forward);//Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitData, 1f, maskTemporaryparent))
                isHit = true;
            return (isHit, hitData);
        }


        void Update()
        {
            if (GameManager.Game == GameManager.GameState.Play || GameManager.Game == GameManager.GameState.Minigame)
            {
                Ray ray = new Ray(RayObject.position, RayObject.forward);
                Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
            }
        }


    }
}