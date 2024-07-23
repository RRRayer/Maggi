using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Scene UI")]
    [SerializeField] private UIPause _pauseScreen = default;
   
    [Header("Gameplay")]
    [SerializeField] private MenuSO _mainMenu;
    [SerializeField] private InputReader _inputReader = default;
    [SerializeField] private SaveLoadSystem _saveLoadSystem = default; // 얘는 나중에 다른 곳으로 옮겨야 할지도
    [SerializeField] private PointStorageSO _pointStorageSO = default; // 얘는 나중에 다른 곳으로 옮겨야 할지도

    [Header("Broadcasting on")]
    [SerializeField] private LoadEventChannelSO _loadMenuEvent = default;
    [SerializeField] private VoidEventChannelSO _onStartGame = default;

    private void OnEnable()
    {
        _inputReader.MenuPauseEvent += OpenUIPause;
    }

    private void OnDisable()
    {
        _inputReader.MenuPauseEvent -= OpenUIPause;
    }

    private void StartNewGame() // 얘는 메인메뉴로 옮겨야겠다
    {
        _saveLoadSystem.WriteEmptySaveFile();
        _saveLoadSystem.SetNewGameData();
        _pointStorageSO.lastPointTaken = null;
        _onStartGame.RaiseEvent();
    }

    private void OpenUIPause()
    {
        _inputReader.MenuPauseEvent -= OpenUIPause;

        Debug.Log("퍼즈");
        Time.timeScale = 0.0f; // Pause Time

        _pauseScreen.Restarted += RestartAtLastSavePoint;
        _pauseScreen.SettingScreenOpened += OpenSettingScreen;
        _pauseScreen.Resumed += CloseUIPause;
        _pauseScreen.BackToMainRequested += ShowBackToMenuConfirmationPopup;

        _pauseScreen.gameObject.SetActive(true);
    }

    private void RestartAtLastSavePoint()
    {
        _onStartGame.RaiseEvent();
    }

    private void OpenSettingScreen()
    {
        Debug.Log("설정창 열기");
    }

    private void CloseUIPause()
    {
        Time.timeScale = 1.0f;

        _inputReader.MenuPauseEvent += OpenUIPause;

        Debug.Log("메뉴 닫기");

        _pauseScreen.Restarted -= RestartAtLastSavePoint;
        _pauseScreen.SettingScreenOpened -= OpenSettingScreen;
        _pauseScreen.Resumed -= CloseUIPause;
        _pauseScreen.BackToMainRequested -= ShowBackToMenuConfirmationPopup;

        _pauseScreen.gameObject.SetActive(false);
    }

    private void ShowBackToMenuConfirmationPopup()
    {
        _loadMenuEvent.RaiseEvent(_mainMenu, false);
    }
}
