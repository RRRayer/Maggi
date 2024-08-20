using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPrototype : MonoBehaviour
{
    [SerializeField] private UIPopup _popupPanel = default;
    [SerializeField] private InputReader _inputReader = default;

    [Header("Gameplay")]
    [SerializeField] private MenuSO _mainMenu;

    [Header("Broadcasting on")]
    [SerializeField] private BoolEventChannelSO _toggleLoadingScreen = default;
    [SerializeField] private FadeChannelSO _fadeRequestChannel = default;
    [SerializeField] private LoadEventChannelSO _loadMenuEvent = default;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowThanksPopup();
        }
    }

    private void ShowThanksPopup()
    {
        Time.timeScale = 0.0f;

        _popupPanel.ConfirmationResponseAction += BackToMainMenu;

        _popupPanel.gameObject.SetActive(true);
        _popupPanel.SetPopup(PopupType.DonePrototype);

        _inputReader.EnableMenuInput();
    }

    private void BackToMainMenu(bool confirm)
    {
        Time.timeScale = 1.0f;

        HideThanksPopup();// hide confirmation screen, show close UI pause, 

        if (confirm)
        {
            _loadMenuEvent.RaiseEvent(_mainMenu, false);
        }
    }

    private void HideThanksPopup()
    {
        _popupPanel.ConfirmationResponseAction -= BackToMainMenu;
        _popupPanel.gameObject.SetActive(false);
    }
}
