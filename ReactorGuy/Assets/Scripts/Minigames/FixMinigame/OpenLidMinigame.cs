using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class OpenLidMinigame : MinigameBase
    {
        [SerializeField] private Transform lid;
        [SerializeField] private Transform lidOpen;
        [SerializeField] private Transform lidClosed;
        private Vector3 targetForwardVector;
        private Vector3 targetUpVector;
        private Vector3 velocityRef;



        private void OpenLid()
        {
            targetForwardVector = lidOpen.forward;
            targetUpVector = lidOpen.up;
        }
        private void CloseLid()
        {
            targetForwardVector = lidClosed.forward;
            targetUpVector = lidClosed.up;
        }

        protected override void Start()
        {
            targetForwardVector = lidClosed.forward;
            targetUpVector = lidClosed.up;
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
            Vector3 forwardVector = Vector3.SmoothDamp(lid.forward, targetForwardVector, ref velocityRef, 0.2f);
            lid.rotation = Quaternion.LookRotation(forwardVector, lid.up);
        }


        public override bool TryActivateMinigame()
        {
            if(!base.TryActivateMinigame())
                return false;
            OpenLid();
            return true;
        }

        public override void ExitMinigame()
        {
            CloseLid();
            base.ExitMinigame();
        }

        public override void EndMinigame()
        {
            CloseLid();
            base.EndMinigame();
        }



    }
}