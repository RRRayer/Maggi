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
        _menuSelectionHandler.HandleMouseEnter(gameObject);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        _menuSelectionHandler.HandleMouseExit(gameObject);
    }
}
