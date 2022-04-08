using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCapsuleOnPlayerHead : MonoBehaviour
{
    public GameObject head;
    public CapsuleCollider col;

    void Start()
    {

    }

    void Update()
    {
        Vector3 difference = col.transform.InverseTransformPoint(head.transform.position);
        difference.y = 0;
        col.center = difference;

        if (UnityXRInputBridge.instance.GetButtonDown(XRButtonMasks.primary2DAxisLeft, XRHandSide.RightHand))
        {
            head.transform.parent.parent.RotateAround(head.transform.position, Vector3.up, -45f);
        }

        if (UnityXRInputBridge.instance.GetButtonDown(XRButtonMasks.primary2DAxisRight, XRHandSide.RightHand))
        {
            head.transform.parent.parent.RotateAround(head.transform.position, Vector3.up, 45f);
        }
    }
}
