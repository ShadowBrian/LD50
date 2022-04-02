using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickablePosition : MonoBehaviour
{

    public bool ShouldBeOn { get; private set; }

    public void SetClickable(bool isOn)
    {
        ShouldBeOn = isOn;
    }
}
