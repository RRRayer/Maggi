using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Settings/Create New Setting SO")]
public class SettingsSO : ScriptableObject
{
    [SerializeField] private float _masterVolume = 1.0f;
    [SerializeField] private float _musicVolume = 0.8f;
    [SerializeField] private float _sfxVolume = 1.0f;
    [SerializeField] private int _resolutionsIndex = default;
    [SerializeField] private bool _isFullscreen = default;
    [SerializeField] int _antiAliasingIndex = default;
    [SerializeField] float _shadowDistance = default;
    public float MasterVolume => _masterVolume;
    public float MusicVolume => _musicVolume;
    public float SfxVolume => _sfxVolume;
    public int ResolutionIndex => _resolutionsIndex;
    public bool IsFullScreen => _isFullscreen;
    public int AntiAliasingIndex => _antiAliasingIndex;
    public float ShadowDistance => _shadowDistance;

    public void SaveAudioSettings(float newMasterVolume, float newMusicVolume, float newSfxVolume)
    {
        _masterVolume = newMasterVolume;
        _musicVolume = newMusicVolume;
        _sfxVolume = newSfxVolume;
    }

    public void SaveGraphicsSettings(int newResolutionsIndex, int newAntiAliasingIndex, float newShadowDistance, bool fullscreenState)
    {
        _resolutionsIndex = newResolutionsIndex;
        _antiAliasingIndex = newAntiAliasingIndex;
        _shadowDistance = newShadowDistance;
        _isFullscreen = fullscreenState;
    }

    public void LoadSavedSettings(Save saveFile)
    {
        _masterVolume = saveFile._masterVolume;
        _musicVolume = saveFile._musicVolume;
        _sfxVolume = saveFile._sfxVolume;
        _resolutionsIndex = saveFile._resolutionsIndex;
        _isFullscreen = saveFile._isFullscreen;
    }
}
