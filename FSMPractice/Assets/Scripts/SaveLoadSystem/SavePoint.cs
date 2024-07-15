using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public enum Level
    {
        ONE, TWO, THREE, FOUR
    };

    public Level level = default;

    [SerializeField] private PointSO _savePoint = default;
    [SerializeField] private PointStorageSO _pointStorage = default; // This is where the last point taken will be stored
    [SerializeField] private SaveLoadSystem _saveLoadSystem = default;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("¿˙¿Â");
            _saveLoadSystem.SaveDataToDisk();
        }
    }
}
