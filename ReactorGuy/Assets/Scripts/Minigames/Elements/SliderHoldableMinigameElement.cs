using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SliderHoldableMinigameElement : HoldableMinigameElement
    {
        [SerializeField] private SliderPosition position;

        protected override void Update()
        {
            Vector3 smoothedWorldPosition = Vector3.SmoothDamp(transform.position, followTransform.position, ref velocityRef, 0.1f);
            Vector3 smoothedLocalPosition = transform.parent.InverseTransformPoint(smoothedWorldPosition);
            smoothedLocalPosition.x = transform.localPosition.x;
            smoothedLocalPosition.z = transform.localPosition.z;
            transform.localPosition = smoothedLocalPosition;


#if UNITY_EDITOR
            Debug.DrawLine(followTransform.position, followTransform.position + followTransform.forward);
            Vector3 downPosition = followTransform.position;
            Vector3 upPosition = followTransform.position;
            (float downY, float upY) = SliderValueGetter.GetY();
            downPosition.y = downY;
            upPosition.y = upY;
            Debug.DrawLine(downPosition, upPosition);
#endif


        }

        public override void ResetElement()
        {
            Vector3 localPos = followTransform.localPosition;
            localPos.y = 0;
            localPos.z = transform.localPosition.z;
            followTransform.localPosition = localPos;

            IsOnProperPosition = Mathf.Abs(SliderValueGetter.Value(followTransform.position.y) - position.ProperValue) < 0.1f;
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

        public override void ReleaseItem()
        {
            followTransform.parent = minigameParent;

            IsOnProperPosition = Mathf.Abs(SliderValueGetter.Value(transform.position.y) - position.ProperValue) < 0.1f;

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