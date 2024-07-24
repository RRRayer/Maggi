using UnityEngine;

[CreateAssetMenu(menuName = "Save Load System")]
public class SaveLoadSystem : ScriptableObject
{
    [SerializeField] private LoadEventChannelSO _loadLocation = default;
    [SerializeField] private PointStorageSO _pointStorage = default;

    public string saveFilename = "save.pudding";
    public string backupSaveFilename = "save.pudding.bak";
    public Save saveData = new Save();

    private void OnEnable()
    {
        _loadLocation.OnLoadingRequested += CacheLoadLocation;
    }

    private void OnDisable()
    {
        _loadLocation.OnLoadingRequested -= CacheLoadLocation;
    }

    private void CacheLoadLocation(GameSceneSO locationToLoad, bool showLoadingScreen, bool fadeScreen)
    {
        LocationSO locationSO = locationToLoad as LocationSO;
        if (locationSO != null)
        {
            saveData._locationId = locationSO.Guid;
        }

        SaveDataToDisk();
    }

    private void CacheSavePoint()
    {

    }

    public void SaveDataToDisk()
    {
        saveData._pointStorage = _pointStorage;

        if (FileManager.MoveFile(saveFilename, backupSaveFilename))
        {
            if (FileManager.WriteToFile(saveFilename, saveData.ToJson()))
            {
                //Debug.Log("Save successful " + saveFilename);
            }
        }
    }

    public bool LoadSaveDataFromDisk()
    {
        if (FileManager.LoadFromFile(saveFilename, out var json))
        {
            saveData.LoadFromJson(json);
            return true;
        }
        return false;
    }

    public void WriteEmptySaveFile()
    {
        FileManager.WriteToFile(saveFilename, "");
    }

    public void SetNewGameData()
    {
        FileManager.WriteToFile(saveFilename, "");
        if (_pointStorage)
            _pointStorage.lastPointTaken = null;
        SaveDataToDisk();
    }
}
