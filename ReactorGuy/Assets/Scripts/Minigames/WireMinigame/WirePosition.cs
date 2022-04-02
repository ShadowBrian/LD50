using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WirePosition : MonoBehaviour
{
    private Renderer meshRenderer;
    private MaterialPropertyBlock propertyBlock;

    private void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();
        meshRenderer = GetComponent<Renderer>();
        meshRenderer.GetPropertyBlock(propertyBlock);

    }

    public void TurnOn()
    {
        propertyBlock.SetColor("_BaseColor", Color.yellow);
        meshRenderer.SetPropertyBlock(propertyBlock);
    }

    public void TurnOff()
    {
        propertyBlock.SetColor("_BaseColor", Color.white);
        meshRenderer.SetPropertyBlock(propertyBlock);
    }
}
