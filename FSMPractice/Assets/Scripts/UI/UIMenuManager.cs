using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuManager : MonoBehaviour
{
    [SerializeField] private UIMainMenu _mainMenuPanel = default;

    [Header("Broadcasting on")]
    [SerializeField] private VoidEventChannelSO _startGameEvent = default;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.4f);
        SetMenuScreen();
    }

    private void SetMenuScreen()
    {
        _mainMenuPanel.StartGameAction += ButtonStartGameClicked;
    }

    private void ButtonStartGameClicked()
    {
        _startGameEvent.OnEventRaised();
    }
}
