using UnityEngine;

[CreateAssetMenu(menuName = "Save Load System")]
public class SaveLoadSystem : ScriptableObject
{
    public string saveFilename = "save.pudding";
    public string backupSaveFilename = "save.pudding.bak";
    public Save saveData = new Save();

    public void SaveDataToDisk()
    {
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
        SaveDataToDisk();
    }
}
