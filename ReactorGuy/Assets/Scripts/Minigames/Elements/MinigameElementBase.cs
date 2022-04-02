using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class MinigameElementBase : MonoBehaviour
    {
        public System.Action OnChanged;
        protected ProperPositionCheckerBase checker;
        protected Transform minigameParent;
        protected Collider snappedCollider;
        public bool IsOnProperPosition { get; protected set; }


        protected Vector3 velocityRef;
        protected Renderer meshRenderer;
        protected MaterialPropertyBlock propertyBlock;

        protected virtual void Awake()
        {
            minigameParent = transform.parent;
            checker = GetComponentInParent<MinigameBase>().checker;

            propertyBlock = new MaterialPropertyBlock();
            meshRenderer = GetComponentInChildren<Renderer>();
            meshRenderer.GetPropertyBlock(propertyBlock);
        }

        public virtual void ResetElement()
        {
            IsOnProperPosition = false;
            propertyBlock.SetColor("_BaseColor", Color.red);
            meshRenderer.SetPropertyBlock(propertyBlock);
        }



    }
}