using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public string _locationId;
    public PointStorageSO _pointStorage;

    // Settings
    public float _masterVolume = default;
    public float _musicVolume = default;
    public float _sfxVolume = default;

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }
}
