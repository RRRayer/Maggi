using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIGenericButton : MonoBehaviour
{
    [SerializeField] private MultiInputButton _button = default;

    public UnityAction Clicked = default;

    private void OnDisable()
    {
        _button.IsSelected = false;
    }

    public void Click()
    {
        Clicked.Invoke();
    }
}
