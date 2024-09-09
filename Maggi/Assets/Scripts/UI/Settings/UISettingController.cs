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
}

public enum SettingsType
{
    Audio,
    Graphic,
}

public class UISettingController : MonoBehaviour
{
    [SerializeField] private UISettingsAudioComponent _audioComponent;
    [SerializeField] private SettingSO _currentSettings = default;
    [SerializeField] private InputReader _inputReader = default;
    [SerializeField] private VoidEventChannelSO _saveSettingEvent = default;

    public UnityAction Closed;

    private void OnEnable()
    {
        _audioComponent._save += SaveAudioSettings;

        _inputReader.MenuCloseEvent += CloseScreen;
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

    private void OpenSetting(SettingsType settingType)
    {
        switch (settingType)
        {
        case SettingsType.Audio:
            _audioComponent.Setup(_currentSettings.MasterVolume, _currentSettings.MusicVolume, _currentSettings.SfxVolume);
            break;
        case SettingsType.Graphic:
            break;
        default:
            break;
        }
    }

    private void SaveAudioSettings(float _masterVolume, float _musicVolume, float _sfxVolume)
    {
        _currentSettings.SaveAudioSettings(_masterVolume, _musicVolume, _sfxVolume);
    }

    
}
