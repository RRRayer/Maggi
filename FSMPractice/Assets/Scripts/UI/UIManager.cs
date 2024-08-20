using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Scene UI")]
    [SerializeField] private UIPopup _popupPanel = default;
    [SerializeField] private UIPause _pauseScreen = default;
    [SerializeField] private UISettingController _settingScreen = default;
    [SerializeField] private UIControl _controlScreen = default;
   
    [Header("Gameplay")]
    [SerializeField] private MenuSO _mainMenu;
    [SerializeField] private InputReader _inputReader = default;
    [SerializeField] private SaveLoadSystem _saveLoadSystem = default; // 얘는 나중에 다른 곳으로 옮겨야 할지도
    [SerializeField] private PointStorageSO _pointStorageSO = default; // 얘는 나중에 다른 곳으로 옮겨야 할지도

    [Header("Broadcasting on")]
    [SerializeField] private LoadEventChannelSO _loadMenuEvent = default;
    [SerializeField] private VoidEventChannelSO _onContinueButton = default;

    private void OnEnable()
    {
        _inputReader.MenuPauseEvent += OpenUIPause;
    }

    private void OnDisable()
    {
        _inputReader.MenuPauseEvent -= OpenUIPause;
    }

    private void OpenUIPause()
    {
        _inputReader.MenuPauseEvent -= OpenUIPause; // you can open UI pause menu again, if it's closed

        Time.timeScale = 0.0f; // Pause Time

        _pauseScreen.Restarted += RestartAtLastSavePoint;
        _pauseScreen.SettingScreenOpened += OpenSettingScreen;
        _pauseScreen.ControlScreenOpened += OpenControlScreen;
        _pauseScreen.Resumed += CloseUIPause;
        _pauseScreen.BackToMainRequested += ShowBackToMenuConfirmationPopup;

        _pauseScreen.gameObject.SetActive(true);

        _inputReader.EnableMenuInput();
    }

    private void RestartAtLastSavePoint()
    {
        CloseUIPause();

        _onContinueButton.RaiseEvent();
    }

    private void CloseUIPause()
    {
        Time.timeScale = 1.0f;

        _inputReader.MenuPauseEvent += OpenUIPause; // you can open UI pause menu again, if it's closed

        _pauseScreen.Restarted -= RestartAtLastSavePoint;
        _pauseScreen.SettingScreenOpened -= OpenSettingScreen;
        _pauseScreen.ControlScreenOpened -= OpenControlScreen;
        _pauseScreen.Resumed -= CloseUIPause;
        _pauseScreen.BackToMainRequested -= ShowBackToMenuConfirmationPopup;

        _pauseScreen.gameObject.SetActive(false);

        _inputReader.EnableGameplayInput();
    }

    private void OpenSettingScreen()
    {
        _settingScreen.Closed += CloseSettingScreen;
        _pauseScreen.gameObject.SetActive(false);
        _settingScreen.gameObject.SetActive(true);
    }

    private void OpenControlScreen()
    {
        _controlScreen.Closed += CloseControlScreen;
        _pauseScreen.gameObject.SetActive(false);
        _controlScreen.gameObject.SetActive(true);
    }

    private void CloseSettingScreen()
    {
        _settingScreen.Closed -= CloseSettingScreen;
        _pauseScreen.gameObject.SetActive(true);
        _settingScreen.gameObject.SetActive(false);
    }

    private void CloseControlScreen()
    {
        _controlScreen.Closed -= CloseControlScreen;
        _pauseScreen.gameObject.SetActive(true);
        _controlScreen.gameObject.SetActive(false);
    }

    private void ShowBackToMenuConfirmationPopup()
    {
        _pauseScreen.gameObject.SetActive(false);

        _popupPanel.ClosePopupAction += HideBackToMenuConfirmationPopup;
        _popupPanel.ConfirmationResponseAction += BackToMainMenu;

        _inputReader.EnableMenuInput();
        _popupPanel.gameObject.SetActive(true);
        _popupPanel.SetPopup(PopupType.BackToMenu);
    }

    private void BackToMainMenu(bool confirm)
    {
        HideBackToMenuConfirmationPopup();// hide confirmation screen, show close UI pause, 

        if (confirm)
        {
            CloseUIPause();
            _loadMenuEvent.RaiseEvent(_mainMenu, false);
        }
    }

    private void HideBackToMenuConfirmationPopup()
    {
        _popupPanel.ClosePopupAction -= HideBackToMenuConfirmationPopup;
        _popupPanel.ConfirmationResponseAction -= BackToMainMenu;

        _popupPanel.gameObject.SetActive(false);
        _pauseScreen.gameObject.SetActive(true);
    }
}
