using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Settings/Create New Setting SO")]
public class SettingSO : ScriptableObject
{
    [SerializeField] private float _masterVolume = default;
    [SerializeField] private float _musicVolume = default;
    [SerializeField] private float _sfxVolume = default;
    [SerializeField] private int _resolutionsIndex = default;
    [SerializeField] private bool _isFullScreen = default;
    public float MasterVolume => _masterVolume;
    public float MusicVolume => _musicVolume;
    public float SfxVolume => _sfxVolume;
    public int ResolutionIndex => _resolutionsIndex;
    public bool IsFullScreen => _isFullScreen;

    public void SaveAudioSettings(float newMasterVolume, float newMusicVolume, float newSfxVolume)
    {
        _masterVolume = newMasterVolume;
        _musicVolume = newMusicVolume;
        _sfxVolume = newSfxVolume;
    }

    public void SaveGraphicsSettings(int newResolutionsIndex, bool fullscreeState)
    {
        _resolutionsIndex = newResolutionsIndex;
        _isFullScreen = fullscreeState;
    }

    public void LoadSavedSettings(Save saveFile)
    {
        _masterVolume = saveFile._masterVolume;
        _musicVolume = saveFile._musicVolume;
        _sfxVolume = saveFile._sfxVolume;
        _resolutionsIndex = saveFile._resolutionsIndex;
        _isFullScreen = saveFile._isFullscreen;
    }
}
