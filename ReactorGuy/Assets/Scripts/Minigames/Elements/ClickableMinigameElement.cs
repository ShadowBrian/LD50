using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ClickableMinigameElement : MinigameElementBase
    {
        [SerializeField] private Transform movableElement;
        [SerializeField] private ClickablePosition position;
        private bool isOn;


        protected override void Awake()
        {
            isOn = false;
            base.Awake();
        }

        public override void ResetElement()
        {
            isOn = false;
            movableElement.localPosition = Vector3.zero;
            IsOnProperPosition = position.ShouldBeOn == isOn;
            if(IsOnProperPosition)
            {
                propertyBlock.SetColor("_BaseColor", Color.green);
                meshRenderer.SetPropertyBlock(propertyBlock);
            }
            else
            {
                propertyBlock.SetColor("_BaseColor", Color.red);
                meshRenderer.SetPropertyBlock(propertyBlock);
            }
        }


        public void ChangeState()
        {
            isOn = !isOn;
            if(!isOn)
            {
                movableElement.localPosition = Vector3.zero;
            }
            else
            {
                movableElement.localPosition = Vector3.up * 0.1f;
            }

            IsOnProperPosition = position.ShouldBeOn == isOn;
            if(IsOnProperPosition)
            {
                propertyBlock.SetColor("_BaseColor", Color.green);
                meshRenderer.SetPropertyBlock(propertyBlock);
            }
            else
            {
                propertyBlock.SetColor("_BaseColor", Color.red);
                meshRenderer.SetPropertyBlock(propertyBlock);
            }

            OnChanged?.Invoke();
        }

    }
}