using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MultiInputButton : Button
{
    [ReadOnly] public bool IsSelected;

    private MenuSelectionHandler _menuSelectionHandler;

    private new void Awake()
    {
        base.Awake();
        _menuSelectionHandler = transform.root.gameObject.GetComponentInChildren<MenuSelectionHandler>();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (_menuSelectionHandler != null)
            _menuSelectionHandler.HandleMouseEnter(gameObject);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (_menuSelectionHandler != null)
            _menuSelectionHandler.HandleMouseExit(gameObject);
    }

    public void UpdateSelected()
    {
        if (_menuSelectionHandler == null)
            _menuSelectionHandler = transform.root.gameObject.GetComponentInChildren<MenuSelectionHandler>();

        _menuSelectionHandler.UpdateSelection(gameObject);
    }
}
