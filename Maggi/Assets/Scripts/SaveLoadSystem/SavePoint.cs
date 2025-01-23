using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public PointSO PointPath => _savePoint;

    [SerializeField] private LocationSO _location;
    [SerializeField] private PointSO _savePoint = default;
    [SerializeField] private PointStorageSO _pointStorage = default; // This is where the last point taken will be stored
    [SerializeField] private SaveLoadSystem _saveLoadSystem = default;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            _saveLoadSystem.saveData._locationId = _location.Guid;

            Debug.Log($"���� {_saveLoadSystem.saveData._locationId}"); // ���߿� �̰� UI ����ִ°ŷ� ������ ����

            _pointStorage.lastPointTaken = _savePoint;
            _saveLoadSystem.SaveDataToDisk();
        }
    }
}
