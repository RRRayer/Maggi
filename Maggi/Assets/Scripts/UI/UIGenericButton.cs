using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UIGenericButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _buttonText = default;
    [SerializeField] private MultiInputButton _button = default;

    public UnityAction Clicked = default;

    private bool _isDefaultSelection = false;

    private void OnDisable()
    {
        _button.IsSelected = false;
        _isDefaultSelection = false;
    }

    public void Click()
    {
        Clicked.Invoke();
    }

    public void SetButton(bool isSelect)
    {
        _isDefaultSelection = isSelect;
        if (isSelect)
            _button.UpdateSelected();
    }

    public void SetButton(string buttonText, bool isSelected)
    {
        _buttonText.text = buttonText;

        if (isSelected)
            SelectButton();
    }

    public void SelectButton()
    {
        _button.Select();
    }
}
