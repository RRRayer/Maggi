using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuManager : MonoBehaviour
{
    [SerializeField] private UIMainMenu _mainMenuPanel = default;

    [Header("Broadcasting on")]
    [SerializeField] private VoidEventChannelSO _startNewGameEvent = default;
    [SerializeField] private VoidEventChannelSO _continueGameEvent = default;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.4f);
        SetMenuScreen();
    }

    private void SetMenuScreen()
    {
        _mainMenuPanel.NewGameButtonAction += ButtonStartNewGameClicked;
        _mainMenuPanel.ContinueButtonAction += ButtonContinueGameClicked;
    }

    private void ButtonStartNewGameClicked()
    {
        _startNewGameEvent.OnEventRaised();
    }

    private void ButtonContinueGameClicked()
    {
        _continueGameEvent.OnEventRaised();
    }
}
