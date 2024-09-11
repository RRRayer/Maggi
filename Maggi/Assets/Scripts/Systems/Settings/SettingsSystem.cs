using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsSystem : MonoBehaviour
{
    [SerializeField] private SettingsSO _currentSettings = default;
    [SerializeField] private SaveLoadSystem _saveLoadSystem = default;

    [Header("Listening to")]
    [SerializeField] private VoidEventChannelSO _saveSettingEvent = default;

    [Header("Broadcasting on")]
    [SerializeField] private FloatEventChannelSO _changeMasterVolumeEventChannel = default;
    [SerializeField] private FloatEventChannelSO _changeMusicVolumeEventChannel = default;
    [SerializeField] private FloatEventChannelSO _changeSfxVolumeEventChannel = default;

    private void Awake()
    {
        _saveLoadSystem.LoadSaveDataFromDisk();
        _currentSettings.LoadSavedSettings(_saveLoadSystem.saveData);
    }

    private void OnEnable()
    {
        _saveSettingEvent.OnEventRaised += SaveSettings;
    }

    private void OnDisable()
    {
        _saveSettingEvent.OnEventRaised -= SaveSettings;
    }

    private void Start()
    {
        // Execute after init volume channels in AudioManager.cs
        SetCurrentSettings();
    }

    private void SetCurrentSettings()
    {
        _changeMasterVolumeEventChannel.RaiseEvent(_currentSettings.MasterVolume);
        _changeMusicVolumeEventChannel.RaiseEvent(_currentSettings.MusicVolume);
        _changeSfxVolumeEventChannel.RaiseEvent(_currentSettings.SfxVolume);
        Resolution currentResolution = Screen.currentResolution;
        if (_currentSettings.ResolutionIndex < Screen.resolutions.Length)
        {
            currentResolution = Screen.resolutions[_currentSettings.ResolutionIndex];
        }
        Screen.SetResolution(currentResolution.width, currentResolution.height, _currentSettings.IsFullScreen);
    }

    private void SaveSettings()
    {
        _saveLoadSystem.SaveDataToDisk();
    }
}
