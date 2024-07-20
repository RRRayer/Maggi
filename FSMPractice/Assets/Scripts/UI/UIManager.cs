using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader = default;
    [SerializeField] private SaveLoadSystem _saveLoadSystem = default; // 얘는 나중에 다른 곳으로 옮겨야 할지도
    [SerializeField] private PointStorageSO _pointStorageSO = default; // 얘는 나중에 다른 곳으로 옮겨야 할지도

    [Header("Gameplay")]
    [SerializeField] private MenuSO _mainMenu;

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

    private void Restart()
    {
        _onStartGame.RaiseEvent();
    }

    private void StartNewGame()
    {
        _saveLoadSystem.WriteEmptySaveFile();
        _saveLoadSystem.SetNewGameData();
        _pointStorageSO.lastPointTaken = null;
        _onStartGame.RaiseEvent();
    }

    private void GoMainMenu()
    {
        _loadMenuEvent.RaiseEvent(_mainMenu, false);
    }

    private void OpenUIPause()
    {
        Time.timeScale = 0.0f;

        Debug.Log("퍼즈");
    }
}
