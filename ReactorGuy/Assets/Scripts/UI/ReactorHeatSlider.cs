using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReactorHeatSlider : MonoBehaviour
{
    [SerializeField] private Image image;
    private Material material;
    void Awake()
    {
        material = new Material(image.material);
        image.material = material;

        material.SetFloat("_Percentage", 0);
    }

    private void Update()
    {
        material.SetFloat("_Percentage", Game.Reactor.ReactorHeat);
    }
}
