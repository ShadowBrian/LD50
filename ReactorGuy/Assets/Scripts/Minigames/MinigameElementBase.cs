using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class MinigameElementBase : MonoBehaviour
    {
        public System.Action OnChanged;
        [SerializeField] private Transform followTransform;
        private ProperPositionChecker checker;
        private Transform minigameParent;
        private Collider snappedCollider;
        public bool IsOnProperPosition { get; private set; }
        public enum Type { Clickable, Holdable }
        public Type type;

        private Vector3 velocityRef;
        private Renderer meshRenderer;
        private MaterialPropertyBlock propertyBlock;

        private void Awake()
        {
            followTransform.position = transform.position;
            minigameParent = transform.parent;
            checker = GetComponentInParent<MinigameBase>().checker;

            propertyBlock = new MaterialPropertyBlock();
            meshRenderer = GetComponentInChildren<Renderer>();
            meshRenderer.GetPropertyBlock(propertyBlock);
        }

        private void Update()
        {
            transform.position = Vector3.SmoothDamp(transform.position, followTransform.position, ref velocityRef, 0.1f);
        }

        public void ChangeState()
        {
            IsOnProperPosition = true;
            OnChanged?.Invoke();
        }
        public void ParentToTemporaryTransform(Transform parent)
        {
            followTransform.parent = parent;
            followTransform.localPosition = Vector3.zero;
            propertyBlock.SetColor("_BaseColor", Color.red);
            meshRenderer.SetPropertyBlock(propertyBlock);
        }
        public void ReleaseItem()
        {
            Vector3 newLocal = snappedCollider.transform.localPosition;

            newLocal.z = -0.5f;
            followTransform.parent = minigameParent;
            followTransform.localPosition = newLocal;

            if(checker.properWireColliders.Contains(snappedCollider))
            {
                IsOnProperPosition = true;
                propertyBlock.SetColor("_BaseColor", Color.green);
                meshRenderer.SetPropertyBlock(propertyBlock);
            }


            OnChanged?.Invoke();
        }

        public void ResetElement()
        {
            IsOnProperPosition = false;
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