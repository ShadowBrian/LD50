using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class HoldableMinigameElement : MinigameElementBase
    {
        [SerializeField] private Collider firstCollider;
        [SerializeField] protected Transform followTransform;
        private ProperWirePositionChecker properChecker;
        private Collider lastSnappedCollider;

        protected override void Awake()
        {
            base.Awake();
            lastSnappedCollider = snappedCollider = firstCollider;
            followTransform.position = transform.position;
            if(this is not SliderHoldableMinigameElement && this is not FixHoldableMinigameElement) //XDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD
                properChecker = (ProperWirePositionChecker)checker;
        }

        protected virtual void Update()
        {
            transform.position = Vector3.SmoothDamp(transform.position, followTransform.position, ref velocityRef, 0.1f);
        }

        public virtual void ReleaseItem()
        {
            Vector3 newLocal = snappedCollider.transform.localPosition;

            newLocal.z = -0.5f;
            followTransform.parent = minigameParent;
            followTransform.localPosition = newLocal;

            if(properChecker.properWireColliders.Contains(snappedCollider))
            {
                IsOnProperPosition = true;
                propertyBlock.SetColor("_BaseColor", Color.green);
                meshRenderer.SetPropertyBlock(propertyBlock);
            }
            else
            {
                IsOnProperPosition = false;
                propertyBlock.SetColor("_BaseColor", Color.red);
                meshRenderer.SetPropertyBlock(propertyBlock);
            }

            OnChanged?.Invoke();
        }

        public virtual void ParentToTemporaryTransform(Transform parent)
        {
            followTransform.parent = parent;
            followTransform.localPosition = Vector3.zero;
            propertyBlock.SetColor("_BaseColor", Color.red);
            meshRenderer.SetPropertyBlock(propertyBlock);
        }

        public override void ResetElement()
        {
            if(properChecker.properWireColliders.Contains(snappedCollider))
            {
                IsOnProperPosition = true;
                propertyBlock.SetColor("_BaseColor", Color.green);
                meshRenderer.SetPropertyBlock(propertyBlock);
            }
            else
            {
                IsOnProperPosition = false;
                propertyBlock.SetColor("_BaseColor", Color.red);
                meshRenderer.SetPropertyBlock(propertyBlock);
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Position"))
            {
                lastSnappedCollider = snappedCollider;
                snappedCollider = other;
            }
            else if(other.CompareTag("Wire"))
            {
                snappedCollider = lastSnappedCollider;
            }
        }

    }
}