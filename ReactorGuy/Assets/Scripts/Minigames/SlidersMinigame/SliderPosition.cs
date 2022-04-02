using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPosition : MonoBehaviour
{
    public float ProperValue { get; private set; }

    public void SetSlider(float value)
    {
        ProperValue = value;
    }
}
