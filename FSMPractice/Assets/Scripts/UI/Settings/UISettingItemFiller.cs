using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UISettingItemFiller : MonoBehaviour
{
    [SerializeField] private SettingFieldType _fieldType = default;

    [SerializeField] private MultiInputButton _buttonNext = default;
    [SerializeField] private MultiInputButton _buttonPrevious = default;

    public event UnityAction OnNextOption = delegate { };
    public event UnityAction OnPreviousOption = delegate { };

    public void FillSettingField(int paginationCount, int selectedPaginationIndex, string selectedOption)
    {
        // Pagination 관련 로직 추가 - UI

        //_buttonNext.interactable = (selectedPaginationIndex)
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
