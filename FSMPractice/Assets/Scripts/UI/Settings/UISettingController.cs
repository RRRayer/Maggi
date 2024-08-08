using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public enum SettingFieldType
{
    Volume, Master,
    Volume_Music,
    Volume_Sfx,
}

public enum SettingsType
{
    Audio,
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
    }

    private void SaveAudioSettings(float _masterVolume, float _musicVolume, float _sfxVolume)
    {
        _currentSettings.SaveAudioSettings(_masterVolume, _musicVolume, _sfxVolume);
    }

    public void ClosedScreen()
    {
        Closed.Invoke();
    }
}
