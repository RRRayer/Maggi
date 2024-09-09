using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UISettingsAudioComponent : MonoBehaviour
{
    [SerializeField] private UISettingItemFiller _masterVolumeField;
    [SerializeField] private UISettingItemFiller _musicVolumeField;
    [SerializeField] private UISettingItemFiller _sfxVolumeField;

    [SerializeField] UIGenericButton _saveButton;
    [SerializeField] UIGenericButton _resetButton;

    [Header("Broadcasting on")]
    [SerializeField] private FloatEventChannelSO _masterVolumeEventChannel = default;
    [SerializeField] private FloatEventChannelSO _musicVolumeEventChannel = default;
    [SerializeField] private FloatEventChannelSO _sfxVolumeEventChannel = default;

    private float _savedMasterVolume;
    private float _savedMusicVolume;
    private float _savedSfxVolume;
    private float _masterVolume;
    private float _musicVolume;
    private float _sfxVolume;

    private int _maxVolume = 10;

    public event UnityAction<float, float, float> _save = delegate { };

    private void OnEnable()
    {
        _masterVolumeField.OnNextOption += IncreaseMasterVolume;
        _masterVolumeField.OnPreviousOption += DecreaseMasterVolume;
        _musicVolumeField.OnNextOption += IncreaseMusicVolume;
        _musicVolumeField.OnPreviousOption += DecreaseMusicrVolume;
        _sfxVolumeField.OnNextOption += IncreaseSfxVolume;
        _sfxVolumeField.OnPreviousOption += DecreaseSfxVolume;

        _saveButton.Clicked += SaveVolumes;
        _resetButton.Clicked += ResetVolumes;
    }

    private void OnDisable()
    {
        ResetVolumes();

        _masterVolumeField.OnNextOption -= IncreaseMasterVolume;
        _masterVolumeField.OnPreviousOption -= DecreaseMasterVolume;
        _musicVolumeField.OnNextOption -= IncreaseMusicVolume;
        _musicVolumeField.OnPreviousOption -= DecreaseMusicrVolume;
        _sfxVolumeField.OnNextOption -= IncreaseSfxVolume;
        _sfxVolumeField.OnPreviousOption -= DecreaseSfxVolume;

        _saveButton.Clicked -= SaveVolumes;
        _resetButton.Clicked -= ResetVolumes;
    }

    public void Setup(float masterVolume, float musicVolume, float sfxVolume)
    {
        _masterVolume = masterVolume;
        _musicVolume = sfxVolume;
        _sfxVolume = musicVolume;

        _savedMasterVolume = _masterVolume = masterVolume;
        _savedMusicVolume = _musicVolume = musicVolume;
        _savedSfxVolume = _sfxVolume = sfxVolume;

        SetMasterVolumeField();
        SetMusicVolumeField();
        SetSfxVolumeField();
    }

    private void SetMasterVolumeField()
    {
        // Pagination 包访 肺流阑 贸府
        int paginationCount = _maxVolume + 1; // adding a page in the pagination since the count starts from 0
        int selectedPaginationIndex = Mathf.RoundToInt(_maxVolume * _musicVolume);
        string selectedOption = Mathf.RoundToInt(_maxVolume * _musicVolume).ToString();

        SetMasterVolume();

        //_masterVolumeField.
    }

    private void SetMusicVolumeField()
    {
        // Pagination 包访 肺流阑 贸府

        SetMusicVolume();
    }

    private void SetSfxVolumeField()
    {
        // Pagination 包访 肺流阑 贸府

        SetSfxVolume();
    }

    private void SetMasterVolume()
    {
        _masterVolumeEventChannel.OnEventRaised(_masterVolume);
    }

    private void SetMusicVolume()
    {
        _musicVolumeEventChannel.OnEventRaised(_musicVolume);
    }

    private void SetSfxVolume()
    {
        _sfxVolumeEventChannel.OnEventRaised(_sfxVolume);
    }

    private void IncreaseMasterVolume()
    {
        _masterVolume += 1 / (float)_maxVolume;
        _masterVolume = Mathf.Clamp(_masterVolume, 0, 1);
        Debug.Log("Current Master Volume " + _masterVolume);
        SetMasterVolumeField();
    }

    private void DecreaseMasterVolume()
    {
        _masterVolume -= 1 / (float)_maxVolume;
        _masterVolume = Mathf.Clamp(_masterVolume, 0, 1);
        Debug.Log("Current Master Volume " + _masterVolume);
        SetMasterVolumeField();
    }

    private void IncreaseMusicVolume()
    {
        _musicVolume += 1 / (float)_maxVolume;
        _musicVolume = Mathf.Clamp(_musicVolume, 0, 1);
        Debug.Log("Current Music Volume " + _musicVolume);
        SetMusicVolumeField();
    }

    private void DecreaseMusicrVolume()
    {
        _musicVolume -= 1 / (float)_maxVolume;
        _masterVolume = Mathf.Clamp(_masterVolume, 0, 1);
        Debug.Log("Current Music Volume " + _musicVolume);
        SetMusicVolumeField();
    }

    private void IncreaseSfxVolume()
    {
        _sfxVolume += 1 / (float)_maxVolume;
        _sfxVolume = Mathf.Clamp(_sfxVolume, 0, 1);
        Debug.Log("Current Sfx Volume " + _sfxVolume);
        SetSfxVolumeField();
    }

    private void DecreaseSfxVolume()
    {
        _sfxVolume -= 1 / (float)_maxVolume;
        _sfxVolume = Mathf.Clamp(_sfxVolume, 0, 1);
        Debug.Log("Current Sfx Volume " + _sfxVolume);
        SetSfxVolumeField();
    }

    private void SaveVolumes()
    {
        _savedMasterVolume = _masterVolume;
        _savedMusicVolume = _musicVolume;
        _savedSfxVolume = _sfxVolume;

        _save.Invoke(_masterVolume, _musicVolume, _sfxVolume);
    }

    private void ResetVolumes()
    {
        Setup(_masterVolume, _musicVolume, _sfxVolume);
    }
}
