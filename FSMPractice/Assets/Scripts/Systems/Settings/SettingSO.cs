using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Settings/Create New Setting SO")]
public class SettingSO : ScriptableObject
{
    [SerializeField] private float _masterVolume = default;
    [SerializeField] private float _musicVolume = default;
    [SerializeField] private float _sfxVolume = default;
    public float MasterVolume => _masterVolume;
    public float MusicVolume => _musicVolume;
    public float SfxVolume => _sfxVolume;

    public void SaveAudioSettings(float newMasterVolume, float newMusicVolume, float newSfxVolume)
    {
        _masterVolume = newMasterVolume;
        _musicVolume = newMusicVolume;
        _sfxVolume = newSfxVolume;
    }

    public void LoadSavedSettings(Save saveFile)
    {
        _masterVolume = saveFile._masterVolume;
        _musicVolume = saveFile._musicVolume;
        _sfxVolume = saveFile._sfxVolume;
    }
}
