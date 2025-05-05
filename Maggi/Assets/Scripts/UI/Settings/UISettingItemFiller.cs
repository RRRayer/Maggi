using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UISettingItemFiller : MonoBehaviour
{
    [SerializeField] private SettingFieldType _fieldType = default;
    [SerializeField] private TextMeshProUGUI _title = default;
    [SerializeField] private TextMeshProUGUI _currentSelectedOption_Text = default;
    [SerializeField] private MultiInputButton _buttonNext = default;
    [SerializeField] private MultiInputButton _buttonPrevious = default;

    public event UnityAction OnNextOption = delegate { };
    public event UnityAction OnPreviousOption = delegate { };

    public void FillSettingField(int paginationCount, int selectedPaginationIndex, string selectedOption_int)
    {
        _title.text = _fieldType.ToString();
        _currentSelectedOption_Text.text = selectedOption_int.ToString();

        _buttonNext.interactable = (selectedPaginationIndex < paginationCount - 1);
        _buttonPrevious.interactable = (selectedPaginationIndex > 0);
    }

    public void NextOption()
    {
        OnNextOption.Invoke();
    }

    public void PreviousOption()
    {
        OnPreviousOption.Invoke();
    }
}
