using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIPause : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader = default;
    [SerializeField] private UIGenericButton _restartButton = default;
    [SerializeField] private UIGenericButton _settingsButton = default;
    [SerializeField] private UIGenericButton _backToMenuButton = default;
    [SerializeField] private UIGenericButton _resumeButton = default;

    [Header("Broadcasting on")]
    [SerializeField] private BoolEventChannelSO _onPauseOpened = default;

    public event UnityAction Restarted = default;
    public event UnityAction SettingScreenOpened = default;
    public event UnityAction BackToMainRequested = default;
    public event UnityAction Resumed = default;

    private void OnEnable()
    {
        _onPauseOpened.RaiseEvent(true);

        _restartButton.Clicked += Restart;
        _settingsButton.Clicked += OpoenSettingScreen;
        _backToMenuButton.Clicked += BackToMainMenuConfirmation;
        _resumeButton.Clicked += CloseScreen;
    }

    private void OnDisable()
    {
        _onPauseOpened.RaiseEvent(false);

        _restartButton.Clicked -= Restart;
        _settingsButton.Clicked -= OpoenSettingScreen;
        _backToMenuButton.Clicked -= BackToMainMenuConfirmation;
        _resumeButton.Clicked -= CloseScreen;
    }

    private void Restart()
    {
        Restarted.Invoke();
    }

    private void OpoenSettingScreen()
    {
        SettingScreenOpened.Invoke();
    }

    private void BackToMainMenuConfirmation()
    {
        BackToMainRequested.Invoke();
    }

    private void CloseScreen()
    {
        Resumed.Invoke();
    }
}
