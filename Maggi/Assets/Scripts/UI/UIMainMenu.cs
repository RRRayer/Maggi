using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private Button _newGameButton = default;
    [SerializeField] private Button _continueButton = default;

    public UnityAction NewGameButtonAction;
    public UnityAction ContinueButtonAction;
    public UnityAction SettingsButtonAction;
    public UnityAction ExitButtonAction;

    public void SetMenuScreen(bool hasSaveData)
    {
        _continueButton.gameObject.SetActive(hasSaveData);
        if (hasSaveData)
        {
            _continueButton.Select();
        }
        else
        {
            _newGameButton.Select();
        }
    }

    public void NewGameButton()
    {
        NewGameButtonAction.Invoke();
    }

    public void ContinueButton()
    {
        ContinueButtonAction.Invoke();
    }

    public void SettingsButton()
    {
        SettingsButtonAction.Invoke();
    }

    public void ExitButton()
    {
        ExitButtonAction.Invoke();
    }
}
