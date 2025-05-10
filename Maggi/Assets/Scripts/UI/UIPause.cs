using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIPause : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader = default;
    
    [Header("Pause UI")]
    [SerializeField] private UIGenericButton _restartButton = default;
    [SerializeField] private UIGenericButton _settingsButton = default;
    [SerializeField] private UIGenericButton _controlButton = default;
    [SerializeField] private UIGenericButton _backToMenuButton = default;
    [SerializeField] private UIGenericButton _resumeButton = default;

    [Header("Broadcasting on")]
    [SerializeField] private BoolEventChannelSO _onPauseOpened = default;

    public event UnityAction Restarted = default;
    public event UnityAction SettingScreenOpened = default;
    public event UnityAction ControlScreenOpened = default;    
    public event UnityAction Resumed = default;
    public event UnityAction BackToMainRequested = default;

    private void OnEnable()
    {
        _onPauseOpened.RaiseEvent(true);

        _resumeButton.SetButton(true);
        _inputReader.MenuCloseEvent += Resume;
        _restartButton.Clicked += Restart;
        _settingsButton.Clicked += OpenSettingScreen;
        _controlButton.Clicked += OpenControlScreen;
        _backToMenuButton.Clicked += BackToMainMenuConfirmation;
        _resumeButton.Clicked += Resume;
    }

    private void OnDisable()
    {
        _onPauseOpened.RaiseEvent(false);

        _inputReader.MenuCloseEvent -= Resume;
        _restartButton.Clicked -= Restart;
        _settingsButton.Clicked -= OpenSettingScreen;
        _controlButton.Clicked -= OpenControlScreen;
        _backToMenuButton.Clicked -= BackToMainMenuConfirmation;
        _resumeButton.Clicked -= Resume;
    }

    private void Restart()
    {
        Restarted.Invoke();
    }

    private void Resume()
    {
        Debug.Log("Resume");
        Resumed.Invoke();
    }

    private void OpenSettingScreen()
    {
        SettingScreenOpened.Invoke();
    }

    private void OpenControlScreen()
    {
        ControlScreenOpened.Invoke();
    }

    private void BackToMainMenuConfirmation()
    {
        BackToMainRequested.Invoke();
    }

    public void CloseScreen()
    {
        Resumed.Invoke();
    }
}
