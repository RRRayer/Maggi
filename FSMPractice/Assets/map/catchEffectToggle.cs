using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catchEffectToggle : MonoBehaviour
{
    private Material originalMaterial;
    private Material copiedMaterial;

    [SerializeField]
    private bool isSelected = false;
    private bool previousIsSelected = false;

    public void ToggleMaterial()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null) return;
        if (isSelected)
        {
            renderer.material = copiedMaterial;
        }
        else
        {
            renderer.material = originalMaterial;
        }
    }

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            originalMaterial = renderer.sharedMaterial;
            copiedMaterial = new Material(originalMaterial);
            copiedMaterial.SetFloat("_on_off", 1.0f);
        }
    }

    void OnValidate()
    {
        if (isSelected != previousIsSelected)
        {
            ToggleMaterial();
            previousIsSelected = isSelected;
        }
    }
}
