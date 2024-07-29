using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleEffect : MonoBehaviour
{
    [SerializeField] private bool _isMeshInChild = false;

    private Material _originalMaterial;
    private Material _copiedMaterial;
    private Renderer _renderer;

    void Start()
    {
        if (_isMeshInChild)
            _renderer = transform.GetChild(0).GetComponent<Renderer>();
        else
            _renderer = GetComponent<Renderer>();

        if (_renderer != null)
        {
            _originalMaterial = _renderer.sharedMaterial;
            _copiedMaterial = new Material(_originalMaterial);
            _copiedMaterial.SetFloat("_on_off", 1.0f);
        }
    }

    public void ToggleMaterial(bool isSelected)
    {
        if (_renderer == null)
        {
            Debug.LogWarning("There is no render _ ToggleEffect.cs");
            return;
        }

        if (isSelected)
        {
            _renderer.material = _copiedMaterial;
        }
        else
        {
            _renderer.material = _originalMaterial;
        }
    }
}
