using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class RaycastManager : MonoBehaviour
    {
        public static System.Func<(bool isHit,RaycastHit hitData)> GetRaycastHit;
        public static System.Func<(bool isHit,RaycastHit hitData)> GetRaycastHitTempPlane;

        [SerializeField] private Transform playerCamera;
        [SerializeField] private LayerMask maskTempAndTriggers;
        [SerializeField] private LayerMask maskTemporaryparent;

        private void Awake()
        {
            GetRaycastHit = TryAndGetRaycastHit;
            GetRaycastHitTempPlane = TryAndGetRaycastHitOnTempPlane;
        }

        private (bool, RaycastHit) TryAndGetRaycastHit()
        {
            bool isHit = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hitData, 1000, maskTempAndTriggers))
                isHit = true;
            return (isHit, hitData);
        }
        private (bool, RaycastHit) TryAndGetRaycastHitOnTempPlane()
        {
            bool isHit = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hitData, 1000, maskTemporaryparent))
                isHit = true;
            return (isHit, hitData);
        }

#if UNITY_EDITOR
        void Update()
        {
            if(GameManager.Game == GameManager.GameState.Play || GameManager.Game == GameManager.GameState.Minigame)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
            } 
        }
#endif

    }
}