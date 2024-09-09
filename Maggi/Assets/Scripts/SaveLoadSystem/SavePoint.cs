using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public PointSO PointPath => _savePoint;

    [SerializeField] private PointSO _savePoint = default;
    [SerializeField] private PointStorageSO _pointStorage = default; // This is where the last point taken will be stored
    [SerializeField] private SaveLoadSystem _saveLoadSystem = default;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("저장"); // 나중에 이게 UI 띄워주는거로 빠껴야 겠지
            _pointStorage.lastPointTaken = _savePoint;
            _saveLoadSystem.SaveDataToDisk();
        }
    }
}
