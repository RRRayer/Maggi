using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public enum SettingFieldType
{
    Volume_Master,
    Volume_Music,
    Volume_Sfx,
    Resolution,
    FullScreen,
    AntiAliasing,
    ShadowDistance,
    ShadowQuality,
}

public enum SettingsType
{
    Audio,
    Graphic,
}

public class UISettingController : MonoBehaviour
{
    [SerializeField] private UISettingsAudioComponent _audioComponent;
    [SerializeField] private UISettingsGraphicsComponent _graphicsComponent;
    [SerializeField] private SettingsSO _currentSettings = default;
    [SerializeField] private InputReader _inputReader = default;

    [Header("Broadcasting on")]
    [SerializeField] private VoidEventChannelSO _saveSettingEvent = default;

    public UnityAction Closed;

    private void OnEnable()
    {
        _audioComponent._save += SaveAudioSettings;
        _graphicsComponent._save += SaveGraphicsSettings;
        _inputReader.MenuCloseEvent += CloseScreen;

        OpenSetting();
    }

    private void OnDisable()
    {
        _audioComponent._save -= SaveAudioSettings;
        _inputReader.MenuCloseEvent -= CloseScreen;
    }

    public void CloseScreen()
    {
        Closed.Invoke();
    }

    private void OpenSetting()
    {
        _audioComponent.Setup(_currentSettings.MasterVolume, _currentSettings.MusicVolume, _currentSettings.SfxVolume);
        _graphicsComponent.Setup();
    }

    private void SaveAudioSettings(float _masterVolume, float _musicVolume, float _sfxVolume)
    {
        _currentSettings.SaveAudioSettings(_masterVolume, _musicVolume, _sfxVolume);
        _saveSettingEvent.RaiseEvent();
    }

    public void SaveGraphicsSettings(int newResolutionsIndex, int newAntiAliasingIndex, float newShadowDistance, bool fullscreenState)
    {
        _currentSettings.SaveGraphicsSettings(newResolutionsIndex, newAntiAliasingIndex, newShadowDistance, fullscreenState);
        _saveSettingEvent.RaiseEvent();
    }
}
