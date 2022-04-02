using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderValueGetter : MonoBehaviour
{
    [SerializeField] private Transform up;
    [SerializeField] private Transform down;
    
    private static Transform upS;
    private static Transform downS;

    private void Awake()
    {
        upS = up;
        downS = down;
    }

    public static float Value(float worldY)
    {
        float max = upS.position.y;
        float min = downS.position.y;
        return Mathf.Clamp01(Mathf.InverseLerp(min, max, worldY));
    }


}
