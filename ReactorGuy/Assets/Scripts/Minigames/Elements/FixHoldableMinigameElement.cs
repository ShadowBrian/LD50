using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class FixHoldableMinigameElement : HoldableMinigameElement
    {
        [SerializeField] private ProperFixChecker fixChecker;
        private Vector3 initialPosition;
        private bool isDroppedNearWire;


        protected override void Awake()
        {
            base.Awake();
            initialPosition = transform.position;
        }

        public override void ResetElement()
        {
            isDroppedNearWire = false;
        }

        public override void ReleaseItem()
        {
            if(isDroppedNearWire)
            {
                fixChecker.CheckTool(gameObject.name);
            }
            isDroppedNearWire = false;
            followTransform.parent = null;
            followTransform.position = initialPosition;
            followTransform.parent = minigameParent;

            OnChanged?.Invoke();
        }

        public override void ParentToTemporaryTransform(Transform parent)
        {
            followTransform.parent = parent;
            followTransform.localPosition = Vector3.zero;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Position"))
            {
                isDroppedNearWire = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Position"))
            {
                isDroppedNearWire = false;
            }
        }
    }
}