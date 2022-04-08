using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayObjectController : MonoBehaviour
{
    public Transform LHand, RHand;

    public bool IsLeft;

    // Update is called once per frame
    void Update()
    {
        if (UnityXRInputBridge.instance.GetButtonDown(XRButtonMasks.triggerButton, XRHandSide.LeftHand))
        {
            IsLeft = true;
        }

        if (UnityXRInputBridge.instance.GetButtonDown(XRButtonMasks.triggerButton, XRHandSide.RightHand))
        {
            IsLeft = false;
        }

        if (IsLeft)
        {
            transform.position = LHand.position;
            transform.rotation = LHand.rotation;
        }
        else
        {
            transform.position = RHand.position;
            transform.rotation = RHand.rotation;
        }
    }
}
