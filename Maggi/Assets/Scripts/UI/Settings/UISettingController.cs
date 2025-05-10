using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] private InputReader _inputReader = default;
    
    [Header("Setting UI")]
    [SerializeField] private UIGenericButton _backButton = default;
    
    [Header("Setting")]
    [SerializeField] private UISettingsAudioComponent _audioComponent;
    [SerializeField] private UISettingsGraphicsComponent _graphicsComponent;
    [SerializeField] private SettingsSO _currentSettings = default;
    
    [Header("Broadcasting on")]
    [SerializeField] private VoidEventChannelSO _saveSettingEvent = default;

    public UnityAction Closed;

    private void OnEnable()
    {
        _backButton.Clicked += CloseSettingScreen;
        _audioComponent._save += SaveAudioSettings;
        _graphicsComponent._save += SaveGraphicsSettings;
        //_inputReader.MenuCloseEvent += CloseSettingScreen;

        OpenSetting();
    }

    private void OnDisable()
    {
        _backButton.Clicked -= CloseSettingScreen;
        _audioComponent._save -= SaveAudioSettings;
        _graphicsComponent._save -= SaveGraphicsSettings;
        //_inputReader.MenuCloseEvent -= CloseSettingScreen;
    }

    public void CloseSettingScreen()
    {
        Debug.Log("Close Screen");
        Closed?.Invoke();
    }

    private void OpenSetting()
    {
        _audioComponent.Setup(_currentSettings.MasterVolume, _currentSettings.MusicVolume, _currentSettings.SfxVolume);
        _graphicsComponent.Setup(_currentSettings.ResolutionIndex);
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
