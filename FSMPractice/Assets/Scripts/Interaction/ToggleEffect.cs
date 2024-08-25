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
    private bool _isSelected;

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
            _isSelected = false;
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
            //Debug.Log("_scale 0.5f");
            _isSelected = true;
            _copiedMaterial.SetFloat("_scale", 0.5f);
            _renderer.material = _copiedMaterial;
        }
        else
        {
            _isSelected = false;
            _renderer.material = _originalMaterial;
        }
    }
    /*
    public void HoverMaterial(bool isHover)
    {
        if (_renderer == null)
        {
            Debug.LogWarning("There is no render _ ToggleEffect.cs");
            return;
        }

        if (isHover)
        {
            _copiedMaterial.SetFloat("_scale", 1.5f);
            _renderer.material = _copiedMaterial;
        }
        else
        {
            _renderer.material = _originalMaterial;
        }
    }*/

    void OnMouseEnter()
    {
        //Debug.Log("OnMouseEnter");
        if (_renderer == null)
        {
            Debug.LogWarning("There is no render _ ToggleEffect.cs");
            return;
        }
        if (!_isSelected) {
            _copiedMaterial.SetFloat("_scale", 1.5f);
            _renderer.material = _copiedMaterial;
        }
        
    }

    void OnMouseExit()
    {
        if (_renderer == null)
        {
            Debug.LogWarning("There is no render _ ToggleEffect.cs");
            return;
        }
        if (!_isSelected)
        {
            _renderer.material = _originalMaterial;
        }
    }
}
