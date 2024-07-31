using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuManager : MonoBehaviour
{
    [SerializeField] InputReader _inputReader = default;
    [SerializeField] private UIMainMenu _mainMenuPanel = default;
    [SerializeField] private SaveLoadSystem _saveLoadSystem = default;

    [Header("Broadcasting on")]
    [SerializeField] private VoidEventChannelSO _startNewGameEvent = default;
    [SerializeField] private VoidEventChannelSO _continueGameEvent = default;

    private bool _hasSaveData = false;

    private IEnumerator Start()
    {
        _inputReader.EnableMenuInput();
        yield return new WaitForSeconds(0.4f);
        SetMenuScreen();
    }

    private void SetMenuScreen()
    {
        _hasSaveData = _saveLoadSystem.LoadSaveDataFromDisk();
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
