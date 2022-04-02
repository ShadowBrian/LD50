using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class HoldableMinigameElement : MinigameElementBase
    {
        [SerializeField] protected Transform followTransform;
        private ProperWirePositionChecker properChecker;

        protected override void Awake()
        {
            base.Awake();
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


            OnChanged?.Invoke();
        }

        public virtual void ParentToTemporaryTransform(Transform parent)
        {
            followTransform.parent = parent;
            followTransform.localPosition = Vector3.zero;
            propertyBlock.SetColor("_BaseColor", Color.red);
            meshRenderer.SetPropertyBlock(propertyBlock);
        }


        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Position"))
            {
                snappedCollider = other;
            }
        }
    }
}