using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Save
{
    public string _locationId;
    public PointStorageSO _pointStorage;

    // Settings
    public float _masterVolume = default;
    public float _musicVolume = default;
    public float _sfxVolume = default;
    public int _resolutionsIndex = default;
    public bool _isFullscreen = default;

    public void SaveSettings(SettingsSO settings)
    {
        _masterVolume = settings.MasterVolume; 
        _musicVolume = settings.MusicVolume;
        _sfxVolume = settings.SfxVolume;
        _resolutionsIndex = settings.ResolutionIndex;
        _isFullscreen = settings.IsFullScreen;
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }
}
