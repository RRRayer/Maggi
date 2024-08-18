using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsSystem : MonoBehaviour
{
    [SerializeField] private SettingSO _currentSettings = default;
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
        SetCurrentSettings();
    }

    private void OnEnable()
    {
        _saveSettingEvent.OnEventRaised += SaveSettings;
    }

    private void OnDisable()
    {
        _saveSettingEvent.OnEventRaised -= SaveSettings;
    }

    private void SetCurrentSettings()
    {
        _changeMasterVolumeEventChannel.RaiseEvent(_currentSettings.MasterVolume);
        _changeMusicVolumeEventChannel.RaiseEvent(_currentSettings.MusicVolume);
        _changeSfxVolumeEventChannel.RaiseEvent(_currentSettings.SfxVolume);
    }

    private void SaveSettings()
    {
        _saveLoadSystem.SaveDataToDisk();
    }
}